using UnityEngine;

/// <summary>
/// Create the controls with the Input Manager in Unity
/// </summary>
public class PlayerInputBindingManager : MonoBehaviour
{
    public static PlayerInputBindingManager instance;

    private CommonInputActions controls;
    public static CommonInputActions Controls { get { return instance.controls; } }

    private void Awake()
    {
        instance = this;
        controls = new CommonInputActions();
    }

    public void EnableControls()
    {
        controls.Enable();
    }
    public void DisableControls()
    {
        controls.Disable();
    }

    private void OnEnable()
    {
    }
    private void OnDisable()
    {
        controls.Disable();
    }
}
