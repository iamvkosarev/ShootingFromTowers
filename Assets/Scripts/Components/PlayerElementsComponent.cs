using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerElementsComponent : MonoBehaviour
{
    [SerializeField] public Rigidbody rigidbody;
    [SerializeField] public NavMeshAgent navMeshAgent;
    [SerializeField] public Animator animator;
    [SerializeField] public Collider bodyCollider;
}
