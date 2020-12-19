using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerComponent : MonoBehaviour
{
    public List<SO_String> compatibleTags = new List<SO_String>();
    public ColliderEvent onTriggerEnter;
    public ColliderEvent onTriggerExit;

    private void OnTriggerEnter(Collider other)
    {
        if(compatibleTags.Count == 0)
        {
            Debug.LogError("Trigger component has no compatible tags");
        }
        TagComponent tag = other.GetComponent<TagComponent>();
        if (tag != null && tag.HasAnyOf(compatibleTags))
        {
            onTriggerEnter.Invoke(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (compatibleTags.Count == 0)
        {
            Debug.LogError("Trigger component has no compatible tags");
        }
        TagComponent tag = other.GetComponent<TagComponent>();
        if (tag != null && tag.HasAnyOf(compatibleTags))
        {
            onTriggerExit.Invoke(other);
        }
    }
}
