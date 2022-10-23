using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapedHorse.BallBattle
{
    public class GameManager : MonoBehaviour
    {
        //Properties of the player

        [System.Serializable]
        public class PlayerProperties
        {
            public enum PlayerType { Attacker, Defender }
            public PlayerType playerType;
            public bool IsAttacker { get { return playerType == PlayerType.Attacker; } }

            public float energyRegenTime = .5f;
            public float energyCost = 2;
            public float spawnTime = .5f;
            public float reactivateTime = 2.5f;
            public float normalSpeed = 1.5f;
            
            [ShowIf(nameof(IsAttacker))] public float carryingSpeed = .75f;
            [ShowIf(nameof(IsAttacker))] public float passBallSpeed = 1.5f;
            [HideIf(nameof(IsAttacker))] public float returnSpeed = 2;
            [HideIf(nameof(IsAttacker))] public float detectionRangeFromStadium = 0.35f;
        }

        public List<PlayerProperties> playerProperties;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

