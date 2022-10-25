using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapedHorse.BallBattle
{
    public class Ball : MonoBehaviour
    {
        public enum State { Hold, Idle }
        public State status;
        public Animator ballAnim;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Passed(Transform target)
        {

        }
    }
}

