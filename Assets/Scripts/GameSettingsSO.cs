using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapedHorse.BallBattle
{
    [CreateAssetMenu(menuName = "Game Settings", fileName = "Game Settings")]
    public class GameSettingsSO : ScriptableObject
    {
        public int matchPerGame;
        [Tooltip("In Seconds")]public float timePerMatch;
        public int energyBar;
    }
}

