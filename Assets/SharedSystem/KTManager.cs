using System;
using UnityEngine;
using UnityEngine.UI;

public class KTManager : MonoBehaviour
{
    private float HUE;
    [SerializeField]private Image BG;
    void Update()
    {
        HUE = (HUE+Time.deltaTime / 25f) % 1;
        BG.color = Color.HSVToRGB(HUE,1,1);
    }
}
