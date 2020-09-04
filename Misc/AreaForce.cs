using System.Collections.Generic;
using UnityEngine;

public class AreaForce : MonoBehaviour
{
    public List<Rigidbody> AffectedObjects;
    public float force = 1f;

    void OnTriggerEnter(Collider collidee)
    {
        Rigidbody rb = collidee.GetComponentInParent<Rigidbody>();
        if(rb && !AffectedObjects.Contains(rb))
        {
            AffectedObjects.Add(rb);
        }
    }

    void OnTriggerExit(Collider collidee)
    {
        AffectedObjects.Remove(collidee.GetComponentInParent<Rigidbody>());
    }

    void FixedUpdate()
    {
        for (int i = 0; i < AffectedObjects.Count; i++)
        {
            AffectedObjects[i].GetComponentInParent<Rigidbody>().AddForce(transform.forward * force, ForceMode.VelocityChange);
        }
    }
}
