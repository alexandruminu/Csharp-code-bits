using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public enum Type
    {
        Type1,
        Type2,
        Type3
    }
    public Type ingredientType;

    public Rigidbody rb;
    Collider col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        rb.isKinematic = false;
        col.enabled = true;
    }
}
