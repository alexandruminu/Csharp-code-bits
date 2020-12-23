using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    public GameObject prefab;
    public Queue<GameObject> objects = new Queue<GameObject>();

    public ObjectPool(GameObject _prefab)
    {
        prefab = _prefab;
    }
    public GameObject Get(bool active)
    {
        if(objects.Count == 0)
        {
            ObjectPoolManager.instance.AddObjectToPool(this, active);
        }
        return objects.Dequeue();
    }

    public void ReturnToPool(GameObject objectToReturn)
    {
        objectToReturn.gameObject.SetActive(false);
        objects.Enqueue(objectToReturn);
    }
}
