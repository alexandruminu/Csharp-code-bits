using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class ItemContainer : MonoBehaviour
{
    //[SerializeField] private int maxItems = 1;
    [SerializeField] private Item item;
    [SerializeField] Transform socket;
    public bool HasItem { get { return item != null; } private set { } }
    public Item GetItem()
    {
        return item;
    }
    public void AddItem(Item _item)
    {
        item = _item;
        //item.transform.SetParent(socket);
        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = socket;
        source.weight = 1f;
        _item.parentConstraint.AddSource(source);
        _item.HandlePickUp();
        //item.transform.localPosition = Vector3.zero;
    }

    public void RemoveItem(Item _item)
    {
        item.transform.SetParent(null);
        item = null;
        _item.HandlePlace();
    }
}
