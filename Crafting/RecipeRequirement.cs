using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RecipeRequirement
{
    public IngredientType ingredientType;
    public int ingredientAmmount;

    public RecipeRequirement(IngredientType type, int ammount)
    {
        ingredientType = type;
        ingredientAmmount = ammount;
    }

    public RecipeRequirement Copy()
    {
        return new RecipeRequirement(ingredientType, ingredientAmmount);
    }
}
