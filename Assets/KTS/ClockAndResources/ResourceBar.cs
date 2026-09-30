using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceBar : MonoBehaviour
{
    [SerializeField] private Image _slider;
    private float progress = 0;
    private int resources;
    [SerializeField] private TMP_Text resourcelable;
    void Start()
    {
        UpdateResourceText(0);
    }
    void Update()
    {
        progress += Time.deltaTime;
        if (progress > 1)
        {
            progress = 0;
            resources++;
            UpdateResourceText(resources);
        }
        _slider.fillAmount = progress;
    }
    void UpdateResourceText(int _res)
    {
        resourcelable.text = $"Ресов: {_res}";
    }
}
