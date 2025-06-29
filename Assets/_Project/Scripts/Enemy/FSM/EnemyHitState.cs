using UnityEngine;
using System.Collections.Generic;

// 피격 행동을 정의하는 인터페이스
public interface IHitBehavior
{
    void HandleHit(EnemyStateMachine enemy, float elapsed, float hitDuration);
}

// 기본 피격 행동
public class DefaultHitBehavior : IHitBehavior
{
    public void HandleHit(EnemyStateMachine enemy, float elapsed, float hitDuration)
    {
        if (elapsed >= hitDuration)
        {
            enemy.TransitionToState(enemy.ChaseState);
        }
    }
}

// ChronoMonk 전용 피격 행동 (2배 긴 피격 시간 후 AttackState로 전환)
public class ChronoMonkHitBehavior : IHitBehavior
{
    public void HandleHit(EnemyStateMachine enemy, float elapsed, float hitDuration)
    {
        // 0.2초 미만이면 아무것도 하지 않음 (경직 애니메이션 재생)
        if (elapsed < hitDuration) return;
        
        // 0.4초 이상이면 AttackState로 전환
        if (elapsed >= hitDuration * 2f)
        {
            enemy.TransitionToState(enemy.AttackState);
            return;
        }
        
        // 0.2초 ~ 0.4초 구간에서는 ChaseState로 전환
        enemy.TransitionToState(enemy.ChaseState);
    }
}

public class EnemyHitState : EnemyBaseState
{
    private float hitDuration = 0.2f;
    private float elapsed = 0f;
    private float lastHitTime = 0f;
    
    // 피격 행동 전략 Dictionary
    private static readonly Dictionary<EnemyType, IHitBehavior> hitBehaviors = new Dictionary<EnemyType, IHitBehavior>
    {
        { EnemyType.ChronoMonk, new ChronoMonkHitBehavior() },
        // Watcher, MirrorDuelist는 DefaultHitBehavior 사용 (Dictionary에 없으면 자동으로 기본값 사용)
    };
    
    public override void Enter(EnemyStateMachine enemy)
    {
        enemy.Agent.isStopped = true;
        enemy.Animator.SetTrigger("IsHit");
        elapsed = 0f;
        lastHitTime = Time.time;
    }

    public override void Update(EnemyStateMachine enemy)
    {
        elapsed += enemy.Enemy.GetAdjustedDeltaTime();
        
        // 적 타입에 따른 피격 행동 실행
        var behavior = hitBehaviors.GetValueOrDefault(enemy.Enemy.Type, new DefaultHitBehavior());
        behavior.HandleHit(enemy, elapsed, hitDuration);
    }

    public override void Exit(EnemyStateMachine enemy)
    {
        enemy.Agent.isStopped = false;
    }
}
