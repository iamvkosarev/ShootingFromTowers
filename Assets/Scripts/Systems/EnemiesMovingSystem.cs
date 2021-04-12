using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kuhpik;

public class EnemiesMovingSystem : GameSystem, IUpdating
{
    [SerializeField] private Vector3 movingZonePos;
    [SerializeField] private Vector3 movingZoneSize;

    void IUpdating.OnUpdate()
    {
        for (int i = 0; i < game.enemies.Count; i++)
        {
            var distanceToTarget = Vector3.Distance(game.enemies[i].transform.position, game.enemies[i].currentMovingPos);
            if (distanceToTarget <= 1f)
            {
                game.enemies[i].currentMovingPos = GetPointInMovingZone();
                game.enemies[i].navMeshAgent.destination = game.enemies[i].currentMovingPos;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green * 0.3f;
        Gizmos.DrawCube(movingZonePos, movingZoneSize);

        Gizmos.color = Color.red;
        if (game.enemies != null)
        {
            for (int i = 0; i < game.enemies.Count; i++)
            {
                Gizmos.DrawSphere(game.enemies[i].currentMovingPos, 0.3f);
                Gizmos.DrawLine(game.enemies[i].currentMovingPos, game.enemies[i].transform.position);
            }
        }
    }
    public Vector3 GetPointInMovingZone()
    {
        Vector3 point = new Vector3(
            Random.Range(transform.position.x + movingZonePos.x - movingZoneSize.x / 2f, transform.position.x + movingZonePos.x + movingZoneSize.x / 2f),
            Random.Range(transform.position.y + movingZonePos.y - movingZoneSize.y / 2f, transform.position.y + movingZonePos.y + movingZoneSize.y / 2f),
            Random.Range(transform.position.z + movingZonePos.z - movingZoneSize.z / 2f, transform.position.z + movingZonePos.z + movingZoneSize.z / 2f));
        return point;
    }

}
