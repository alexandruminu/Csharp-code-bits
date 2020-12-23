using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance;
    public Dictionary<GameObject, ObjectPool> pools = new Dictionary<GameObject, ObjectPool>();

    private void Awake()
    {
        instance = this;
    }

    public ObjectPool CreatePool(GameObject prefab)
    {
        ObjectPool addedPool = new ObjectPool(prefab);
        pools.Add(prefab, addedPool);
        return addedPool;
    }

    public void SpawnObject(GameObject onjectToSpawn, Vector3 objectPosition)
    {
        GameObject objectInstance;
        if (pools.ContainsKey(onjectToSpawn) && pools.TryGetValue(onjectToSpawn, out ObjectPool pool))
        {
            objectInstance = pool.Get(true);
        }
        else
        {
            objectInstance = CreatePool(onjectToSpawn).Get(true);
        }
        objectInstance.transform.position = objectPosition;
    }

    public void AddObjectToPool(ObjectPool pool, bool active)
    {
        GameObject newObject = Instantiate(pool.prefab);
        newObject.gameObject.SetActive(active);
        pool.objects.Enqueue(newObject);
    }
}
