using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickupSystem : MonoBehaviour
{
    [SerializeField] private ItemContainer container;
    [SerializeField] private KeyCode useKey;
    [SerializeField] private List<Item> registeredItems = new List<Item>();

    private void Update()
    {
        if (Input.GetKeyDown(useKey))
        {
            if (container.HasItem)
            {
                Place();
            }
            else
            {
                Pickup();
            }
        }
    }

    void Pickup()
    {
        if(registeredItems.Count == 0) { return; }

        container.AddItem(registeredItems[0]);
        registeredItems[0].OutlineOff();
    }
    void Place()
    {
        if (registeredItems.Count == 0) { return; }

        container.RemoveItem(registeredItems[0]);
    }

    public void RegisterItem(Collider col)
    {
        Item item = col.GetComponent<Item>();
        if(registeredItems.Contains(item) == true) { return; }

        item.OutlineOn();
        registeredItems.Add(item);
    }
    public void UnregisterItem(Collider col)
    {
        Item item = col.GetComponent<Item>();
        if (registeredItems.Contains(item) == false) { return; }

        item.OutlineOff();
        registeredItems.Remove(item);
    }
}
