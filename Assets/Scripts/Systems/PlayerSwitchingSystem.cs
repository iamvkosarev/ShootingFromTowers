using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Kuhpik;
using System;


public class PlayerSwitchingSystem : GameSystem, IIniting
{
    [SerializeField] private Animator cameraSwitchingAnimator;
    [SerializeField] private Button switchOffShootingButton;

    private TowerComponent currentTowerComponent;

    void IIniting.OnInit()
    {
        SwitchPlayerAction(false);
        switchOffShootingButton.onClick.AddListener(() =>
        {
            SwitchPlayerAction(false);
        });
    }

    public void ActivateTower(TowerComponent towerComponent)
    {
        currentTowerComponent = towerComponent;
        game.towerCamera.position = towerComponent.cameraPoint.position;
        game.towerCamera.rotation = towerComponent.cameraPoint.rotation;
        SwitchPlayerAction(true);
    } 

    private void SwitchPlayerAction(bool switchOnShooting)
    {
        player.canMove = !switchOnShooting;
        cameraSwitchingAnimator.SetBool("Shoot Mode", switchOnShooting);
        if (switchOnShooting)
        {
            game.playerElements.navMeshAgent.destination = currentTowerComponent.pointForShooting.position;
            StartCoroutine(WaitWhilePrepering(switchOnShooting));
        }
        else
        {
            player.canShoot = switchOnShooting;
            switchOffShootingButton.gameObject.SetActive(switchOnShooting);
        }
    }

    IEnumerator WaitWhilePrepering(bool switchOnShooting)
    {
        while (game.playerElements.rigidbody.transform.InverseTransformDirection(game.playerElements.navMeshAgent.velocity).z != 0)
        {
            yield return null;
        }

        player.canShoot = switchOnShooting;
        switchOffShootingButton.gameObject.SetActive(switchOnShooting);
    }

}
