using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Kuhpik;

public class PlayerSwitchingSystem : GameSystem, IIniting
{
    [SerializeField] private Animator cameraSwitchingAnimator;
    [SerializeField] private Button switchOffShootingButton;

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
        player.canShoot = switchOnShooting;
        switchOffShootingButton.gameObject.SetActive(switchOnShooting);
        cameraSwitchingAnimator.SetBool("Shoot Mode", switchOnShooting);
    }

}
