using UnityEngine;

public class ExamplePooledObject : MonoBehaviour
{
    public void ReturnToPool ()
    {
        ExamplePool.Instance.ReturnToPool(this);
    }
}
