using UnityEngine;

public class VerticalSmashState : BaseBossAttackState
{
    public int damage = 15;
    public VerticalSmashState(BossController boss, BossStateMachine stateMachine) : base(boss, stateMachine, "VerticalSmash", "VerticalSmash")
    {
    }
}
