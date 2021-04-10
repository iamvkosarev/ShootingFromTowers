using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kuhpik;

public class AnimationPlayerSystem : GameSystem, IUpdating
{
    void IUpdating.OnUpdate()
    {
        Vector3 localVelocity = game.playerElements.rigidbody.transform.InverseTransformDirection(game.playerElements.navMeshAgent.velocity);
        float speed = localVelocity.z;
        game.playerElements.animator.SetFloat("Run", speed / config.GetValue(EGameValue.playerSpeed));
    }
}
