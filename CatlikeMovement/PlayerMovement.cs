using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public SO_PlayerStats stats;
    public SO_String groundTag;
    public PlayerFacing playerFacing;
    public SO_Float gravity;
    private Rigidbody rbComponent;

    float horizontalInput = 0f;
    float verticalInput = 0f;
    public bool IsMovementInput { 
        get { return horizontalInput != 0 || verticalInput != 0; }
        private set { } }
    bool jumpInput = false;
    bool isGrounded = true;

    private void Awake()
    {
        rbComponent = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        RegisterInput();
        SetFacing();
    }

    private void FixedUpdate()
    {
        if (jumpInput) { Jump(); }
        if (IsMovementInput) { AcceleratePlayer(); } else { DeceleratePlayer(); }
        ApplyGravity();
    }

    void RegisterInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        if(!jumpInput)
            jumpInput = Input.GetKeyDown(KeyCode.Space) && isGrounded;
    }

    void AcceleratePlayer()
    {
        rbComponent.AddForce(new Vector3(horizontalInput, 0, verticalInput).normalized * stats.acceleration, stats.movementForceMode);
    }
    void DeceleratePlayer()
    {
        if(rbComponent.velocity.x == 0 && rbComponent.velocity.z == 0) { return; }

        rbComponent.velocity = new Vector3(0, rbComponent.velocity.y, 0);
    }

    void Jump()
    {
        if(isGrounded == false) { return; }

        rbComponent.AddRelativeForce(new Vector3(0, stats.jumpForce, 0), stats.jumpForceMode);

        jumpInput = false;
        isGrounded = false;
    }

    void ApplyGravity()
    {
        if(isGrounded) { return; }

        rbComponent.AddForce(Vector3.down * gravity.value, ForceMode.Acceleration);
    }

    void SetFacing()
    {
        if(IsMovementInput == false) { return; }

        playerFacing.SetFacing(new Vector3(horizontalInput, 0, verticalInput));
    }

    private void OnCollisionEnter(Collision collision)
    {
        CheckGroundedEnter(collision);
    }

    void CheckGroundedEnter(Collision collision)
    {
        if (isGrounded) { return; }
        TagComponent tagComponent = collision.gameObject.GetComponent<TagComponent>();
        if (tagComponent == null) { return; }

        isGrounded = tagComponent.HasTag(groundTag);
    }
}
