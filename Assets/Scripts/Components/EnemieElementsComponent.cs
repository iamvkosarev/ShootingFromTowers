using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemieElementsComponent : MonoBehaviour
{
    [SerializeField] public NavMeshAgent navMeshAgent;
    [SerializeField] public Vector3 currentMovingPos;
    [SerializeField] public Animator animator;
    [SerializeField] public Rigidbody[] ragdollsRigidbodies;
    public bool isFallen;
}
