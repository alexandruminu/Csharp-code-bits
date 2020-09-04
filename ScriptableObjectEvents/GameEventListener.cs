using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    public GameEvent Event;
    public UnityEvent Response;

    protected virtual void OnEnable()
    {
        Event.RegisterListener(this);
    }
    protected virtual void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventInvoked()
    {
        if (Event == null)
        {
#if UNITY_EDITOR
            Debug.Log("Event is NULL on:" + gameObject.name);
#endif
            return;
        }
        Response.Invoke();
    }
}
