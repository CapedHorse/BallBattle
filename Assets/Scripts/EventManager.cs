using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CapedHorse.BallBattle
{
    public class EventManager : MonoBehaviour
    {
        public static EventManager instance;

        public static UnityAction<Control> SetTurn; //Invoked whena player is done spawning a soldier
        public static UnityAction SetPosition; //Invoked when it's time to set turn (times up or attacker win)
        public static UnityAction<Control, Vector3> OnSpawning; //Invoked when a player is spawning
        public static UnityAction<Control, float> OnCostingEnergy; //Same, costing energy when spawning
        public static UnityAction<AttackerSoldier> OnNearestToBall; //Invoked on spawning attacker

        public static UnityAction<Control> OnNearestToPass; //Should add this??
        public static UnityAction OnBallLoose; //Should add this??

        public static UnityAction<AttackerSoldier> OnBallHolded; //Invoked when an attacker caught a ball        
        public static UnityAction<DefenderSoldier, AttackerSoldier> OnAttackerCaught; //Invoked when an attacker is getting caught
        public static UnityAction OnGoal; // Invoked when a ball entered the defender's gate
        public static UnityAction OnNoBallPasses; //Invoked when no more attacker passed
        public static UnityAction OnTimesUp; //Invoked when times run out with ball reached defender's gate
        public static UnityAction OnTriggerPenalty; //invoked when the score is equal or draw


        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }
    }
}

