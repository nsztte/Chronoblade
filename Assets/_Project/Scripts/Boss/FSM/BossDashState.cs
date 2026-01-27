using UnityEngine;

public class BossDashState : BaseBossState
{
    private readonly Transform player;
    private readonly BaseBossState nextState;

    private Vector3 startPos;
    private Vector3 targetPos;
    private Vector3 direction;

    private readonly float dashSpeed;
    private readonly float stoppingDistance;
    private readonly float lookAtSpeed;

    public BossDashState(
        BossController boss,
        BossStateMachine stateMachine,
        BaseBossState nextState
    ) : this(boss, stateMachine, nextState, dashSpeed: 15f, stoppingDistance: 3f, lookAtSpeed: 12f)
    {
    }

    public BossDashState(
        BossController boss,
        BossStateMachine stateMachine,
        BaseBossState nextState,
        float dashSpeed,
        float stoppingDistance,
        float lookAtSpeed = 12f
        ) : base(boss, stateMachine)
    {
        this.player = boss.Player;
        this.nextState = nextState;

        this.dashSpeed = Mathf.Max(0.1f, dashSpeed);
        this.stoppingDistance = Mathf.Max(0.0f, stoppingDistance);
        this.lookAtSpeed = Mathf.Max(0.0f, lookAtSpeed);
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
        boss.LookAtPlayer(lookAtSpeed);

        boss.transform.position += direction * dashSpeed * Time.deltaTime;

        if(Vector3.Distance(boss.transform.position, targetPos) <= stoppingDistance)
        {
            boss.PlayAnimation("IsDash", false);
            stateMachine.ChangeState(nextState);
        }
    }
}
