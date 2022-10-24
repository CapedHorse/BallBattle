using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapedHorse.BallBattle
{
    public class Soldier : MonoBehaviour
    {
        public enum State { Inactive, StandBy, Chasing, HoldingBall, StraightThrough }
        public State status = State.Inactive;
        //object cache
        [Header("Object References")]
        public Transform modelParent;
        public Animator anim;
        //vfx
        public GameObject puffShow;
        public GameObject exploded;
        public GameObject running;
        public GameObject hit;
        //variables
        [Header("Variables")]
        public float energyRegenTime;
        public float spawnEnergyCost;
        public float spawnTime;
        public float reactivateTime;
        public float normalSpeed;


        // Start is called before the first frame update
        public void BaseStart()
        {
            //OnChangingState(State.Spawned);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public virtual void OnChangingState(State state)
        {
            status = state;
        }
    }
}

