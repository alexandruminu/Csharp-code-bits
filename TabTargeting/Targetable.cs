using UnityEngine;

public class Targetable : MonoBehaviour
{
    [HideInInspector]
    public Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
    }
}
