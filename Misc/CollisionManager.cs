using UnityEngine;
using UnityEngine.Events;

public class CollisionManager : MonoBehaviour
{
    public static CollisionManager instance;
    public UnityCollisionEvent onCollision;
    public UnityEvent onCollisionNoParam;

    private void Awake()
    {
        instance = this;

        if (onCollision == null)
        {
            onCollision = new UnityCollisionEvent();
            onCollisionNoParam = new UnityEvent();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        onCollision.Invoke(collision);
        onCollisionNoParam.Invoke();
    }
}

[System.Serializable]
public class UnityCollisionEvent : UnityEvent<Collision>
{
    public UnityCollisionEvent()
    {
    }
}
