using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Kuhpik
{
    public enum EGameValue
    {
        minPowerOfBulletsExplosion,
        maxPowerOfBulletsExplosion,
        playerSpeed,
        playerRotatingSpeed,
        delayBetweenShoots,
        speedOfBullet

    }

    [Serializable]
    public class GameValueConfig
    {
        public EGameValue type;
        public float value;
        public Vector2 minmaxValue;
    }

    [CreateAssetMenu(menuName = "Kuhpik/GameConfig")]
    public sealed class GameConfig : ScriptableObject
    {
        [field: SerializeField] GameValueConfig[] gameValueConfigs;
        public Dictionary<EGameValue, GameValueConfig> gameValuesDict { get; private set; }
        public GameValueConfig[] GameValusConfigs => gameValueConfigs;

        public void Init(GameValueConfig[] gameValues)
        {
            gameValuesDict = gameValues.ToDictionary(x => x.type, x => x);
        }

        public float GetValue(EGameValue type)
        {
            return gameValuesDict[type].value;
        }
    }
}