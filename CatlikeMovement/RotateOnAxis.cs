using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOnAxis : MonoBehaviour
{
    [SerializeField] private Vector3 axis = Vector3.up;
    [SerializeField] private float speed = 45f;

    private void Update()
    {
        transform.Rotate(axis * speed * Time.deltaTime);
    }
}
