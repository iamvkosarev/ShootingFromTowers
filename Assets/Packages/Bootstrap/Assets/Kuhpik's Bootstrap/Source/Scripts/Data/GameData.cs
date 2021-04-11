using UnityEngine;
using System.Collections.Generic;

namespace Kuhpik
{
    public class GameData
    {
        public PlayerElementsComponent playerElements;
        public Transform playerTarget;
        public Camera camera;
        public List<EnemieElementsComponent> enemies;
        public Animator cameraSwitchingAnimator;
    }
}