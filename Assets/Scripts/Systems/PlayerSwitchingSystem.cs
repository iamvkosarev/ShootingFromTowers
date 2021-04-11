using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Kuhpik;

public class PlayerSwitchingSystem : GameSystem, IIniting
{
    [SerializeField] private Animator cameraSwitchingAnimator;
    [SerializeField] private Button switchOffShootingButton;
    [SerializeField] private Transform pointForShooting;

    void IIniting.OnInit()
    {
        SwitchPlayerAction(false);
        switchOffShootingButton.onClick.AddListener(() =>
        {
            SwitchPlayerAction(false);
        });
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerElementsComponent>())
        {
            SwitchPlayerAction(true);
        }

    }

    private void SwitchPlayerAction(bool switchOnShooting)
    {
        player.canMove = !switchOnShooting;
        cameraSwitchingAnimator.SetBool("Shoot Mode", switchOnShooting);
        if (switchOnShooting)
        {
            game.playerElements.navMeshAgent.destination = pointForShooting.position;
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
