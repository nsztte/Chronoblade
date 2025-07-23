using UnityEngine;

public class HorizontalSlashState : BaseBossAttackState
{
    public int damage = 15;
    public HorizontalSlashState(BossController boss, BossStateMachine stateMachine) : base(boss, stateMachine, "HorizontalSlash", "HorizontalSlash")
    {
    }
}
