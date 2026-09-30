using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCell : MonoBehaviour
{
    public TMP_Text levelNumLabel;
    public List<Image> StarImgs;
    public Sprite starOpenSpr;
    public GameObject starsPObj;
    public GameObject labelObj;
    public GameObject lockObj;
    public void Setup(bool locked = false,int _levelnum = 0, int starnum = 0)
    {
        levelNumLabel.text = _levelnum.ToString();
        lockObj.SetActive(locked);
        starsPObj.SetActive(!locked);
        labelObj.SetActive(!locked);
        for(int _i = 0; _i < starnum; _i++)
        {
            StarImgs[_i].sprite = starOpenSpr;
        }
    }
}
