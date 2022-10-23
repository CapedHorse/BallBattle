using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapedHorse.BallBattle
{
    public class Soldier : MonoBehaviour
    {
        //object cache
        [Header("Object References")]
        public Transform modelParent;
        //variables
        [Header("Variables")]
        public float energyRegenTime;
        public float spawnEnergyCost;
        public float spawnTime;
        public float reactivateTime;
        public float normalSpeed;


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

