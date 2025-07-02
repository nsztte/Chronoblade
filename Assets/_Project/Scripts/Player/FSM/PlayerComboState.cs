using UnityEngine;

public class PlayerComboState : PlayerBaseState
{
    private PlayerController playerController;
    private ComboSequence combo;
    private int currentAttackIndex = 0;
    private float comboStartTime;
    private float beatInterval;
    private int upperBodyLayerIndex = 1; // Animator에서 상체 레이어 인덱스
    private bool isWaitingForInput = true; // 첫 번째 입력 대기 상태

    public PlayerComboState(PlayerStateMachine stateMachine, ComboSequence combo) : base(stateMachine)
    {
        this.combo = combo;
        playerController = stateMachine.playerController;
        beatInterval = TimingComboManager.Instance.BeatInterval;
    }

    public override void Enter()
    {
        Debug.Log($"[PlayerComboState] 진입: {combo.comboName}");
        currentAttackIndex = 0;
        isWaitingForInput = true;
        comboStartTime = Time.time;
        
        // 첫 번째 공격은 자동 실행하지 않고 입력 대기
        // PlayCurrentComboAttack(); // 이 줄 제거
        
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
            weapon.SetAttackingFalse(); // 상태 전이 시 무조건 공격 상태 해제
        }
        
        Debug.Log($"[PlayerComboState] 종료: {combo.comboName}");
        PlayerManager.Instance.SetAnimatorFloat("AttackSpeed", 1f);

        // 입력 이벤트 구독 해제
        if (WeaponManager.Instance.CurrentWeapon?.weaponData.weaponType == WeaponType.Sword)
        {
            InputManager.Instance.OnLightAttackPressed -= OnLightAttack;
            InputManager.Instance.OnHeavyAttackPressed -= OnHeavyAttack;
        }

        stateMachine.animator.CrossFadeInFixedTime("Empty", 0.1f, upperBodyLayerIndex);
        stateMachine.animator.ResetTrigger("IsAttacking");
    }

    public override void Update()
    {
        // 타이밍 체크 - 일정 시간 내에 입력이 없으면 콤보 실패
        if (Time.time - comboStartTime > TimingComboManager.Instance.GetComboWindow())
        {
            Debug.Log("[PlayerComboState] 콤보 실패 - 입력 시간 초과");
            stateMachine.ChangeState(new PlayerLocomotionState(stateMachine));
            return;
        }

        playerController.LocomotionUpdate();
    }

    private void OnLightAttack()
    {
        TryContinueCombo(AttackType.Light);
    }

    private void OnHeavyAttack()
    {
        TryContinueCombo(AttackType.Heavy);
    }

    private void TryContinueCombo(AttackType input)
    {
        // 타이밍 판정
        var (result, damageMultiplier, absOffset) = TimingComboManager.Instance.JudgeTiming(Time.time);
        
        if (result == TimingComboManager.TimingResult.Miss)
        {
            Debug.Log($"[PlayerComboState] 콤보 실패 - 타이밍 Miss");
            stateMachine.ChangeState(new PlayerLocomotionState(stateMachine));
            return;
        }

        if (isWaitingForInput)
        {
            // 첫 번째 입력 처리
            isWaitingForInput = false;
            var expectedAttack = combo.attackSequence[currentAttackIndex].attackType;
            if (input == expectedAttack)
            {
                // 첫 번째 공격 실행
                PlayCurrentComboAttack();
            }
            else
            {
                Debug.Log($"[PlayerComboState] 콤보 실패 - 잘못된 첫 번째 입력 (예상: {expectedAttack}, 입력: {input})");
                stateMachine.ChangeState(new PlayerLocomotionState(stateMachine));
            }
        }
        else
        {
            // 다음 콤보 입력 처리
            currentAttackIndex++;
            
            if (currentAttackIndex < combo.attackSequence.Count)
            {
                // 다음 공격이 올바른지 확인
                var expectedAttack = combo.attackSequence[currentAttackIndex].attackType;
                if (input == expectedAttack)
                {
                    // 다음 공격 실행
                    PlayCurrentComboAttack();
                }
                else
                {
                    Debug.Log($"[PlayerComboState] 콤보 실패 - 잘못된 입력 (예상: {expectedAttack}, 입력: {input})");
                    stateMachine.ChangeState(new PlayerLocomotionState(stateMachine));
                }
            }
            else
            {
                // 콤보 완료
                Debug.Log($"[PlayerComboState] 콤보 완료: {combo.comboName}");
                stateMachine.ChangeState(new PlayerLocomotionState(stateMachine));
            }
        }
    }

    // TODO: 공격 이펙트, 데미지, 넉백 등 처리
    private void PlayCurrentComboAttack()
    {
        var attackData = combo.attackSequence[currentAttackIndex];

        // 애니메이션 속도 조절 (비트 길이에 맞춰 동기화)
        float animLength = attackData.animationClip.length;
        float speed = animLength / beatInterval;
        PlayerManager.Instance.SetAnimatorFloat("AttackSpeed", speed);
        // 공격 애니메이션 실행 (상체 전용 레이어에서)
        stateMachine.animator.CrossFadeInFixedTime(
            attackData.animationClip.name,
            0.05f,
            upperBodyLayerIndex
        );

        // 타이밍 판정 (TimingComboManager로 위임)
        var (result, damageMultiplier, absOffset) = TimingComboManager.Instance.JudgeTiming(Time.time);

        if (result == TimingComboManager.TimingResult.Miss)
        {
            Debug.Log($"[{currentAttackIndex+1}타] 판정: Miss, 콤보 종료");
            stateMachine.ChangeState(new PlayerLocomotionState(stateMachine));
            return;
        }

        float finalDamage = attackData.damage * damageMultiplier;
        ApplyDamage(finalDamage, attackData);

        // 공격 시작 시간 기록
        comboStartTime = Time.time;

        // 디버그 출력
        Debug.Log($"[콤보] {combo.comboName} - {currentAttackIndex + 1}타: {attackData.attackType}, 판정: {result}, 데미지: {finalDamage:F1} (배율: {damageMultiplier:F1}, absOffset: {absOffset:F3})");
    }

    private void ApplyDamage(float damage, ComboAttackData attackData)
    {
        // TODO: 실제 데미지 적용 로직
        // 예: 플레이어 주변의 적들을 찾아서 데미지 전달
        // 예: Physics.OverlapSphere나 Raycast를 사용하여 적 감지
        
        // 임시로 콘솔에 출력
        // Debug.Log($"[데미지] {damage:F1} 데미지 적용 (넉백: {attackData.knockbackPower})");
    }
}
