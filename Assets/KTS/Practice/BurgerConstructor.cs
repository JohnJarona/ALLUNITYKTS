using System.Collections.Generic;
using TMPro;
using UnityEngine;
public sealed class BurgerConstructor : MonoBehaviour
{
    [SerializeField] private TMP_InputField compIF;
    [SerializeField] private TMP_Text burgerNameCheckText;
    [SerializeField] private TMP_Text[] compsTextSlots = new TMP_Text[4];
    private readonly List<string> comps = new();
    private void Start()
    {
        UpdateCompsText();
    }
    public void AddComp()
    {
        if (comps.Count > 4)
        {
            comps.RemoveAt(comps.Count-1);
        }
        comps.Add(compIF.text);
        compIF.SetTextWithoutNotify("");
        UpdateCompsText();
    }
    public void CreateBurger()
    {
        if(comps.Count < 4)
        {
            for(int i = 0; i < 4-comps.Count; i++) comps.Add("...");
        }
        TryGetBurger(comps[0], comps[1], comps[2], comps[3], out string bname);
        comps.Clear();
        UpdateCompsText();
        burgerNameCheckText.text = bname;
    }
    private void UpdateCompsText()
    {
        for(int i = 0; i < 4; i++){
            if (comps.Count > i) compsTextSlots[i].text = comps[i];
            else
            {
                compsTextSlots[i].text = "...";
            }
        }
    }
    private bool TryGetBurger(string comp1, string comp2, string comp3, string comp4, out string burgerName){

        List<string> food = new List<string>
        {
            comp1.ToLower(),
            comp2.ToLower(),
            comp3.ToLower(),
            comp4.ToLower()
        };
        
        HashSet<string> cheeseburger = new HashSet<string> { "сыр", "кетчуп", "котлета", "огурец" };
        HashSet<string> bigmac = new HashSet<string> { "специальный соус", "булочка с кунжутом", "две котлеты" };
        
        bool iscb = true;
        foreach(string comp in cheeseburger)
        {
            if (!food.Contains(comp))
            {
                iscb = false;
                break;
            }
        }
        if (iscb)
        {
            burgerName = "чизбургер";
            return true;
        }
        bool isbm = true;
        foreach(string comp in bigmac)
        {
            if (!food.Contains(comp))
            {
                isbm = false;
                break;
            }
        }
        if (isbm)
        {
            burgerName = "биг мак";
            return true;
        }
        burgerName = "мусор";
        return false;
    }
}