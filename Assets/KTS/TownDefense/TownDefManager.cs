using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace TownDefense
{
    public class TownDefManager : MonoBehaviour
    {
        public static TownDefManager Instance;
        private Dictionary<chelType,List<GameObject>> allCreatedUnitvisuals;
        //UI
        [SerializeField] private Transform ourAttack;
        [SerializeField] private Transform enemyAttack;
        [SerializeField] private Transform ourJar;
        [SerializeField] private Transform enemyJar;
        [SerializeField] private GameObject krestyanenPrefab;
        [SerializeField] private GameObject voinPrefab;
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private GameObject MenuUI;
        [SerializeField] private GameObject GameUI;
        [SerializeField] private GenVisual wheatGenVisual;
        [SerializeField] private GenVisual waveGenVisual;
        //Logic
        private int currentWave;
        
        public int wheat;
        public int GetUnitCount(chelType chelType)
        {
            return chelType switch
            {
                chelType.enemy => ENEMY_count,
                chelType.krestyanen => UNITS_krestyane,
                chelType.voin => UNITS_voins,
                _=>0
            };
        }
        public void AddWheat(int _how)
        {
            wheat += _how;
            UpdateWheatText();
        }
        private int wheatAdding = 1;
        [SerializeField] private int startingKrestyanens;
        [SerializeField] private float wheatGeneratingTime;
        [SerializeField] private float waveCooldownTime;
        private int UNITS_krestyane;
        private int UNITS_voins;
        private int ENEMY_count;
        
        private void UpdateWheatText() => wheatGenVisual.UpdateText($"{wheat}<sprite=0>");
        private void UpdateWaveText()
        {
            string _up = $"{currentWave}<sprite=2>";
            if (ENEMY_count != 0)
            {
                _up += $" - {ENEMY_count}<sprite=1>";
            }
            waveGenVisual.UpdateText(_up);
        }
        private Coroutine _cowavegen;
        private IEnumerator CoWaveGen()
        {
            float curT = 0;
            while (true)
            {
                curT += Time.deltaTime;
                waveGenVisual.UpdateSlider(curT/waveCooldownTime);
                if (curT > waveCooldownTime)
                {
                    StartCoroutine(CreateNewWave());
                    curT = 0;
                    UpdateWaveText();
                }
                yield return null;
            }
        }
        public IEnumerator CreateNewWave()
        {
            currentWave ++;
            ENEMY_count += (int)Mathf.Round(1 * Mathf.Pow(1.4f,currentWave));
            for(int _i=0;_i<ENEMY_count;_i++) SpawnPhysicsChel(chelType.enemy,false);
            yield return new WaitForSeconds(2);
            CalculateAttack();
        }
        private Coroutine _cowheatgen;
        private IEnumerator CoWheatGenerating()
        {
            float curT = 0;
            while (true)
            {
                curT += Time.deltaTime;
                wheatGenVisual.UpdateSlider(curT/wheatGeneratingTime);
                if (curT > wheatGeneratingTime)
                {
                    wheat += wheatAdding; 
                    curT = 0;
                    UpdateWheatText();
                }
                yield return null;
            }
        }
        public void Awake()
        {
            Instance = this;
            allCreatedUnitvisuals = new Dictionary<chelType, List<GameObject>>()
            {
                {chelType.krestyanen,new()},
                {chelType.voin,new()},
                {chelType.enemy,new()}
            };
            if (GameUI.activeSelf) StartGame();
        }
        public void StartGame()
        {
            _cowheatgen = StartCoroutine(CoWheatGenerating());
            _cowavegen = StartCoroutine(CoWaveGen());
            MenuUI.SetActive(false);
            GameUI.SetActive(true);
            UpdateWheatText();
            UpdateWaveText();
            UNITS_krestyane = startingKrestyanens;
            for(int _i=0;_i<startingKrestyanens;_i++) SpawnPhysicsChel(chelType.krestyanen,true);
        }
        public void CalculateAttack()
        {
            for(int _i = 0; _i < ENEMY_count; _i++)
            {
                if (UNITS_voins != 0)
                {
                    KillUnit(chelType.enemy);
                    if (_i % 3 == 0){
                        KillUnit(chelType.voin);
                    }
                }
                else if (UNITS_krestyane != 0)
                {
                    KillUnit(chelType.enemy);
                    KillUnit(chelType.krestyanen);
                }
                else
                {
                    EndGame();
                }
            }
        }
        public void SpawnPhysicsChel(chelType _type, bool ourTeam = true)
        {
            Transform Where = ourTeam ? ourJar : enemyJar;
            GameObject _prefab = _type switch
            {
                chelType.krestyanen => krestyanenPrefab,
                chelType.voin => voinPrefab,
                chelType.enemy => enemyPrefab,
                _=> krestyanenPrefab
            };
            GameObject _ourObject = GameObject.Instantiate(_prefab,Where);
            allCreatedUnitvisuals[_type].Add(_ourObject);
        }
        public void KillUnit(chelType _type)
        {
            GameObject.Destroy(allCreatedUnitvisuals[_type][0]);
            switch (_type)
            {
                case chelType.krestyanen:
                    UNITS_krestyane--;
                break;
                case chelType.voin:
                    UNITS_voins--;
                break;
                case chelType.enemy:
                    ENEMY_count--;
                break;
            }
        }
        public enum chelType
        {
            krestyanen,
            voin,
            enemy,
        }
        public void EndGame()
        {
            StopCoroutine(_cowavegen);
            StopCoroutine(_cowheatgen);
        }
        public void Update()
        {
            
        }
    }
}
