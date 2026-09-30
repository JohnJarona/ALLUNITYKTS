using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    [SerializeField] private float _time = 1;
    [SerializeField] private  Image clockImg;
    [SerializeField] private  AnimationCurve fadeOutCurve;
    private Coroutine _curCouratine;
    [SerializeField] private  TMP_Text startedCountText;
    [SerializeField] private  Button startTimerBtn;
    private int _countTimer;
    void UpdateStartedCountLabel(int _za)
    {
        startedCountText.text = $"Запуски: {_za}";
    }
    void Start()
    {
        UpdateStartedCountLabel(0);
    }
    void OnEnable()
    {
        startTimerBtn.onClick.AddListener(StartTimer);
    }
    void OnDisable()
    {
        startTimerBtn.onClick.RemoveListener(StartTimer);
    }
    void StartTimer()
    {
        startTimerBtn.interactable = false;
        if (_curCouratine != null)StopCoroutine(_curCouratine);
        _curCouratine = StartCoroutine(CoTimer());
    }
    IEnumerator CoTimer()
    {
        _countTimer++;
        UpdateStartedCountLabel(_countTimer);
        clockImg.fillAmount = 1;
        float _curt = _time;
        while(_curt > 0)
        {
            _curt += -Time.deltaTime;
            clockImg.fillAmount = _curt / _time;
            yield return null;
        }
        startTimerBtn.interactable = true;
        _curt = 0;
        while(_curt < 1)
        {
            _curt += Time.deltaTime * 5;
            clockImg.fillAmount = fadeOutCurve.Evaluate(_curt);
            yield return null;
        }
    }
}
