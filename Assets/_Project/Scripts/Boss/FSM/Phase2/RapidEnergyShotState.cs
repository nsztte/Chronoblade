using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapidEnergyShotState : BaseBossAttackState
{
    private bool hasFired = false;

    public RapidEnergyShotState(BossController boss, BossStateMachine stateMachine) : base(boss, stateMachine, "RapidEnergyShot", "RapidEnergyShot")
    {
    }

    public override void Update()
    {
        base.Update();

        if(!isWindingUp && !hasFired)
        {
            hasFired = true;
            boss.StartCoroutine(FireEnergyBolts());
        }
    }

    private IEnumerator FireEnergyBolts()
    {
        yield return new WaitForSeconds(0.2f);

        int boltCount = 3;
        float radius = 2f;
        float interval = 0.3f;

        var positions = GetBoltPositions(boltCount, radius, boss.transform.position);

        foreach(var position in positions)
        {
            Vector3 targetPosition = boss.Player.position;
            Collider targetCollider = boss.Player.GetComponent<Collider>();

            if(targetCollider != null)
            {
                targetPosition.y = targetCollider.bounds.center.y;
            }

            Vector3 direction = (targetPosition - position).normalized;
            boss.SpawnEnergyBolt(position, direction);
            yield return new WaitForSeconds(interval);
        }
    }

    private List<Vector3> GetBoltPositions(int count, float radius, Vector3 center)
    {
        List<Vector3> positions = new List<Vector3>();

        for(int i = 0; i < count; i++)
        {
            float angle = i * 2 * Mathf.PI / count;
            Vector3 direction = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
            Vector3 position = center + direction * radius + Vector3.up * 2f; // 테스트하면서 보스 높이에 맞추기
            positions.Add(position);
        }

        return positions;
    }
}
