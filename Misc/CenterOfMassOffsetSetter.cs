using UnityEngine;

public class CenterOfMassOffsetSetter : MonoBehaviour
{
    public Vector3 centerOfMassOffset;

    private void Awake()
    {
        GetComponent<Rigidbody>().centerOfMass += centerOfMassOffset;
    }
}
