using UnityEngine;
using TMPro;

public class SpeedometerWidget : MonoBehaviour
{
    Rigidbody rb;
    public TextMeshProUGUI speedometerText;

    float kilometerPerHourConvertRatio = 3.6f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if(rb == null)
        {
#if UNITY_EDITOR
            Debug.LogWarning("Speedometer's Rigidbody reference is NULL");
#endif
            enabled = false;
        }
    }

    private void Update()
    {
        speedometerText.SetText(Mathf.RoundToInt(rb.velocity.magnitude * kilometerPerHourConvertRatio).ToString());
    }
}
