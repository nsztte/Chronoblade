using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerComboState : PlayerBaseState
{
    private PlayerController playerController;
    private List<ComboSequence> candidateCombos = new List<ComboSequence>();
    private List<AttackType> inputSequence = new List<AttackType>();
    private ComboSequence currentCombo = null;
    private int currentAttackIndex = 0;
    private float comboStartTime;
    private float beatInterval;
    private int upperBodyLayerIndex = 1; // Animator에서 상체 레이어 인덱스
    private bool isWaitingForInput = false;
    private bool isLastAttackPlaying = false; // 마지막 공격 애니메이션 재생 중인지 확인


    public PlayerComboState(PlayerStateMachine stateMachine, ComboSequence initialCombo) : base(stateMachine)
    {
        playerController = stateMachine.PlayerController;
        beatInterval = TimingComboManager.Instance.BeatInterval;
        // 첫 입력에 맞는 후보군을 모두 저장
        inputSequence.Clear();
        inputSequence.Add(initialCombo.attackSequence[0].attackType);
        candidateCombos = ComboEvaluator.Instance.GetMatchingCombos(inputSequence);
        currentCombo = null;
        currentAttackIndex = 0;
    }

    public override void Enter()
    {
        Debug.Log($"[PlayerComboState] 진입: 후보군 {candidateCombos.Count}개");
        comboStartTime = Time.time;
        isWaitingForInput = false;
        // 첫타 자동 실행
        ExecuteCurrentAttack();
        isWaitingForInput = true;
        // 입력 이벤트 구독
        if (WeaponManager.Instance.CurrentWeapon?.weaponData.weaponType == WeaponType.Sword)
        {
            InputManager.Instance.OnLightAttackPressed += OnLightAttack;
            InputManager.Instance.OnHeavyAttackPressed += OnHeavyAttack;
        }
    }

    public override void Exit()
    {
        var weapon = WeaponManager.Instance.CurrentWeapon;
        if (weapon != null)
        {
            weapon.SetAttackingFalse();
        }
        Debug.Log($"[PlayerComboState] 종료");
        PlayerManager.Instance.SetAnimatorFloat("AttackSpeed", 1f);
        if (WeaponManager.Instance.CurrentWeapon?.weaponData.weaponType == WeaponType.Sword)
        {
            InputManager.Instance.OnLightAttackPressed -= OnLightAttack;
            InputManager.Instance.OnHeavyAttackPressed -= OnHeavyAttack;
        }
        stateMachine.Animator.CrossFadeInFixedTime("SwordIdle", 0.25f, upperBodyLayerIndex);
        stateMachine.Animator.ResetTrigger("IsAttacking");
        
        // 상태 초기화
        isLastAttackPlaying = false;
        isWaitingForInput = false;
    }

    public override void Update()
    {
        // 마지막 공격 애니메이션이 재생 중이면 완료 확인
        if (isLastAttackPlaying)
        {
            CheckLastAttackAnimationComplete();
        }
        else if (isWaitingForInput && Time.time - comboStartTime > TimingComboManager.Instance.GetComboWindow())
        {
			Debug.Log("[PlayerComboState] 콤보 실패 - 입력 시간 초과");
			TimingComboManager.Instance.MissFeedback();
			stateMachine.ChangeState(new PlayerLocomotionState(stateMachine));
            return;
        }
        playerController.LocomotionUpdate();
    }

    private void OnLightAttack()
    {
        HandleComboInput(AttackType.Light);
    }

    private void OnHeavyAttack()
    {
        HandleComboInput(AttackType.Heavy);
    }

    private void HandleComboInput(AttackType input)
    {
        if (!isWaitingForInput) return;
        inputSequence.Add(input);
        var newCandidates = ComboEvaluator.Instance.GetMatchingCombos(inputSequence);
        if (newCandidates.Count == 0)
        {
            Debug.Log($"[PlayerComboState] 콤보 실패 - 후보군 없음");
            TimingComboManager.Instance.MissFeedback();
            stateMachine.ChangeState(new PlayerLocomotionState(stateMachine));
            return;
        }
        candidateCombos = newCandidates;
        currentAttackIndex++;
        if (candidateCombos.Count == 1)
        {
            currentCombo = candidateCombos[0];
        }
        ExecuteCurrentAttack();
    }

    private void CheckLastAttackAnimationComplete()
    {
        // 현재 재생 중인 애니메이션 상태 정보 가져오기
        AnimatorStateInfo stateInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(upperBodyLayerIndex);
        
        // 애니메이션이 완료되었는지 확인 (normalizedTime >= 1.0f)
        if (stateInfo.normalizedTime >= 1.0f)
        {
            Debug.Log("[PlayerComboState] 마지막 공격 애니메이션 완료, 상태 전환");
            isLastAttackPlaying = false;
            stateMachine.ChangeState(new PlayerLocomotionState(stateMachine));
        }
    }

    private void ExecuteCurrentAttack()
    {
        ComboSequence comboToUse = currentCombo ?? candidateCombos[0];
        var attackData = comboToUse.attackSequence[currentAttackIndex];
        float staminaCost = attackData.attackType == AttackType.Light ? PlayerManager.Instance.StaminaCost : PlayerManager.Instance.StaminaCost * 2;
        
        // 스태미너 체크
        if (!PlayerManager.Instance.UseStaminaIfAvailable(staminaCost))
        {
            Debug.Log("[콤보] 스태미너 부족으로 공격 실패");
            TimingComboManager.Instance.MissFeedback();
            stateMachine.ChangeState(new PlayerLocomotionState(stateMachine));
            return;
        }
        
        // 콤보 타격 시작 시 hitTargets 클리어 (각 타격마다 새로운 타격 기회 제공)
        var meleeWeapon = WeaponManager.Instance.CurrentWeapon as MeleeWeaponController;
        meleeWeapon?.ClearHitTargets();
        
        // 타이밍 판정 및 데미지 계산
        var (result, damageMultiplier, absOffset) = TimingComboManager.Instance.JudgeTiming(Time.time);
        if (result == TimingComboManager.TimingResult.Miss)
        {
            Debug.Log($"[{currentAttackIndex+1}타] 판정: Miss, 콤보 종료");
            stateMachine.ChangeState(new PlayerLocomotionState(stateMachine));
            return;
        }
        else if (result == TimingComboManager.TimingResult.Unavailable)
        {
            Debug.Log($"[{currentAttackIndex+1}타] 판정: Unavailable, 콤보 종료");
            stateMachine.ChangeState(new PlayerLocomotionState(stateMachine));
            return;
        }

        // 콤보 성공 시 무적 시간 부여
        PlayerManager.Instance.OnComboAttackSuccess(result);

        // 마지막 타인지 확인
        bool isLastAttack = attackData.isFinalHit;
        
        // 타이밍 배율을 적용한 데미지 계산
        float finalDamage = attackData.damage * damageMultiplier;

        PlayerManager.Instance.SetCurrentCombo(finalDamage, attackData);

        // 애니메이션 속도 조절 (비트 길이에 맞춰 동기화)
        float animLength = attackData.animationClip.length;
        float speed;
        if (isLastAttack && currentCombo != null)
        {
            speed = currentCombo.lastAttackAnimSpeed > 0 ? currentCombo.lastAttackAnimSpeed : 1f;
        }
        else
        {
            speed = animLength / beatInterval;
        }
        PlayerManager.Instance.SetAnimatorFloat("AttackSpeed", speed);
        stateMachine.Animator.CrossFadeInFixedTime(
            attackData.animationClip.name,
            0.05f,
            upperBodyLayerIndex
        );
        
        comboStartTime = Time.time;
        Debug.Log($"[콤보] {comboToUse.comboName} - {currentAttackIndex + 1}타: {attackData.attackType}, 판정: {result}, 데미지: {finalDamage:F1} (배율: {damageMultiplier:F1}, absOffset: {absOffset:F3})");

        if(isLastAttack)
        {
            Debug.Log($"[PlayerComboState] 막타 애니메이션 시작 - 애니메이션 완료까지 대기");
            isLastAttackPlaying = true; // 마지막 공격 애니메이션 재생 시작
            isWaitingForInput = false; // 입력 대기 상태 해제
        }
        else
        {
            isWaitingForInput = true;
        }
    }
}
