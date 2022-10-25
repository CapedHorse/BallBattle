using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CapedHorse.BallBattle
{
    public class Soldier : MonoBehaviour
    {
        public enum State { Inactive, StandBy, Chasing, PassedBall, HoldingBall, StraightThrough }
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

        


        /// <summary>
        /// Using coroutine to delay 3 seconds waiting for spawned animation before setting to inactive
        /// </summary>
        public IEnumerator BaseStart(UnityAction afterInactive)
        {
            yield return new WaitForSeconds(3);
            OnChangingState(State.Inactive);
            yield return new WaitForSeconds(spawnTime);
            afterInactive?.Invoke();
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

