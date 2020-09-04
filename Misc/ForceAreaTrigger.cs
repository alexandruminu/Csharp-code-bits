using UnityEngine;

public class ForceAreaTrigger : MonoBehaviour
{
    public float force = 10f;

    private void OnTriggerStay(Collider other)
    {
        if(other.attachedRigidbody != null)
        {
            other.attachedRigidbody.AddForce(transform.up * force);
        }
    }
}
