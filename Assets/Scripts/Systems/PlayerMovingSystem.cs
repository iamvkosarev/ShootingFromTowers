using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Kuhpik;
using System;

public class PlayerMovingSystem : GameSystem, IUpdating
{
    void IUpdating.OnUpdate()
    {
        if (player.canShoot) { return; }
        if (Input.GetMouseButton(0))
        {
            MoveToTouchedPoint();
        }
        ChangeSpeed();
    }

    private void ChangeSpeed()
    {
        game.playerElements.navMeshAgent.speed = config.GetValue(EGameValue.playerSpeed);
        game.playerElements.navMeshAgent.angularSpeed = config.GetValue(EGameValue.playerRotatingSpeed);
    }

    private void MoveToTouchedPoint()
    {

        Ray ray = game.camera.ScreenPointToRay(Input.mousePosition);
        bool hasHit = Physics.Raycast(ray, out RaycastHit hit);
        if (hasHit)
        {
            game.playerTarget.position = hit.point;
            game.playerElements.navMeshAgent.destination = hit.point;
        }
    }
}
