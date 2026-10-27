using System.Collections.Generic;
public class BurgerConstructor
{
    public bool TryGetBurger(string comp1, string comp2, string comp3, string comp4, out string burgerName){
        List<string> ingredients = new List<string>(){ comp1, comp2, comp3, comp4 };
        if (ingredients.Contains("сыр") &&
            ingredients.Contains("кетчуп") && 
            ingredients.Contains("котлета") &&
            ingredients.Contains("огурец"))
        {
            burgerName = "чизбургер";
            return true;
        }
        else if (ingredients.Contains("специальный соус") &&
            ingredients.Contains("булочка с кунжутом") && 
            ingredients.Contains("две котлеты") &&
            ingredients.Contains("котлета"))
        {
            burgerName = "биг мак";
            return true;
        }
        burgerName = "Мусор";
        return false;
    }
}
public class InternetShop
{
    
}