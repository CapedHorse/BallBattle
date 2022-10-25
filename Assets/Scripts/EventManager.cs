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
        public static UnityAction OnNearestToBall;

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

