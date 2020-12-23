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
        for (int i = 0; i < recipe.input.Count; i++)
        {
            ingredientsInventory.Add(recipe.input[i].Copy());
            ingredientsInventory[i].ingredientAmmount = 0;
        }
    }

    public bool AddIngredient(Item ing)
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

            CheckCompleteRecipe();

            return true;
        }
        return false;
    }

    void CheckCompleteRecipe()
    {
        if (recipe.CheckComplete(ingredientsInventory))
        {
            for (int i = 0; i < recipe.output.Count; i++)
            {
                ObjectPoolManager.instance.SpawnObject(recipe.output[i], objectSpawnPoint.position);
            }
            for (int i = 0; i < recipe.input.Count; i++)
            {
                ingredientsInventory[i].ingredientAmmount -= recipe.input[i].ingredientAmmount;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Item item))
        {
            if (AddIngredient(item) == false) { return; }
            else
            {
                item.gameObject.SetActive(false);
            }
        }
    }
}
