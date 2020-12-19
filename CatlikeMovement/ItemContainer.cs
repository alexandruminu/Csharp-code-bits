using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        item.transform.SetParent(socket);
        _item.HandlePickUp();
        item.transform.localPosition = Vector3.zero;
    }

    public void RemoveItem(Item _item)
    {
        item.transform.SetParent(null);
        item = null;
        _item.HandlePlace();
    }
}
