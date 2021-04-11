using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kuhpik;
using DG.Tweening;
using System.Threading.Tasks;
using Kuhpik.Pooling;
using System;

public class PlayerShootingSystem : GameSystem, IUpdating
{
    [SerializeField] private GameObject bulletPrefab;

    float time = 0;
    void IUpdating.OnUpdate()
    {
        if (!player.canShoot) { return; }

        if (Input.GetMouseButtonDown(0))
        {
            time = 0;
            Shoot();
        }
        if (Input.GetMouseButton(0))
        {
            if(time >= config.GetValue(EGameValue.delayBetweenShoots))
            {
                Shoot();
                time = 0;
            }
            time += Time.deltaTime;
        }
    }

    private void Shoot()
    {
        Ray ray = game.camera.ScreenPointToRay(Input.mousePosition);
        bool hasHit = Physics.Raycast(ray, out RaycastHit hit);
        if (hasHit)
        {
            var posToMove = hit.point;
            var distance = Vector3.Distance(game.playerShootingPoint.position, posToMove);
            var newBullet = PoolingSystem.GetObject(bulletPrefab);
            newBullet.transform.position = game.playerShootingPoint.position;
            newBullet.transform.DOMove(posToMove, distance / config.GetValue(EGameValue.speedOfBullet));
        }
    }
}
