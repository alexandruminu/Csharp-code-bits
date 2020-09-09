using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject prefab;
    public float spawnInterval = 2f;
    float timer = 0f;
    public float spawnRange = 1f;

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > spawnInterval)
        {
            Vector2 rand = Random.insideUnitCircle * spawnRange;
            Vector3 spawnPos = transform.position;
            spawnPos.x += rand.x;
            spawnPos.z += rand.y;
            ObjectPoolManager.instance.SpawnObject(prefab, spawnPos);
            timer = 0f;
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, spawnRange);
    }
}
