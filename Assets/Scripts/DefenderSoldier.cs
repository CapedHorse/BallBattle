using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapedHorse.BallBattle
{
    public class DefenderSoldier : Soldier, IOnTriggerNotifiable
    {
        //variables
        public float returnSpeed;
        public float detectionRangeFromWidth;

        [Header("Defender Object Refs")]
        public GameObject detectionRadius;
        public Collider detectionRadiusCollider;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void onChild_OnTriggerEnter(Collider myEnteredTrigger, Collider other)
        {
            Debug.Log("Entered detection radius");
        }

        public void onChild_OnTriggerExit(Collider myEnteredTrigger, Collider other)
        {
            Debug.Log("Exited detection radius");
        }

        public void onChild_OnTriggerStay(Collider myEnteredTrigger, Collider other)
        {
            //Debug.Log("Stayed detection radius");
        }
    }
}

