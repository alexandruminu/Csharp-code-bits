using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOverTime : MonoBehaviour
{
    public float speed = 45f;
    void Update()
    {
        transform.Rotate(transform.up * speed * Time.deltaTime);
    }
}
