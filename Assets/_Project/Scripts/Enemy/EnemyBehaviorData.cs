using UnityEngine;

public enum EnemyType
{
    Watcher,
    ChronoMonk,
    MirrorDuelist
}

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/BehaviorData")]
public class EnemyBehaviorData : ScriptableObject
{
    public EnemyType enemyType;

    [Header("Default")]
    public int maxHP;
    public int damage;
    public float moveSpeed;
    public float detectionRange;
    public float attackRange;
    public float attackCooldown;
    public float attackSpeed;

    [Header("ChronoMonk")]
    public float teleportDistance;
    public float slowDuration;
    public float retreatRange = 3f;

    [Header("ChronoMonk 프로젝타일")]
    public GameObject chronoProjectilePrefab;
    public float projectileSpeed = 15f;
    public float projectileLifetime = 5f;

    [Header("Mirror Duelist")]
    public GameObject fakeClonePrefab;
    public int numberOfClones;
    public float cloneLifeTime;
    public float cloneSpread;

    public EnemyAttackState CreateAttackState()
    {
        return enemyType switch
        {
            EnemyType.Watcher => new EnemyAttackState(),
            EnemyType.ChronoMonk => new ChronoAttackState(),
            EnemyType.MirrorDuelist => new MirrorAttackState(),
            _ => new EnemyAttackState()
        };
    }
}
