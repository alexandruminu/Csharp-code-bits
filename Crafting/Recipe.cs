using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "ScriptableObjects/Recipe")]
public class Recipe : ScriptableObject
{
    public List<RecipeRequirement> input = new List<RecipeRequirement>();
    public List<GameObject> output = new List<GameObject>();

    public bool ContainsIngredientType(IngredientType ingredientType)
    {
        for (int i = 0; i < input.Count; i++)
        {
            if(input[i].ingredientType == ingredientType)
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckComplete(List<RecipeRequirement> inventory)
    {
        for (int i = 0; i < input.Count; i++)
        {
            if(input[i].ingredientAmmount != inventory[i].ingredientAmmount)
            {
                return false;
            }
        }

        return true;
    }
}
