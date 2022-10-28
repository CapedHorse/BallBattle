using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapedHorse.BallBattle
{
    /// <summary>
    /// Contains all game setting properties
    /// </summary>
    [CreateAssetMenu(menuName = "Game Settings", fileName = "Game Settings")]
    public class GameSettingsSO : ScriptableObject
    {
        public int matchPerGame;
        [Tooltip("In Seconds")]public float timePerMatch;
        public int energyBar;
        public float uiTweenDuration = 0.25f;
        public float switchingSidesDelay = 2f;
        public float backToMenuDelay = 3f;
    }
}

