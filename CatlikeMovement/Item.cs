using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    Collider colliderComponent;
    Rigidbody rbComponent;

    private void Awake()
    {
        colliderComponent = GetComponent<Collider>();
        rbComponent = GetComponent<Rigidbody>();
    }

    public void HandlePickUp()
    {
        colliderComponent.isTrigger = true;
        rbComponent.isKinematic = true;
    }

    public void HandlePlace()
    {
        colliderComponent.isTrigger = false;
        rbComponent.isKinematic = false;
    }
}
