using UnityEngine;

public class SetTimeScaleOnEnable : MonoBehaviour
{
    public float timeScaleOnEnable = 1;

    private void OnEnable()
    {
        Time.timeScale = timeScaleOnEnable;
    }
}
