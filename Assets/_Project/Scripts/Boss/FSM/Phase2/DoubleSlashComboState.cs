using UnityEngine;

public class DoubleSlashComboState : BaseBossAttackState
{
    public int damage = 10;
    public DoubleSlashComboState(BossController boss, BossStateMachine stateMachine) : base(boss, stateMachine, "DoubleSlashCombo", "DoubleSlashCombo")
    {
    }
}
