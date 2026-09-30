
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public TMP_Text PageLabel;
    public LevelCell levelCellPrefab;
    private Dictionary<int,List<LevelCell>> AllCreatedCells = new();
    public int LevelsCount;
    public int LevelsLockAfter;
    public int LevelsInOnePage;
    private int currentPage;
    private int pagesCnt;
    public RectTransform whereToSpawnCells;
    public void Start()
    {
        CreateAllLevels();
    }
    public void CreateAllLevels()
    {
        AllCreatedCells.Clear();
        int _page = 0;
        int _fpg = 0;
        AllCreatedCells.Add(0,new());
        for(int _i = 0; _i < LevelsCount; _i++)
        {
            LevelCell _creLvl = GameObject.Instantiate(levelCellPrefab,whereToSpawnCells);
            _creLvl.Setup(_i >= LevelsLockAfter,_i+1,_i==LevelsLockAfter-1 ? 0 : Random.Range(1,4));
            AllCreatedCells[_page].Add(_creLvl); 
            _fpg++;
            if (_fpg >= LevelsInOnePage)
            {
                _page++;
                AllCreatedCells.Add(_page,new());
                _fpg = 0;
                ActivatePage(_page,false);
            }
        }
        ActivatePage(_page,false);
        
        ActivatePage(0,true);
        pagesCnt = _page+1;
        UpdatePageText();
    }
    public void ActivatePage(int _num,bool _what)
    {
        AllCreatedCells[_num].ForEach(_element=>_element.gameObject.SetActive(_what));
    }
    public void PageLeft()
    {
        ActivatePage(currentPage,false);
        currentPage = (int)Mathf.Repeat(currentPage-1,pagesCnt);
        ActivatePage(currentPage,true);
        UpdatePageText();
    } 
    public void PageRight()
    {
        ActivatePage(currentPage,false);
        currentPage = (int)Mathf.Repeat(currentPage+1,pagesCnt);
        ActivatePage(currentPage,true);
        UpdatePageText();
    } 
    public void UpdatePageText()
    {
        PageLabel.text = $"{currentPage+1}/{pagesCnt}";
    }
}
