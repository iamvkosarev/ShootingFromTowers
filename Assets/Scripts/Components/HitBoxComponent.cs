using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Supyrb;


public class OnHitEnemieSignal: Signal<EnemieElementsComponent, Vector3> { }

public class HitBoxComponent : MonoBehaviour
{
    public EnemieElementsComponent enemieElements { set; get; }

    public void ActivateAttack(Vector3 attackPos)
    {
         Signals.Get<OnHitEnemieSignal>().Dispatch(enemieElements, attackPos);
    }
}
