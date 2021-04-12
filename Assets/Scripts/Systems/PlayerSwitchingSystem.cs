using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Kuhpik;
using DG.Tweening;
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
            game.playerElements.animator.SetFloat("ActionTransition", 0f);
            player.canShoot = switchOnShooting;
            switchOffShootingButton.gameObject.SetActive(switchOnShooting);
        }
    }

    IEnumerator WaitWhilePrepering(bool switchOnShooting)
    {
        while (Vector3.Distance(currentTowerComponent.pointForShooting.position, game.playerElements.rigidbody.transform.position) >= 1f)
        {
            yield return null;
        }
        var t = 0f;
        DOTween.To(() => t, x => t = x, 1f, 0.4f).OnUpdate(() =>
        {
            game.playerElements.animator.SetFloat("ActionTransition", t);

        }
        ).OnComplete(() =>
        {
            switchOffShootingButton.gameObject.SetActive(switchOnShooting);
            player.canShoot = switchOnShooting;
        }).OnStart(() =>
        {
            game.playerElements.rigidbody.transform.DORotate(currentTowerComponent.pointForShooting.eulerAngles, 0.4f);
        });
    }

}
