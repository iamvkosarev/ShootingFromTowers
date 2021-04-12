using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Kuhpik;
using System;

public class LoadingSystem : GameSystem, IIniting
{
    [SerializeField] private Camera camera;
    [SerializeField] private Transform towerCamera;
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private Transform playerTarget;
    [SerializeField] private Transform playerShootingPoint;
    [SerializeField] private PlayerElementsComponent playerElements;
    [SerializeField] private GameObject enemiePrefab;
    [SerializeField] private Button restartButton;
    [SerializeField] private int enemiesNum = 1;
    void IIniting.OnInit()
    {
        config.Init(config.GameValusConfigs);
        CreatEnemies();
        SetupTowers();
        SetParameters();
        restartButton.onClick.AddListener(() => RestartGame());
        Bootstrap.ChangeGameState(EGamestate.Game);
    }

    private void SetupTowers()
    {
        game.towersComponents = FindObjectsOfType<TowerComponent>();
        var playerSwitchingSystem = Bootstrap.GetSystem<PlayerSwitchingSystem>();
        foreach (var item in game.towersComponents)
        {
            item.OnClimbOnTowerAction += playerSwitchingSystem.ActivateTower;
        }
    }

    public void RestartGame()
    {
        var playerSwitchingSystem = Bootstrap.GetSystem<PlayerSwitchingSystem>();
        foreach (var item in game.towersComponents)
        {
            item.OnClimbOnTowerAction -= playerSwitchingSystem.ActivateTower;
        }
        Bootstrap.GameRestart(0);
    }

    private void CreatEnemies()
    {
        var enemiesParent = new GameObject("Enemies");
        game.enemies = new List<EnemieElementsComponent>();
        for (int i = 0; i < enemiesNum; i++)
        {
            var enemieGO = Instantiate(enemiePrefab, enemiesParent.transform);
            var point = Bootstrap.GetSystem<EnemiesMovingSystem>().GetPointInMovingZone();
            enemieGO.transform.position = point;
            game.enemies.Add(enemieGO.GetComponent<EnemieElementsComponent>());
            SetHitBoxesForEnemie(game.enemies[i]);
            Bootstrap.GetSystem<CollisionSystem>().ChangeEnemieRagdollState(game.enemies[i], false);
            game.enemies[i].currentMovingPos = enemieGO.transform.position;
        }
    }

    private void SetParameters()
    {
        game.towerCamera = towerCamera;
        game.bullets = new List<BulletComponent>();
        game.playerShootingPoint = playerShootingPoint;
        game.playerElements = playerElements;
        game.playerTarget = playerTarget;
        game.camera = camera;
        player.canMove = true;
        player.canShoot = false;
    }

    private void SetHitBoxesForEnemie(EnemieElementsComponent enemie)
    {
        foreach (var rb in enemie.ragdollsRigidbodies)
        {
            HitBoxComponent hitBox = rb.gameObject.AddComponent<HitBoxComponent>();
            hitBox.enemieElements = enemie;
        }
    }
}
