using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace TownDefense
{
    public class GenVisual : MonoBehaviour
    {
        [SerializeField] private TMP_Text resCntText;
        [SerializeField] private TMP_Text percText;
        [SerializeField] private Image sliderImg;
        public void UpdateText(string _text) => resCntText.text = _text;
        public void UpdateSlider(float progress)
        {
            sliderImg.fillAmount = progress;
            percText.text = (progress*100).ToString("F2") + "%";
        }
    }
}