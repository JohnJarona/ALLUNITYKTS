using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace TownDefense
{
    public class BuyUnitVisual : MonoBehaviour
    {
        [SerializeField] private int cost;
        [SerializeField] private Button buyButton;
        [SerializeField] private TMP_Text BuyText;
        [SerializeField] private TMP_Text CntText;
        [SerializeField] private TMP_Text CostText;
        [SerializeField] private Image sliderImg;
        [SerializeField] private float buyTime;
        public void UpdateCountText(string _text) => CntText.text = _text;
        public void UpdateCostText(string _text) => CostText.text = _text;
        public void Start()
        {
            buyButton.onClick.AddListener(Press);
            UpdateSlider(0);
            UpdateCostText(cost.ToString()+"<sprite=0>");
            UpdateCountText(TownDefManager.Instance.GetUnitCount(myUnit).ToString());
        }
        public void OnDestroy()
        {
            buyButton.onClick.RemoveListener(Press);
        }
        public void UpdateSlider(float progress)
        {
            sliderImg.fillAmount = progress;
        }
        [SerializeField] private TownDefManager.chelType myUnit;
        private void Press()
        {
            if (TownDefManager.Instance.wheat < cost) return;
            BuyText.color = Color.red;
            TownDefManager.Instance.AddWheat(-cost);
            buyButton.interactable = false;
            StartCoroutine(CoWait());
        }
        public IEnumerator CoWait()
        {
            float curt = buyTime;
            while(curt > 0)
            {
                curt += -Time.deltaTime;
                sliderImg.fillAmount = curt/buyTime;
                yield return null;
            }
            buyButton.interactable = true;
            TownDefManager.Instance.SpawnPhysicsChel(myUnit);
            BuyText.color = Color.white;
            UpdateCountText(TownDefManager.Instance.GetUnitCount(myUnit).ToString());
        }

    }
}