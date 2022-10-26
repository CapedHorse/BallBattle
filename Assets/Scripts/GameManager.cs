using System.Collections.Generic;
using UnityEngine;


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
        public Transform attackerTargetGate;
        public AttackerSoldier attackerPrefab;
        public DefenderSoldier defenderSoldier;
        public Ball ball;
        public List<SoldierCostume> costumes;


        [Header("Stats")]
        public List<Control> controllers;
        public bool isPlaying = false;
        public bool IsPlaying => isPlaying;
        public float currentPlayTime;

        public Control currentTurn;
        public Control currentAttacker;       

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
            EventManager.OnSpawning += SpawningSoldier;
        }

        void OnDisable()
        {
            EventManager.OnSpawning -= SpawningSoldier;
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
            SetControllerPosition();
            StartCoroutine(UIManager.instance.CountingDown(() =>
            {
                isPlaying = true;
                currentPlayTime += 1f;
                currentTurn = controllers[controllers.Count-1];
                currentAttacker = controllers[controllers.Count - 1];                
                SetControllerTurn();
                
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

            EventManager.SetTurn?.Invoke(currentTurn);
        }

        Soldier soldier;
        /// <summary>
        /// Checking if the energy is enough to spawn or not first.
        /// </summary>
        /// <param name="spawner"></param>
        /// <param name="spawnedPosition"></param>
        void SpawningSoldier(Control spawner, Vector3 spawnedPosition)
        {
            Debug.Log("Spawning");
            
            switch (spawner.position)
            {
                case Position.Attacker:
                    if (spawner.energy < attackerPrefab.spawnEnergyCost)
                    {
                        //notifies that energy is low
                        return;
                    }
                    soldier = Instantiate(attackerPrefab, attackersParent);
                    soldier.transform.LookAt(attackerTargetGate);
                    break;
                case Position.Defender:
                    if (spawner.energy < defenderSoldier.spawnEnergyCost)
                    {
                        //notifies that energy is low
                        return;
                    }
                    soldier = Instantiate(defenderSoldier, defendersParent);
                    soldier.transform.LookAt(defendersParent);  
                    break;
                default:
                    break;
            }

            spawner.CostingEnergy(soldier.spawnEnergyCost);
            soldier.transform.position = spawnedPosition;
            soldier.controller = spawner;
            spawnedSoldiers.Add(soldier);
            spawner.soldiers.Add(soldier);
            soldier = null;
            SetControllerTurn();
        }

        public void ChangingCostume(Soldier soldier, SoldierCostume costume)
        {

        }

        [System.Serializable]
        public class SoldierCostume
        {
            public string name;
            public Material headMat;
            public Material suitMat;
        }
    }
}

