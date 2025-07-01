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
        Debug.Log($"[PlayerComboState] 진입: {combo.comboName}");
        currentAttackIndex = 0;
        PlayCurrentComboAttack();
    }

    public override void Update()
    {
        if (Time.time - comboStartTime >= beatInterval)
    {
        currentAttackIndex++;

        if (currentAttackIndex < combo.attackSequence.Count)
        {
            // 먼저 애니메이션 실행 (공격 동기화)
            PlayCurrentComboAttack();

            // 다음 공격을 실행했지만, 입력이 안 맞았으면 즉시 종료
            if (!ComboEvaluator.Instance.IsValidStep(combo, currentAttackIndex))
            {
                Debug.Log("[PlayerComboState] 콤보 입력 불일치 → 상태 종료");
                stateMachine.ChangeState(new PlayerLocomotionState(stateMachine));
            }
        }
        else
        {
            stateMachine.ChangeState(new PlayerLocomotionState(stateMachine));
        }
    }

    playerController.LocomotionUpdate();
    }

    public override void Exit()
    {
        Debug.Log($"[PlayerComboState] 종료: {combo.comboName}");
        TimingComboManager.Instance.StopBeatRoutine();
        PlayerManager.Instance.SetAnimatorFloat("AttackSpeed", 1f);
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

        // 공격 시작 시간 기록
        comboStartTime = Time.time;

        // 디버그 출력
        Debug.Log($"[콤보] {combo.comboName} - {currentAttackIndex + 1}타: {attackData.attackType}");
    }
}
