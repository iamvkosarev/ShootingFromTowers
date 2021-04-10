using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kuhpik;

public class LoadingSystem : GameSystem, IIniting
{
    [SerializeField] private Camera camera;
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private Transform playerTarget;
    [SerializeField] private PlayerElementsComponent playerElements;
    void IIniting.OnInit()
    {
        config.Init(config.GameValusConfigs);
        game.playerElements = playerElements;
        game.playerTarget = playerTarget;
        game.camera = camera;

        Bootstrap.ChangeGameState(EGamestate.Game);
    }
}
