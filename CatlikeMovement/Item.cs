using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Item : MonoBehaviour
{
    public IngredientType ingredientType = IngredientType.DEFAULT;

    Collider colliderComponent;
    Rigidbody rbComponent;
    Outline outline;
    public ParentConstraint parentConstraint;

    private void Awake()
    {
        colliderComponent = GetComponent<Collider>();
        rbComponent = GetComponent<Rigidbody>();
        outline = GetComponent<Outline>();
        OutlineOff();
    }

    public void HandlePickUp()
    {
        colliderComponent.enabled = false;
        rbComponent.isKinematic = true;
        parentConstraint.constraintActive = true;
        //OutlineOff();
    }

    public void HandlePlace()
    {
        colliderComponent.enabled = true;
        rbComponent.isKinematic = false;
        for (int i = 0; i < parentConstraint.sourceCount; i++)
        {
            parentConstraint.RemoveSource(0);
        }
        parentConstraint.constraintActive = false;
    }

    public void OutlineOn()
    {
        outline.enabled = true;
    }
    public void OutlineOff()
    {
        outline.enabled = false;
    }
}
