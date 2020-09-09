using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    public Recipe recipe;
    public List<RecipeRequirement> ingredientsInventory = new List<RecipeRequirement>();
    public Transform objectSpawnPoint;

    private void Start()
    {
        //ingredientsInventory = new List<RecipeRequirement>(recipe.requiredInput);
        for (int i = 0; i < recipe.requiredInput.Count; i++)
        {
            ingredientsInventory.Add(recipe.requiredInput[i].Copy());
            ingredientsInventory[i].ingredientAmmount = 0;
        }
    }

    public bool AddIngredient(Ingredient ing)
    {
        if (recipe.ContainsIngredientType(ing.ingredientType))
        {
            for (int i = 0; i < ingredientsInventory.Count; i++)
            {
                if (ingredientsInventory[i].ingredientType == ing.ingredientType)
                {
                    ingredientsInventory[i].ingredientAmmount++;
                }
            }
            if (recipe.CheckComplete(ingredientsInventory))
            {
                ObjectPoolManager.instance.SpawnObject(recipe.output, objectSpawnPoint.position);
                for (int i = 0; i < recipe.requiredInput.Count; i++)
                {
                    ingredientsInventory[i].ingredientAmmount -= recipe.requiredInput[i].ingredientAmmount;
                }
            }
            return true;
        }
        return false;
    }
}
