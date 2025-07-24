using UnityEngine;

public class BossDashState : BaseBossState
{
    private readonly Transform player;
    private readonly BaseBossState nextState;

    private Vector3 startPos;
    private Vector3 targetPos;
    private Vector3 direction;
    private float dashSpeed = 15f;
    private float stoppingDistance = 3f;

    public BossDashState(BossController boss, BossStateMachine stateMachine, BaseBossState nextState) : base(boss, stateMachine)
    {
        this.player = boss.Player;
        this.nextState = nextState;
    }

    public override void Enter()
    {
        startPos = boss.transform.position;
        targetPos = player.position;
        direction = (targetPos - startPos).normalized;
        boss.PlayAnimation("IsDash", true);
    }

    public override void Update()
    {
        boss.LookAtPlayer(12f);

        boss.transform.position += direction * dashSpeed * Time.deltaTime;

        if(Vector3.Distance(boss.transform.position, targetPos) <= stoppingDistance)
        {
            boss.PlayAnimation("IsDash", false);
            stateMachine.ChangeState(nextState);
        }
    }
}
