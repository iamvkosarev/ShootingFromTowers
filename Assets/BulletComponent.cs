using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletComponent : MonoBehaviour
{
    [SerializeField] public ParticleSystem movingEffect;
    [SerializeField] public float radius = 0.3f;
    public bool canCheck = false;

    public Vector3 movingVelocity { set; get; }
}
