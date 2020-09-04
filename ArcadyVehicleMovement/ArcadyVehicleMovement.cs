using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class uses scriptable object variables, these can be replaced with regular ones and remove the .Value when using them
/// </summary>
public class ArcadyVehicleMovement : MonoBehaviour
{
    public static ArcadyVehicleMovement instance;

    public bool IsGrounded { get { return surfaceNormal != Vector3.zero; } }

    public ForceMode driveForceMode;
    public FloatVariable maxKmph;
    public FloatVariable KMPHConversionFactor;
    private float maxSpeed = 100f;
    public AnimationCurveVariable speedMultiplierCurve;
    public FloatVariable speedBackwards;
    [HideInInspector] public UnityEvent onRecalculateMaxSpeed;

    public ForceMode rotateForceMode;
    public FloatVariable rotateSpeedOnGround;
    public FloatVariable rotateSpeedInAir;

    public FloatVariable rbAngularDragOnGround;
    public FloatVariable rbAngularDragInAir;
    public FloatVariable rbDragOnGround, rbDragInAir;

    public FloatVariable uprightForceOnGround;
    public FloatVariable uprightForceInAir;
    public Vector3Variable downForceInAir;

    public bool IsFlipped { get { return transform.up.y < 0.15f && rb.velocity.magnitude < 0.1f; } }
    public FloatVariable flipTorque;

    public bool takeInput = true;

    public List<Transform> suspensionRaycastOrigins = new List<Transform>();
    public List<Wheel> wheels = new List<Wheel>();
    public Animator carBodyAnimator;
    public Transform carBody;
    public List<Animator> wheelAnimators = new List<Animator>();
    public List<Transform> frontWheels = new List<Transform>();
    public LayerMask suspensionRaycastLayerMask;
    public float suspensionRaycastDistance = 0.5f;
    public float suspensionForce = 10f;
    public float suspensionNegativeForce = 10f;
    public float suspensionMoveWheelMultiplier = 1f;
    public float sidewaysTractionFactor = 1f;


    [SerializeField] Transform transformComponent;
    [SerializeField] Rigidbody rb;
    float accelerateInput;
    public float accelerateCurveValue = 0f;
    public float accelerateIncrease = 0.5f;
    public float accelerateDecrease = 0.5f;
    public AnimationCurve accelerateAnimCurve;
    public float accelerateAnimCurveValue = 0f;
    public float accelerateAnimValueIncrease = 1f;
    public float accelerateAnimValueDecrease = 1f;
    public AnimationCurve rotateAnimCurve;
    public float rotateAnimCurveValue = 0f;
    public float rotateAnimValueIncrease = 1f;
    public float rotateAnimValueDecrease = 1f;
    float steerInput;
    float rotateLastFrame;
    float motor;
    Vector3 surfaceNormal = Vector3.zero;


    private void Awake()
    {
        instance = this;

        PlayerInputBindingManager.Controls.Player.Move.performed += ctx => accelerateInput = ctx.ReadValue<float>();
        PlayerInputBindingManager.Controls.Player.Move.canceled += ctx => accelerateInput = 0;

        PlayerInputBindingManager.Controls.Player.Steer.performed += ctx => steerInput = ctx.ReadValue<float>();
        PlayerInputBindingManager.Controls.Player.Steer.canceled += ctx => steerInput = 0;

        foreach (Wheel wheel in wheels)
        {
            wheel.wheelStartPos = wheel.wheel.transform.localPosition;
        }
        maxSpeed = maxKmph.Value / KMPHConversionFactor.Value;
    }

    private void OnEnable()
    {
        PlayerInputBindingManager.instance.EnableControls();
    }

    private void OnDisable()
    {
        PlayerInputBindingManager.instance.DisableControls();
    }

    private void Update()
    {
        if (takeInput)
        {
            //Physical acceleration
            if (accelerateInput != 0f)
            {
                accelerateCurveValue = Mathf.Clamp01(accelerateCurveValue + accelerateIncrease * Time.deltaTime);
            }
            else
            {
                accelerateCurveValue = Mathf.Clamp01(accelerateCurveValue - accelerateDecrease * Time.deltaTime);
            }
            //Animation for acceleration
            accelerateAnimCurveValue = Mathf.Lerp(accelerateAnimCurveValue, accelerateInput, Time.deltaTime * accelerateAnimValueIncrease);
            rotateAnimCurveValue = Mathf.Lerp(rotateAnimCurveValue, steerInput * Mathf.Sign(accelerateInput), Time.deltaTime * rotateAnimValueIncrease);
        }
        else
        {
            accelerateInput = 0;
            steerInput = 0;
        }

        for (int i = 0; i < wheelAnimators.Count; i++)
        {
            wheelAnimators[i].SetFloat("Z", accelerateAnimCurve.Evaluate(accelerateAnimCurveValue));
            wheelAnimators[i].SetFloat("X", rotateAnimCurve.Evaluate(rotateAnimCurveValue));
        }
        carBodyAnimator.SetFloat("Z", accelerateAnimCurve.Evaluate(accelerateAnimCurveValue));
        carBodyAnimator.SetFloat("X", rotateAnimCurve.Evaluate(rotateAnimCurveValue));

        if (takeInput)
        {
            foreach (Transform wheel in frontWheels)
            {
                wheel.localEulerAngles = new Vector3(0, 45f * steerInput);
            }
        }
        else
        {
            foreach (Transform wheel in frontWheels)
            {
                wheel.localEulerAngles = new Vector3(0, 0);
            }
        }
    }

    private void FixedUpdate()
    {
        if (accelerateInput != 0)
        {
            motor = speedMultiplierCurve.Value.Evaluate(accelerateCurveValue) * accelerateInput;
        }
        else
        {
            motor = 0;
        }

        if (IsFlipped)
        {
            //rb.AddTorque(transform.forward * flipTorque * Input.GetAxis("Horizontal"), ForceMode.Acceleration);
        }

        surfaceNormal = Vector3.zero;

        foreach (Wheel suspension in wheels)
        {
            Vector3 suspensionDirection = suspension.suspensionRaycastOrigin.up;
            Vector3 suspensionOrigin = suspension.suspensionRaycastOrigin.position + suspension.offset;
            float comressionRatio = 0;

            Debug.DrawRay(suspensionOrigin, -suspensionDirection * suspensionRaycastDistance, Color.green, 0, false);

            if (Physics.Raycast(suspensionOrigin, -suspensionDirection, out RaycastHit hit, suspensionRaycastDistance, suspensionRaycastLayerMask))
            {
                comressionRatio = 1f - hit.distance / suspensionRaycastDistance;
                //rb.AddForceAtPosition(suspensionDirection * suspensionForce * comressionRatio, suspensionOrigin, ForceMode.Acceleration);
                rb.AddForceAtPosition(suspensionDirection * GetRequiredAcceleraton(suspensionForce * comressionRatio, rb.drag), suspensionOrigin, ForceMode.Acceleration);

                surfaceNormal += hit.normal;
            }
            else
            {
                //rb.AddForceAtPosition(-Vector3.up * suspensionNegativeForce, suspensionOrigin, ForceMode.Acceleration);
            }
            if (surfaceNormal == Vector3.zero)
            {
                //rb.AddForceAtPosition(-Vector3.up * suspensionNegativeForce, suspension.suspensionRaycastOrigin.position);
            }
            //suspension.wheel.localPosition = suspension.wheelStartPos - new Vector3(0, (1 - comressionRatio) * suspensionMoveWheelMultiplier);
        }

        // if grounded
        Quaternion rot = Quaternion.FromToRotation(transform.up, Vector3.up);
        if (surfaceNormal != Vector3.zero)
        {
            rot = Quaternion.FromToRotation(transform.up, surfaceNormal);

            rb.angularDrag = rbAngularDragOnGround.Value;
            rb.drag = rbDragOnGround.Value;
        }
        else
        {
            rot = Quaternion.FromToRotation(transform.up, Vector3.up);

            rb.angularDrag = rbAngularDragInAir.Value;
            rb.drag = rbDragInAir.Value;

            rb.AddForce(downForceInAir.Value, ForceMode.Acceleration);
        }
        rb.AddTorque(new Vector3(rot.x, rot.y, rot.z) * uprightForceInAir.Value, ForceMode.Acceleration);

#if UNITY_EDITOR
        Debug.DrawRay(rb.worldCenterOfMass, Vector3.ProjectOnPlane(transform.forward, surfaceNormal.normalized) * motor * 4, Color.magenta, 0, false);
#endif

        if (motor != 0)
        {
            if (surfaceNormal != Vector3.zero)
            {
                if (motor > 0)
                {
                    rb.AddForceAtPosition(
                        Vector3.ProjectOnPlane(transform.forward, surfaceNormal.normalized) * GetRequiredAcceleraton(motor * maxSpeed, rb.drag) * accelerateInput,
                        rb.worldCenterOfMass,
                        driveForceMode);
                }
                else
                {
                    rb.AddForceAtPosition(
                        Vector3.ProjectOnPlane(-transform.forward, surfaceNormal.normalized) * GetRequiredAcceleraton(motor * speedBackwards.Value, rb.drag) * accelerateInput,
                        rb.worldCenterOfMass,
                        driveForceMode);
                }
            }
        }

        ApplyTorqueByInput();

        Debug.DrawRay(transform.position, transform.right * transform.InverseTransformDirection(-rb.velocity).normalized.x * sidewaysTractionFactor, Color.magenta, 0, false);
        //Debug.Log(transform.right * transform.InverseTransformDirection(-rb.velocity).normalized.x);
        rb.AddForce(transform.right * transform.InverseTransformDirection(-rb.velocity).normalized.x * sidewaysTractionFactor, ForceMode.Acceleration);
    }

    /// <summary>
    /// Applies torque to the rigidbody based on input
    /// </summary>
    public void ApplyTorqueByInput()
    {
        if (steerInput != 0)
        {
            if (Mathf.Sign(steerInput) != Mathf.Sign(rotateLastFrame))
            {
                rb.angularVelocity = Vector3.zero;
            }

            if (surfaceNormal != Vector3.zero)
            {
                rb.AddTorque(transform.up * steerInput * Mathf.Sign(accelerateInput) * rotateSpeedOnGround.Value, rotateForceMode);
            }
            else if (surfaceNormal == Vector3.zero)
            {
                rb.AddTorque(transform.up * steerInput * Mathf.Sign(accelerateInput) * rotateSpeedInAir.Value, rotateForceMode);
            }

            rotateLastFrame = steerInput;
        }
    }

    float GetFinalVelocity(float aVelocityChange, float aDrag)
    {
        return aVelocityChange * (1 / Mathf.Clamp01(aDrag * Time.fixedDeltaTime) - 1);
    }
    float GetFinalVelocityFromAcceleration(float aAcceleration, float aDrag)
    {
        return GetFinalVelocity(aAcceleration * Time.fixedDeltaTime, aDrag);
    }


    float GetDrag(float aVelocityChange, float aFinalVelocity)
    {
        return aVelocityChange / ((aFinalVelocity + aVelocityChange) * Time.fixedDeltaTime);
    }
    float GetDragFromAcceleration(float aAcceleration, float aFinalVelocity)
    {
        return GetDrag(aAcceleration * Time.fixedDeltaTime, aFinalVelocity);
    }


    float GetRequiredVelocityChange(float aFinalSpeed, float aDrag)
    {
        float m = Mathf.Clamp01(aDrag * Time.fixedDeltaTime);
        return aFinalSpeed * m / (1 - m);
    }
    float GetRequiredAcceleraton(float aFinalSpeed, float aDrag)
    {
        return GetRequiredVelocityChange(aFinalSpeed, aDrag) / Time.fixedDeltaTime;
    }

    public void RecalculateMaxKMPH()
    {
        maxSpeed = 0;
        ModifyMaxKMPH(maxKmph.Value);
        onRecalculateMaxSpeed.Invoke();
    }

    public void ModifyMaxKMPH(float kmph)
    {
        maxSpeed += kmph / KMPHConversionFactor.Value;
    }
}
