using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CapedHorse.BallBattle
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public enum Position { Attacker, Defender }

        [Header("Managers And Settings")]
        public GameSettingsSO gameSetting;

        [Header("Object Refs")]
        public Transform field;
        public Transform attackersParent, defendersParent;
        public AttackerSoldier attackerPrefab;
        public DefenderSoldier defenderSoldier;


        [Header("Stats")]
        public List<Control> controllers;
        public bool isPlaying = false;
        public bool IsPlaying => isPlaying;
        public float currentPlayTime;

        public Control currentTurn;
        public Control currentAttacker;
        
        public static UnityAction<Control> SetTurn;
        public static UnityAction<Control, Position> SetPosition;
        public static UnityAction<Control, Vector3> OnSpawning;

        public List<Soldier> spawnedSoldiers;

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
        // Start is called before the first frame update
        void Start()
        {
            StartCountDown();
        }

        void OnEnable()
        {
            OnSpawning += SpawningSoldier;
        }

        void OnDisable()
        {
            OnSpawning -= SpawningSoldier;
        }

        // Update is called once per frame
        void Update()
        {
            if (isPlaying)
            {
                if(currentPlayTime > 0)
                {
                    currentPlayTime -= Time.deltaTime;

                    UIManager.instance.UpdateTimerUI(currentPlayTime);
                }
                
            }
        }



        /// <summary>
        /// Function to start counting down, will initiate all necessary initial set up before match here.
        /// will start a coroutine in the UI Manager the callback is allowing the game to start playing. Add 1 second so it doesn't immediately decrease 1 second of the time.
        /// </summary>
        void StartCountDown()
        {
            currentPlayTime = gameSetting.timePerMatch;
            UIManager.instance.UpdateTimerUI(currentPlayTime);
            StartCoroutine(UIManager.instance.CountingDown(() =>
            {
                isPlaying = true;
                currentPlayTime += 1f;
                currentTurn = controllers[controllers.Count-1];
                currentAttacker = controllers[controllers.Count - 1];
                SetControllerTurn();
                SetControllerPosition();
            }));
        }

        void SetControllerPosition()
        {
            if (currentAttacker == controllers[0])
            {
                currentAttacker = controllers[1];
                controllers[0].position = Position.Defender;
                controllers[1].position = Position.Attacker;
            }
            else
            {
                currentAttacker = controllers[0];
                controllers[0].position = Position.Attacker;
                controllers[1].position = Position.Defender;
            }
        }

        void SetControllerTurn()
        {
            if (currentTurn == controllers[0])
            {
                currentTurn = controllers[1];
            }
            else
            {
                currentTurn = controllers[0];
            }

            SetTurn?.Invoke(currentTurn);
        }

        void SpawningSoldier(Control spawner, Vector3 spawnedPosition)
        {
            Debug.Log("Spawning");
            var soldier = new Soldier();
            switch (spawner.position)
            {
                case Position.Attacker:
                    soldier = Instantiate(attackerPrefab, attackersParent);
                    break;
                case Position.Defender:
                    soldier = Instantiate(defenderSoldier, defendersParent);
                    break;
                default:
                    break;
            }
            soldier.transform.position = spawnedPosition;
            spawnedSoldiers.Add(soldier);

            SetControllerTurn();
        }
    }
}

