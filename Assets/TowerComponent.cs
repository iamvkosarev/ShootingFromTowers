using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class TowerComponent : MonoBehaviour
{
    [SerializeField] public Transform cameraPoint;
    [SerializeField] public Transform pointForShooting;
    public PlayerSwitchingSystem playerSwitchingSystem { set; get; }
    public event Action<TowerComponent> OnClimbOnTowerAction;
    private bool wasInTower = false;

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerElementsComponent>();
        if(player != null && !wasInTower)
        {
            OnClimbOnTowerAction?.Invoke(this);
            wasInTower = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var player = other.GetComponent<PlayerElementsComponent>();
        if (player != null)
        {
            wasInTower = false;
        }
    }
}
