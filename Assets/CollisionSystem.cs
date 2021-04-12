using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kuhpik;
using NaughtyAttributes;
using System;
using Supyrb;

public class CollisionSystem : GameSystem, IFixedUpdating
{
    [ReadOnly] public List<BulletComponent> bullets;
    Action<EnemieElementsComponent, Vector3> enemieHitCollisionAction;
    private void OnEnable()
    {
        enemieHitCollisionAction = (enemieElements, attackPos) =>
        {
            if (!enemieElements.isFallen)
            {
                enemieElements.isFallen = true;
                ChangeEnemieRagdollState(enemieElements, true);
                enemieElements.navMeshAgent.enabled = false;
            }
            ApplyForceToEnemie(enemieElements, attackPos);
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
            Collider[] colliders = Physics.OverlapSphere(bullet.transform.position, bullet.radius);
            foreach (var collider in colliders)
            {
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

    public void ApplyForceToEnemie(EnemieElementsComponent enemieElementsComponent, Vector3 positionOfForce)
    {
        var hips = enemieElementsComponent.animator.GetBoneTransform(HumanBodyBones.Hips);
        float forcePower = UnityEngine.Random.Range(config.GetValue(EGameValue.minPowerOfBulletsExplosion),
            config.GetValue(EGameValue.maxPowerOfBulletsExplosion));
        Vector3 forceDirection = (hips.position - positionOfForce).normalized;
        var rb = hips.GetComponent<Rigidbody>();
        rb.AddForce(forcePower * forceDirection, ForceMode.VelocityChange);
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
