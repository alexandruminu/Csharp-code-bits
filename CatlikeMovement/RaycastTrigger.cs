using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RaycastTrigger : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float length = 4f;
    [Range(0.1f, 30f)]
    public float cooldown = 3f;
    public LayerMask mask;
    public UnityEvent onTriggerEnable;
    public UnityEvent onTriggerEnter;

    private void OnValidate()
    {
        lineRenderer.SetPosition(1, Vector3.forward * length);
    }
    private void Awake()
    {
        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.SetPosition(1, Vector3.forward * length);
    }
    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, transform.forward, length, mask, QueryTriggerInteraction.Ignore))
        {
            onTriggerEnter.Invoke();
        }
    }

    public void Cooldown()
    {
        Invoke("EnableTrigger", cooldown);
        enabled = false;
    }

    void EnableTrigger()
    {
        onTriggerEnable.Invoke();
    }
}
