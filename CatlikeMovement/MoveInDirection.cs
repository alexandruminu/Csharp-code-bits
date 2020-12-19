using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInDirection : MonoBehaviour
{
    [SerializeField] private Vector3 direction = Vector3.forward;
    [SerializeField] private float speed = 1f;

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
