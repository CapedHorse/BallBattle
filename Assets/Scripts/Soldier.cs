using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CapedHorse.BallBattle
{
    public class Soldier : MonoBehaviour
    {
        public enum State { Inactive, StandBy, Chasing, ReturnBack, PassedBall, HoldingBall, StraightThrough, ChasingBall }
        public State status = State.Inactive;
        //object cache
        [Header("Object References")]
        public Control controller;
        public Transform modelParent;
        public Transform indicator;
        public Collider collider;
        public Animator anim;
        public Rigidbody rb;
        public SkinnedMeshRenderer mesh;
        
        //vfx
        public GameObject puffShow;
        public GameObject exploded;
        public GameObject running;
        public GameObject hit;
        //variables
        [Header("Variables")]
        public float spawnEnergyCost;
        public float spawnTime;
        public float reactivateTime;
        public float normalSpeed;

        public Transform target;

        public bool justSpawned = true;


        /// <summary>
        /// Using coroutine to delay 3 seconds waiting for spawned animation before setting to inactive
        /// </summary>
        public IEnumerator BaseStart(UnityAction afterInactive)
        {
            puffShow.SetActive(true);
            OnChangingState(State.Inactive);
            GameManager.instance.SetInactiveSuit(this, spawnTime);
            yield return new WaitForSeconds(spawnTime);
            justSpawned = false;
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

        public void MoveTo(Vector3 target, float speed)
        {
            target.y = 0;
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            
            RotateDirection(target);
        }

        public void RotateDirection(Vector3 target)
        {
            transform.LookAt(new Vector3(0, target.y, 0)); //sometimes the rotation is not right
            //indicator.LookAt(new Vector3(0, target.y, 0));
        }

        public void SetAnimation(string state)
        {
            ResetAnimation();
            anim.SetBool(state, true);
        }

        public void ResetAnimation()
        {
            foreach (var item in System.Enum.GetValues(typeof(State)))
            {
                //Debug.Log(item.ToString());
                if (!item.Equals(State.Inactive))
                {
                    anim.SetBool(item.ToString(), false);
                }

            }
        }

        
    }
}

