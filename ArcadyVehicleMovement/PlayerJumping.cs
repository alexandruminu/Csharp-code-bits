using UnityEngine;

public class PlayerJumping : MonoBehaviour
{
    public FloatVariable jumpForce;

    Rigidbody rb;
    bool jumpInput = false;

    private void Awake()
    {
        rb = GetComponentInParent<Rigidbody>();
        PlayerInputBindingManager.Controls.Player.Jump.performed += ctx => TryJump();
    }

    void TryJump()
    {
        if (PlayerMovement.instance.IsGrounded == false) return;
        if (jumpInput) return;

        Jump();
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce.Value, ForceMode.VelocityChange);
    }
}
