using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kuhpik;
using DG.Tweening;
using System.Threading.Tasks;
using Kuhpik.Pooling;
using System;

public class PlayerShootingSystem : GameSystem, IUpdating, IIniting, IFixedUpdating
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int maxBulletsOnScene;
    [SerializeField] private float timeWhileBulletInactive;
    private Queue<BulletComponent> bulletsPool;

    float time = 0;

    void IIniting.OnInit()
    {
        bulletsPool = new Queue<BulletComponent>();
        for (int i = 0; i < maxBulletsOnScene; i++)
        {
            GameObject newBullet = Instantiate(bulletPrefab);
            newBullet.SetActive(false);
            bulletsPool.Enqueue(newBullet.GetComponent<BulletComponent>());
        }
    }
    void IFixedUpdating.OnFixedUpdate()
    {
        foreach (var bullet in game.bullets)
        {
            if (!bullet.gameObject.active) { continue; }
            bullet.transform.position += bullet.movingVelocity * Time.deltaTime;
        }
    }
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
            var newBullet = bulletsPool.Dequeue();
            newBullet.gameObject.SetActive(true);
            newBullet.movingEffect.Stop();
            if (!game.bullets.Contains(newBullet))
            {
                game.bullets.Add(newBullet);
            }
            bulletsPool.Enqueue(newBullet);
            StartCoroutine(WaitWhileInactive(newBullet));
            newBullet.transform.position = game.playerShootingPoint.position;
            newBullet.movingEffect.Play();
            newBullet.movingVelocity = config.GetValue(EGameValue.speedOfBullet) * (posToMove - game.playerShootingPoint.position).normalized;
            //newBullet.transform.DOMove(posToMove, distance / config.GetValue(EGameValue.speedOfBullet));

        }
    }

    IEnumerator WaitWhileInactive(BulletComponent bullet)
    {
        bullet.canCheck = false;
        yield return new WaitForSeconds(timeWhileBulletInactive);
        bullet.canCheck = true;
    }
}
