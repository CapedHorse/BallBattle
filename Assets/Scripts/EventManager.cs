using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CapedHorse.BallBattle
{
    public class EventManager : MonoBehaviour
    {
        public static EventManager instance;

        public static UnityAction<Control> SetTurn;
        public static UnityAction<Control, GameManager.Position> SetPosition;
        public static UnityAction<Control, Vector3> OnSpawning;
        public static UnityAction<Control, float> OnCostingEnergy;
        public static UnityAction<AttackerSoldier> OnNearestToBall;
        public static UnityAction<Control> OnNearestToToPass;
        public static UnityAction<AttackerSoldier> OnBallHolded;
        public static UnityAction OnBallLoose;
        public static UnityAction<DefenderSoldier, AttackerSoldier> OnAttackerCaught;
        public static UnityAction<Control> OnGoal;
        public static UnityAction<Control> OnNoBallPasses;
        public static UnityAction OnTimesUp;
        public static UnityAction OnTriggerPenalty;


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

