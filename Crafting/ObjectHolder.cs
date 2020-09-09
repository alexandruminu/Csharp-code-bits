using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHolder : MonoBehaviour
{
    List<Ingredient> ingredientsInRange = new List<Ingredient>();
    Ingredient grabbedIngredient;
    Ingredient GrabbedIngredient
    {
        get
        {
            return grabbedIngredient;
        }
        set
        {
            grabbedIngredient = value;
            if(value != null)
            {
                grabbedRb = value.GetComponent<Rigidbody>();
                grbbedObjColider = value.GetComponent<Collider>();
            }
            else
            {
                grabbedRb = null;
                grbbedObjColider = null;
            }
        }
    }
    Rigidbody grabbedRb;
    Collider grbbedObjColider;

    Factory factoryInRange;

    private void Update()
    {
        if (Input.GetButtonDown("Interact Controller"))
        {
            if (grabbedRb)
            {
                if (factoryInRange)
                {
                    if (factoryInRange.AddIngredient(grabbedRb.GetComponent<Ingredient>()))
                    {
                        GrabbedIngredient.transform.gameObject.SetActive(false);
                        GrabbedIngredient.transform.SetParent(null);
                        GrabbedIngredient = null;
                    }
                }
                else
                {
                    //rigidbodiesInRange.Add(grabbedRb);
                    grabbedRb.isKinematic = false;
                    GrabbedIngredient.transform.SetParent(null);
                    //grabbedRb.transform.localPosition = Vector3.zero;
                    grbbedObjColider.enabled = true;

                    GrabbedIngredient = null;
                }
            }
            else
            {
                if (ingredientsInRange.Count > 0)
                {
                    GrabbedIngredient = ingredientsInRange[0];
                    ingredientsInRange.Remove(GrabbedIngredient);
                    grabbedRb.isKinematic = true;
                    GrabbedIngredient.transform.SetParent(transform);
                    GrabbedIngredient.transform.localPosition = Vector3.zero;
                    grbbedObjColider.enabled = false;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Factory"))
        {
            factoryInRange = other.GetComponent<Factory>();
        }
        else if (other.CompareTag("Ingredient"))
        {
            Ingredient ing = other.GetComponent<Ingredient>();
            if (ing)
            {
                if (!ingredientsInRange.Contains(ing))
                {
                    ingredientsInRange.Add(ing);
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Factory"))
        {
            factoryInRange = null;
        }
        else if(other.CompareTag("Ingredient"))
        {
            Ingredient ing = other.GetComponent<Ingredient>();
            if (ing)
            {
                if (ingredientsInRange.Contains(ing))
                {
                    ingredientsInRange.Remove(ing);
                }
            }
        }
    }
}
