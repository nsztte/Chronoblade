using UnityEngine;

public class ChronoAttackState : EnemyAttackState
{
    public override void Enter(EnemyStateMachine enemy)
    {
        base.Enter(enemy);
        // 크로노 몽크 특별한 공격 시작 로직
        Debug.Log("ChronoMonk entering attack state");
    }

    public override void Update(EnemyStateMachine enemy)
    {
        // 기본 공격 로직을 먼저 실행
        base.Update(enemy);
        
        // 크로노 몽크 특별한 공격 로직 추가
        // 예: 텔레포트, 시간 조작 등
    }

    public override void Exit(EnemyStateMachine enemy)
    {
        base.Exit(enemy);
        // 크로노 몽크 특별한 공격 종료 로직
    }
}
