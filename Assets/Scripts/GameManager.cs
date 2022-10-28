using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;

namespace CapedHorse.BallBattle
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public enum Position { Attacker, Defender }

        public enum MatchResult { PlayerWin, EnemyWin, Draw }

        [Header("Managers And Settings")]
        public GameSettingsSO gameSetting;

        [Header("Object Refs")]
        public Transform field;
        public Camera mainCamera;
        public BoxCollider fieldCollider;
        public Transform attackersParent, defendersParent;
        public Transform attackerTargetGate;
        public AttackerSoldier attackerPrefab;
        public DefenderSoldier defenderSoldier;
        public Ball ball;
        public List<SoldierCostume> costumes;
        public Dictionary<string, SoldierCostume> costumeTable;

        [Header("Stats")]
        public List<Control> controllers;
        public bool isPlaying = false;
        public int totalMatch;
        public bool IsPlaying => isPlaying;
        public float currentPlayTime;

        public Control currentTurn;
        public Control currentAttacker;       

        public List<Soldier> spawnedSoldiers;

        public Camera MainCamera
        {
            get
            {
                if (MainManager.instance.grantedCamera)
                {
                    return ARSceneManager.instance.arCam;
                }
                else
                {
                    return mainCamera;
                }
            }
        }

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

            costumeTable = new Dictionary<string, SoldierCostume>();
            foreach (var item in costumes)
            {
                costumeTable.Add(item.name, item);
            }

            if (MainManager.instance.grantedCamera)
            {
                mainCamera.gameObject.SetActive(false);
            }
        }
        // Start is called before the first frame update
        IEnumerator Start()
        {
            yield return new WaitUntil(() => MainManager.instance.allowStartGame);
            StartCountDown();
        }

        void OnEnable()
        {
            EventManager.OnSpawning += SpawningSoldier;
            EventManager.SetPosition += SetControllerPosition;
            EventManager.OnGoal += Goal;
            EventManager.OnNoBallPasses += NoBallPassed;
            EventManager.OnTimesUp += TimesUp;
            EventManager.OnTriggerPenalty += TriggerPenalty;
            EventManager.OnMatchEnd += OnMatchResult;
    }

        void OnDisable()
        {
            EventManager.OnSpawning -= SpawningSoldier;
            EventManager.SetPosition -= SetControllerPosition;
            EventManager.OnGoal -= Goal;
            EventManager.OnNoBallPasses -= NoBallPassed;
            EventManager.OnTimesUp -= TimesUp;
            EventManager.OnTriggerPenalty -= TriggerPenalty;
            EventManager.OnMatchEnd -= OnMatchResult;
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
                else
                {
                    isPlaying = false;
                    EventManager.OnTimesUp?.Invoke();
                }
                
            }
        }



        /// <summary>
        /// Function to start counting down, will initiate all necessary initial set up before match here.
        /// will start a coroutine in the UI Manager the callback is allowing the game to start playing. Add 1 second so it doesn't immediately decrease 1 second of the time.
        /// </summary>
        void StartCountDown()
        {
            AudioManager.instance.PlayBGM("Play");
            InitTime();
            SetControllerPosition();
            StartCoroutine(UIManager.instance.CountingDown(() =>
            {
                isPlaying = true;
                currentPlayTime += 1f;
                SetControllerTurn();
            }));
        }

        /// <summary>
        /// To Adjust time per match and in the UI
        /// Will be invoked on first countdown and on switching sides
        /// </summary>
        void InitTime()
        {
            currentPlayTime = gameSetting.timePerMatch;
            UIManager.instance.UpdateTimerUI(currentPlayTime);
        }

        /// <summary>
        /// Switching controller's position
        /// </summary>
        void SetControllerPosition()
        {
            Debug.Log("Setting Controller position");
            ClearField();
            if (currentAttacker == null)
            {
                currentAttacker = controllers[0];
                controllers[0].position = Position.Attacker;
                controllers[1].position = Position.Defender;
                RotateField(true);
            }
            else
            {
                if (currentAttacker == controllers[0])
                {
                    currentAttacker = controllers[1];
                    controllers[0].position = Position.Defender;
                    controllers[1].position = Position.Attacker;
                    RotateField(false);
                }
                else
                {
                    currentAttacker = controllers[0];
                    controllers[0].position = Position.Attacker;
                    controllers[1].position = Position.Defender;
                    RotateField(true);
                }
            }

            Ball.instance.PlaceRandom();
        }
        
        /// <summary>
        /// Rotating y axis field, if asPlayer, means player is an attacker, rotate to default which is 0
        /// </summary>
        /// <param name="asPlayer"></param>
        void RotateField(bool asPlayer)
        {
            var rotation = 0;

            if (!asPlayer)
            {
                rotation = 180;
            }

            field.DORotate(new Vector3(0, rotation, 0), gameSetting.uiTweenDuration);
        }


        /// <summary>
        /// Setting controller's turn every spawning, not invoked while
        /// </summary>
        void SetControllerTurn()
        {
            if (currentTurn == null)
            {
                currentTurn = controllers[0];
            }
            else
            {
                if (currentTurn == controllers[0])
                {
                    currentTurn = controllers[1];
                }
                else
                {
                    currentTurn = controllers[0];
                }
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
                        NotifUI.instance.Notify(NotifUI.NotifType.EnergyLow);
                        return;
                    }
                    soldier = Instantiate(attackerPrefab, attackersParent);
                    soldier.transform.LookAt(attackerTargetGate);
                    break;
                case Position.Defender:
                    if (spawner.energy < defenderSoldier.spawnEnergyCost)
                    {
                        //notifies that energy is low
                        NotifUI.instance.Notify(NotifUI.NotifType.EnergyLow);
                        return;
                    }
                    soldier = Instantiate(defenderSoldier, defendersParent);
                    soldier.transform.LookAt(defendersParent);  
                    break;
                default:
                    break;
            }

            AudioManager.instance.PlaySFX("Ting");
            spawner.CostingEnergy(soldier.spawnEnergyCost);
            soldier.transform.position = spawnedPosition;
            soldier.transform.eulerAngles = spawner.position == Position.Attacker ? Vector3.zero : new Vector3(0, 180, 0);
            soldier.controller = spawner;
            ChangingCostume(soldier);
            spawnedSoldiers.Add(soldier);
            spawner.soldiers.Add(soldier);
            soldier = null;
            SetControllerTurn();
        }

        /// <summary>
        /// Changing costume for spawned soldier based on their controller's color
        /// </summary>
        /// <param name="soldier"></param>
        public void ChangingCostume(Soldier soldier)
        {
            soldier.mesh.materials[0] = costumeTable[soldier.controller.controlType.ToString()].headMat;
            soldier.mesh.materials[1] = costumeTable[soldier.controller.controlType.ToString()].suitMat;
        }

        /// <summary>
        /// Setting a soldier costume to grayscale.
        /// </summary>
        /// <param name="soldier"></param>
        /// <param name="inactiveTime"></param>
        public void SetInactiveSuit(Soldier soldier, float inactiveTime) {
            soldier.mesh.materials[0] = costumes.Find(x => x.name == "Inactive").headMat;
            soldier.mesh.materials[1] = costumes.Find(x => x.name == "Inactive").suitMat;
            DOVirtual.DelayedCall(inactiveTime, () => ChangingCostume(soldier));
        }

        /// <summary>
        /// Should have delay before switching position
        /// </summary>
        public void Goal()
        {

            if (currentAttacker == controllers[0])            
                EventManager.OnMatchEnd(MatchResult.PlayerWin);
            
            else
                EventManager.OnMatchEnd(MatchResult.EnemyWin);

        }

        public void NoBallPassed()
        {
            if (currentAttacker == controllers[0])
                EventManager.OnMatchEnd(MatchResult.EnemyWin);
            else
                EventManager.OnMatchEnd(MatchResult.PlayerWin);
        }

        public void TimesUp()
        {
            EventManager.OnMatchEnd(MatchResult.Draw);
        }

        public void TriggerPenalty()
        {
            StartCoroutine(GameEnd());
        }

        /// <summary>
        /// Mathc result, show the result if the match is not exceeding maximum number yet, 
        /// if do, check which score is higher, 
        /// if draw, then trigger penalty mode, 
        /// if not, show winner pop up, and 
        /// </summary>
        /// <param name="result"></param>
        public void OnMatchResult(MatchResult result)
        {
            isPlaying = false;

            switch (result)
            {
                case MatchResult.PlayerWin:
                    controllers[0].GainScore();
                    NotifUI.instance.Notify(NotifUI.NotifType.PlayerScore);
                    break;
                case MatchResult.EnemyWin:
                    controllers[1].GainScore();
                    NotifUI.instance.Notify(NotifUI.NotifType.EnemyScore);
                    break;
                case MatchResult.Draw:
                    NotifUI.instance.Notify(NotifUI.NotifType.Draw);
                    break;
                default:
                    break;
            }
            totalMatch++;
            if (totalMatch <= gameSetting.matchPerGame)
            {
                StartCoroutine(StartSwitchingSide());
            }
            else
            {
                if (controllers[0].score > controllers[1].score)
                {
                    NotifUI.instance.Notify(NotifUI.NotifType.PlayerWon);
                    StartCoroutine(GameEnd());
                }
                else if (controllers[1].score > controllers[0].score)
                {
                    NotifUI.instance.Notify(NotifUI.NotifType.EnemyWon);
                    StartCoroutine(GameEnd());
                }
                else 
                {
                    NotifUI.instance.Notify(NotifUI.NotifType.Draw);
                    EventManager.OnTriggerPenalty?.Invoke();
                }
                
            }
            
        }

        IEnumerator StartSwitchingSide()
        {
            yield return new WaitForSeconds(gameSetting.switchingSidesDelay);
            EventManager.SetPosition?.Invoke();
            yield return new WaitForSeconds(gameSetting.uiTweenDuration);
            InitTime();
            isPlaying = true;
        }

        IEnumerator GameEnd()
        {
            yield return new WaitForSeconds(gameSetting.backToMenuDelay);
            MainManager.instance.BackToHomeScene();
        }

        void ClearField()
        {
            foreach (var item in spawnedSoldiers)
            {
                Destroy(item);
            }

            spawnedSoldiers.Clear();
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

