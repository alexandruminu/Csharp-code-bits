using UnityEngine;

public class SetTimeScaleOnDisable : MonoBehaviour
{
    public float timeScaleOnDisable = 1;

    private void OnDisable()
    {
        Time.timeScale = timeScaleOnDisable;
    }
}
