using System.Collections;
using TMPro;
using UnityEngine;

public class SpeedometerPointerWidget : MonoBehaviour
{
    public float pointerStartingAngle = 140f;
    public FloatVariable maxKMPH;
    public RectTransform pointerTransform;
    public TextMeshProUGUI speedometerText;

    private Rigidbody rb;
    private int oldRbSpeed = 0;
    private int currentRbSpeed = 0;
    private float currentPointerAngle;
    readonly float kilometerPerHourConvertRatio = 3.6f;
    readonly string targetTag = "Player";

    private void Awake()
    {
        rb = GameObject.FindGameObjectWithTag(targetTag).GetComponent<Rigidbody>();
        if (rb == null)
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        currentPointerAngle = Mathf.Clamp(pointerStartingAngle - (rb.velocity.magnitude * kilometerPerHourConvertRatio / maxKMPH.Value * 280), -pointerStartingAngle, pointerStartingAngle);
        pointerTransform.localEulerAngles = new Vector3(0f, 0f, currentPointerAngle);
        currentRbSpeed = Mathf.RoundToInt(rb.velocity.magnitude * kilometerPerHourConvertRatio);
        if(currentRbSpeed != oldRbSpeed)
        {
            speedometerText.SetText(currentRbSpeed.ToString());
        }
    }
}
