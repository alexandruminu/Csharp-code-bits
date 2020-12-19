using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TagComponent : MonoBehaviour
{
    [SerializeField] private List<SO_String> tags = new List<SO_String>();

    public bool HasTag(SO_String tag)
    {
        return tags.Contains(tag);
    }
    public bool HasAnyOf(List<SO_String> _tags)
    {
        return _tags.Intersect(tags).Count() > 0;
    }
}
