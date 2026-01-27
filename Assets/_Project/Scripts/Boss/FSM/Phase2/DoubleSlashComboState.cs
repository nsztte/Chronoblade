using UnityEngine;

public class DoubleSlashComboState : BaseBossAttackState
{
    public int damage = 10;
    public DoubleSlashComboState(BossController boss, BossStateMachine stateMachine) : base(boss, stateMachine, "DoubleSlashCombo", "DoubleSlashCombo")
    {
    }

    public override void Enter()
    {
        if(!boss.IsPlayerInAttackRange(boss.Phase2AttackRange))
        {
            stateMachine.ChangeState(new BossDashState(boss, stateMachine, this));
            return;
        }

        base.Enter();
    }

}
