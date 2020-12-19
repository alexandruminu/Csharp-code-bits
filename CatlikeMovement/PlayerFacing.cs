using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFacing : MonoBehaviour
{
    public void SetFacing(Vector3 direction)
    {
        transform.localRotation = Quaternion.LookRotation(direction, Vector3.up);
    }
}
