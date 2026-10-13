
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace TownDefense
{
    public class TownDefManager : MonoBehaviour
    {
        public static TownDefManager Instance;
        private Dictionary<unitType,List<GameObject>> allUnits;
        private Dictionary<unitType,int> allUnitCounts;
        //DATA
        [SerializeField] private AnimationCurve MoveCurve;
        [SerializeField] private Vector2 defaultJamPosition;
        [SerializeField] private Vector2 attackJamPosition;
        //UI
        [SerializeField] private AudioSource attackSnd;
        [SerializeField] private AudioSource boomAudioSnd;
        [SerializeField] private Animator boomAn;
        [SerializeField] private Transform ourJarSpawnPos;
        [SerializeField] private RectTransform ourJarRect;
        [SerializeField] private Transform enemyJarSpawnPos;
        [SerializeField] private RectTransform enemyJarRect;
        [SerializeField] private GameObject peasantPrefab;
        [SerializeField] private GameObject knightPrefab;
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private GameObject MenuUI;
        [SerializeField] private GameObject GameUI;
        [SerializeField] private GameObject GameOverUI;
        [SerializeField] private GameObject pauseUI;
        [SerializeField] private GenVisual wheatGenVisual;
        [SerializeField] private GenVisual wheatEatingGenVisual;
        [SerializeField] private GenVisual waveGenVisual;
        [SerializeField] private GenVisual eventGenVisual;
        [SerializeField] private TMP_Text unitCntLabel_peasants;
        [SerializeField] private TMP_Text unitCntLabel_knights;
        [SerializeField] private TMP_Text unitCntLabel_enemy;
        //Logic
        private int currentWave;
        private bool isAttack;
        
        public int wheat;
        public int GetUnitCount(unitType unitType)
        {
            if (!allUnitCounts.ContainsKey(unitType)) return 0;
            return allUnitCounts[unitType];
        }
        private void AddUnitCount(unitType _type, int _int){
            if (!allUnitCounts.ContainsKey(_type)) allUnitCounts.Add(_type,0);
            allUnitCounts[_type] += _int;
            if (GetUnitCount(unitType.knight) > 0)
            {
                wheatEatingGenVisual.UpdateText($"-{GetUnitCount(unitType.knight)}<sprite=0>");
            }
            else wheatEatingGenVisual.UpdateText($"0<sprite=0>");
            
            (_type switch
            {
                unitType.peasant => unitCntLabel_peasants,
                unitType.knight => unitCntLabel_knights,
                unitType.enemy => unitCntLabel_enemy,
                _=> unitCntLabel_peasants
            }).text = allUnitCounts[_type].ToString();
        }
        public void AddWheat(int _how)
        {
            wheat += _how;
            UpdateWheatText();
        }
        [SerializeField] private int startingKrestyanens;
        [SerializeField] private float wheatGeneratingTime;
        [SerializeField] private float knightsEatingCD;
        [SerializeField] private float waveCooldownTime;
        [SerializeField] private float eventCooldownTime;
        private int GetWheatIncome()
        {
            return (int)Mathf.Clamp(Mathf.Floor(GetUnitCount(unitType.peasant) /2f),1,int.MaxValue);
        }
        private void UpdateWheatText() => wheatGenVisual.UpdateText($"{wheat}<sprite=0>({GetWheatIncome()})");
        private void UpdateWaveText()
        {
            string _up = $"{currentWave}<sprite=2>";
            if (GetUnitCount(unitType.enemy) != 0)
            {
                _up += $" - {GetUnitCount(unitType.enemy)}<sprite=1>";
            }
            waveGenVisual.UpdateText(_up);
        }
        private Coroutine _cowavegen;
        private IEnumerator CoWaveGen()
        {
            float curT = 0;
            while (true)
            {
                if (isAttack) {
                    yield return null;
                    continue;
                }
                curT += Time.deltaTime;
                if (curT > waveCooldownTime)
                {
                    curT = 0;
                    StartCoroutine(CreateNewWave());
                    UpdateWaveText();
                    isAttack = true;
                }
                waveGenVisual.UpdateSlider(curT/waveCooldownTime);
                yield return null;
            }
        }
        public IEnumerator CreateNewWave()
        {
            attackSnd.Play();
            currentWave ++;
            SpawnUnit(unitType.enemy,false,(int)Mathf.Round(1 * Mathf.Pow(1.4f,currentWave) * (nextWaveBig ? 2 : 1)));
            if (nextWaveBig)
            {
                if (curEvent == RNGEvent.bignextwave)eventGenVisual.UpdateText("none");
                nextWaveBig = false;
            }
            yield return new WaitForSeconds(1);
            float _timer = 0;
            while(_timer < 1)
            {
                _timer += Time.deltaTime * 1;
                ourJarRect.anchoredPosition = Vector2.LerpUnclamped(defaultJamPosition,attackJamPosition,MoveCurve.Evaluate(_timer));
                enemyJarRect.anchoredPosition = new Vector2(-ourJarRect.anchoredPosition.x,ourJarRect.anchoredPosition.y);
                yield return null;
            }
            yield return new WaitForSeconds(1);
            boomAn.SetTrigger("Boom");
            boomAudioSnd.Play();
            CalculateAttack();
            if(gameOver) yield break;
            yield return new WaitForSeconds(1);
            _timer = 0;
            
            while(_timer < 1)
            {
                _timer += Time.deltaTime * 1;
                ourJarRect.anchoredPosition = Vector2.LerpUnclamped(attackJamPosition,defaultJamPosition,MoveCurve.Evaluate(_timer));
                enemyJarRect.anchoredPosition = new Vector2(-ourJarRect.anchoredPosition.x,ourJarRect.anchoredPosition.y);
                yield return null;
            }
            isAttack = false;
        }
        public void CalculateAttack()
        {
            int _cnt = GetUnitCount(unitType.enemy);
            for(int _i=0;_i<_cnt;_i++)
            {
                if (GetUnitCount(unitType.knight) > 0)
                {
                    if (_i%3==2)KillUnit(unitType.knight);
                    KillUnit(unitType.enemy);
                }
                else if (GetUnitCount(unitType.peasant) > 0)
                {
                    KillUnit(unitType.peasant);
                    KillUnit(unitType.enemy);
                }
                else
                {
                    EndGame();
                    break;
                }
            }
        }
        private void LogAllStats()
        {
            foreach(var _keyval in allUnitCounts)
            {
                Debug.Log($"{_keyval.Key} - {_keyval.Value}");
            }
        }
        private Coroutine _coeventgen;
        private IEnumerator CoEventGen()
        {
            float curT = 0;
            while (true)
            {
                curT += Time.deltaTime;
                if (curT > eventCooldownTime)
                {
                    RNGEvent _event = (RNGEvent)Random.Range(0,3);
                    curEvent = _event;
                    switch (_event)
                    {
                        case RNGEvent.goodWheat:
                            AddWheat(GetWheatIncome()*3);
                            eventGenVisual.UpdateText("More WHEAT!");
                        break;
                        case RNGEvent.bignextwave:
                            nextWaveBig = true;
                            eventGenVisual.UpdateText("Next wave is bigger");
                        break;
                        case RNGEvent.peasantsdead:
                            if (GetUnitCount(unitType.peasant) > 0)
                            {
                                KillUnit(unitType.peasant);
                            }
                            else
                            {
                                EndGame();
                            }
                            eventGenVisual.UpdateText("COVID");
                        break;                      
                    }
                    
                    curT = 0;
                }
                eventGenVisual.UpdateSlider(curT/eventCooldownTime);
                yield return null;
            }
        }
        private RNGEvent curEvent;
        private bool nextWaveBig;
        public enum RNGEvent
        {
            goodWheat,
            bignextwave,
            peasantsdead
        }
        private Coroutine _cowheateat;
        private IEnumerator CoWheatEating()
        {
            float curT = 0;
            while (true)
            {
                if (isAttack || GetUnitCount(unitType.knight) == 0) {
                    yield return null;
                    continue;
                }
                curT += Time.deltaTime;
                if (curT > knightsEatingCD)
                {
                    for(int _i = 0; _i < GetUnitCount(unitType.knight); _i++)
                    {
                        if (wheat > 0)
                        {
                            AddWheat(-1);
                        }
                        else
                        {
                            KillUnit(unitType.knight);
                        }
                    }
                    curT = 0;
                }
                wheatEatingGenVisual.UpdateSlider(curT/knightsEatingCD);
                yield return null;
            }
        }
        private Coroutine _cowheatgen;
        private IEnumerator CoWheatGenerating()
        {
            float curT = 0;
            while (true)
            {
                if (isAttack) {
                    yield return null;
                    continue;
                }
                curT += Time.deltaTime;
                if (curT > wheatGeneratingTime)
                {
                    AddWheat(GetWheatIncome());
                    curT = 0;
                }
                wheatGenVisual.UpdateSlider(curT/wheatGeneratingTime);
                yield return null;
            }
        }
        public void Awake()
        {
            Instance = this;
            allUnits = new();
            allUnitCounts = new();
            if (GameUI.activeSelf) StartGame();
        }
        public void StartGame()
        {
            gameOver = false;
            foreach(var _un in allUnits)
            {
                _un.Value.ForEach(obj=>GameObject.Destroy(obj));
            }
            isAttack = false;
            ourJarRect.anchoredPosition = defaultJamPosition;
            enemyJarRect.anchoredPosition = new Vector2(-ourJarRect.anchoredPosition.x,ourJarRect.anchoredPosition.y);
            currentWave = 0;
            allUnits = new();
            allUnitCounts = new();
            unitCntLabel_peasants.text = "0";
            unitCntLabel_knights.text = "0";
            unitCntLabel_enemy.text = "0";

            _coeventgen = StartCoroutine(CoEventGen());
            _cowheateat = StartCoroutine(CoWheatEating());
            _cowheatgen = StartCoroutine(CoWheatGenerating());
            _cowavegen = StartCoroutine(CoWaveGen());
            MenuUI.SetActive(false);
            GameUI.SetActive(true);
            UpdateWheatText();
            UpdateWaveText();
            eventGenVisual.UpdateText("none");
            wheatEatingGenVisual.UpdateText($"0<sprite=0>");
            SpawnUnit(unitType.peasant,true,startingKrestyanens);

        }
        public void SpawnUnit(unitType _type, bool ourTeam = true,int count = 1)
        {
            for(int _i=0;_i<count;_i++){
                Transform Where = ourTeam ? ourJarSpawnPos : enemyJarSpawnPos;
                GameObject _prefab = _type switch
                {
                    unitType.peasant => peasantPrefab,
                    unitType.knight => knightPrefab,
                    unitType.enemy => enemyPrefab,
                    _=> peasantPrefab
                };
                GameObject _ourObject = GameObject.Instantiate(_prefab,Where);
                if (!allUnits.ContainsKey(_type))allUnits.Add(_type,new());
                allUnits[_type].Add(_ourObject);
            }
            AddUnitCount(_type,count);
        }
        public void KillUnit(unitType _type,int count = 1)
        {
            for(int _i = 0; _i < count; _i++)
            {
                GameObject.Destroy(allUnits[_type][0]);
                allUnits[_type].RemoveAt(0);
            }
            AddUnitCount(_type,-count);
        }
        public enum unitType
        {
            peasant,
            knight,
            enemy,
        }
        private bool gameOver;
        public void EndGame()
        {
            gameOver = true;
            Debug.Log("КОНЕЦ ИГРЫ");
            StopCoroutine(_cowavegen);
            StopCoroutine(_cowheatgen);
            StopCoroutine(_cowheateat);
            StopCoroutine(_coeventgen);
            GameOverUI.SetActive(true);
        }
        public void GoToMenuFromGO()
        {
            GameOverUI.SetActive(false);
            GameUI.SetActive(false);
            MenuUI.SetActive(true);
        }
        public void RestartFromGO()
        {
            GameOverUI.SetActive(false);
            StartGame();
        }
        public void Resume()
        {
            ispaused = false;
            Time.timeScale = 1;
            
            pauseUI.SetActive(false);
        }
        public void MenuFromPause()
        {
            EndGame();
            GameUI.SetActive(false);
            MenuUI.SetActive(true);
            GameOverUI.SetActive(false);
            
            ispaused = false;
            Time.timeScale = 1;
            pauseUI.SetActive(false);
        }
        public void Update()
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame && !MenuUI.activeSelf)
            {
                ispaused = !ispaused;
                pauseUI.SetActive(ispaused);
                Time.timeScale = ispaused ? 0 : 1;
            }
        }
        public bool ispaused;
    }
}
/*
Экран паузы.

Звуки для сбора ресурсов, нападения, спавна юнитов, поедания пшеницы.

Референс: звуки в стиле Age of Empires II.

Лучшие практики

Проверять баланс ресурсов и юнитов, чтобы пройти хотя бы 10 циклов нападений.
Звуки и события должны не мешать производительности.
*/