using UnityEngine;

public class MirrorAttackState : EnemyAttackState
{
    public override void Enter(EnemyStateMachine enemy)
    {
        base.Enter(enemy);
        // 미러 듀얼리스트 특별한 공격 시작 로직
        Debug.Log("MirrorDuelist entering attack state");
    }

    public override void Update(EnemyStateMachine enemy)
    {
        // 기본 공격 로직을 먼저 실행
        base.Update(enemy);
        
        // 미러 듀얼리스트 특별한 공격 로직 추가
        // 예: 클론 생성, 미러링 공격 등
    }

    public override void Exit(EnemyStateMachine enemy)
    {
        base.Exit(enemy);
        // 미러 듀얼리스트 특별한 공격 종료 로직
    }
}
