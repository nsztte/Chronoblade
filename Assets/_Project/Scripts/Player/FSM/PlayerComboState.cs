using UnityEngine;

public class PlayerComboState : PlayerBaseState
{
    private ComboSequence combo;
    private int currentAttackIndex = 0;
    
    public PlayerComboState(PlayerStateMachine stateMachine, ComboSequence combo) : base(stateMachine)
    {
        this.combo = combo;
    }

    public override void Enter()
    {
        Debug.Log($"[콤보] {combo.comboName} 시작");
        currentAttackIndex = 0;
        PlayCurrentComboAttack();
    }

    public override void Update()
    {
        // 콤보 애니메이션/공격이 끝났는지 체크
        if (IsCurrentAttackFinished())
        {
            currentAttackIndex++;
            if (currentAttackIndex < combo.attackSequence.Count)
            {
                PlayCurrentComboAttack();
            }
            else
            {
                // 콤보 종료 → 이동 상태로 복귀
                stateMachine.ChangeState(new PlayerLocomotionState(stateMachine));
            }
        }
    }

    public override void Exit()
    {
        Debug.Log($"[콤보] {combo.comboName} 종료");
        // 필요 시 상태 정리
    }

    private void PlayCurrentComboAttack()
    {
        var attackData = combo.attackSequence[currentAttackIndex];
        // 애니메이션, 이펙트, 데미지 등 처리
        Debug.Log($"[콤보] {combo.comboName} - {currentAttackIndex + 1}타: {attackData.attackType}");
        // playerController.PlayComboAnimation(attackData.animationClip);
        // playerController.ApplyComboDamage(attackData.damage);
    }

    private bool IsCurrentAttackFinished()
    {
        // 실제로는 애니메이션/타이밍/이펙트 등과 연동
        // 여기서는 예시로 간단히 시간 체크 등으로 대체 가능
        // return playerController.IsComboAttackFinished();
        return true; // 임시: 바로 다음 타로 넘어감
    }
}
