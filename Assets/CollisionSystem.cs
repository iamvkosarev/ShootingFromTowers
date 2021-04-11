using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kuhpik;
using NaughtyAttributes;
using System;
using Supyrb;

public class CollisionSystem : GameSystem, IFixedUpdating
{
    [SerializeField] private float bulletRadius;
    [ReadOnly] public List<BulletComponent> bullets;
    Action<EnemieElementsComponent, Vector3> enemieHitCollisionAction;
    private void OnEnable()
    {
        enemieHitCollisionAction = (enemieElements, attackPos) =>
        {
            ChangeEnemieRagdollState(enemieElements, true);
            enemieElements.navMeshAgent.enabled = false;
        };
        Signals.Get<OnHitEnemieSignal>().AddListener(enemieHitCollisionAction);
    }
    private void OnDisable()
    {

        Signals.Get<OnHitEnemieSignal>().RemoveListener(enemieHitCollisionAction);
        enemieHitCollisionAction = null;
    }

    void IFixedUpdating.OnFixedUpdate()
    {
        bullets = game.bullets;
        foreach (var bullet in game.bullets)
        {
            if (!bullet.canCheck) { continue; }
            Collider[] colliders = Physics.OverlapSphere(bullet.transform.position, bulletRadius);
            foreach (var collider in colliders)
            {
                Debug.Log(collider.gameObject.name);
                HitBoxComponent hitBoxComponent = collider.gameObject.GetComponent<HitBoxComponent>();
                if (hitBoxComponent)
                {
                    hitBoxComponent.ActivateAttack(bullet.transform.position);
                }
            }
            if(colliders.Length != 0 || bullet.transform.position.y < -5f)
            {
                StartCoroutine(WaitBeforeVFXEnded(bullet));
            }
            
        }
    }

    IEnumerator WaitBeforeVFXEnded(BulletComponent bullet)
    {
        bullet.canCheck = false;
        bullet.movingVelocity = Vector3.zero;
        yield return new WaitForSeconds(0.6f);
        bullet.gameObject.SetActive(false);
    }

    public void ChangeEnemieRagdollState(EnemieElementsComponent enemie, bool activateRagdoll)
    {
        foreach (var rb in enemie.ragdollsRigidbodies)
        {
            rb.isKinematic = !activateRagdoll;
            rb.useGravity = activateRagdoll;
        }
        enemie.animator.enabled = !activateRagdoll;
    }
}
