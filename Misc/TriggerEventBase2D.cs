using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEventBase2D : MonoBehaviour
{
    public List<string> tags;
    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerStay;
    public UnityEvent onTriggerExit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!tags.Contains(collision.tag)) return;

        onTriggerEnter.Invoke();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!tags.Contains(collision.tag)) return;

        onTriggerStay.Invoke();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!tags.Contains(collision.tag)) return;

        onTriggerExit.Invoke();
    }
}
