using UnityEngine;

public class PlayerComboState : PlayerBaseState
{
    private PlayerController playerController;
    private ComboSequence combo;
    private int currentAttackIndex = 0;
    private float comboStartTime;
    private float beatInterval;
    private int upperBodyLayerIndex = 1; // Animator에서 상체 레이어 인덱스

    public PlayerComboState(PlayerStateMachine stateMachine, ComboSequence combo) : base(stateMachine)
    {
        this.combo = combo;
        playerController = stateMachine.playerController;
        beatInterval = TimingComboManager.Instance.BeatInterval;
    }

    public override void Enter()
    {
        // Debug.Log($"[PlayerComboState] 진입: {combo.comboName}");
        currentAttackIndex = 0;
        
        // 콤보 실행 시작 - 입력 버퍼 업데이트 중단
        ComboEvaluator.Instance.StartComboExecution();
        
        PlayCurrentComboAttack();
    }

    public override void Update()
    {
        if (Time.time - comboStartTime >= beatInterval)
        {
            currentAttackIndex++;

            if (currentAttackIndex < combo.attackSequence.Count)
            {
                // 다음 공격 실행
                PlayCurrentComboAttack();
            }
            else
            {
                // 콤보 완료
                stateMachine.ChangeState(new PlayerLocomotionState(stateMachine));
            }
        }

        playerController.LocomotionUpdate();
    }

    public override void Exit()
    {
        // Debug.Log($"[PlayerComboState] 종료: {combo.comboName}");
        TimingComboManager.Instance.StopBeatRoutine(); // 비트 루프는 계속 실행
        PlayerManager.Instance.SetAnimatorFloat("AttackSpeed", 1f);
        
        // 콤보 실행 종료 - 입력 버퍼 클리어 및 업데이트 재개
        ComboEvaluator.Instance.EndComboExecution();
    }

    // TODO: 공격 이펙트, 데미지, 넉백 등 처리
    private void PlayCurrentComboAttack()
    {
        var attackData = combo.attackSequence[currentAttackIndex];

        // // 애니메이션 속도 조절 (비트 길이에 맞춰 동기화)
        // float animLength = attackData.animationClip.length;
        // float speed = animLength / beatInterval;
        // PlayerManager.Instance.SetAnimatorFloat("AttackSpeed", speed);
        // // 공격 애니메이션 실행 (상체 전용 레이어에서)
        // stateMachine.animator.CrossFadeInFixedTime(
        //     attackData.animationClip.name,
        //     0.05f,
        //     upperBodyLayerIndex
        // );
        

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
