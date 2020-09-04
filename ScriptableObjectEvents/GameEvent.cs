using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent", menuName = "ScriptableObjects/GameEvent", order = 1)]
public class GameEvent : ScriptableObject
{
    private List<GameEventListener> listeners = new List<GameEventListener>();

    public void Invoke()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventInvoked();
        }
    }

    public void RegisterListener(GameEventListener listener)
    {
        if (listeners.Contains(listener)) return;

        listeners.Add(listener);
    }
    public void UnregisterListener(GameEventListener listener)
    {
        if (!listeners.Contains(listener)) return;

        listeners.Remove(listener);
    }

    public void ClearListeners()
    {
        listeners.Clear();
    }
}
