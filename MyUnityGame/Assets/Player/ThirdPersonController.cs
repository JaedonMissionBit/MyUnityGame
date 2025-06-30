using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{
    public float jumpForce = 5f;
    public float moveSpeed = 5f;
    public float turnSpeed = 10f; // How fast the player rotates smoothly

    public Transform cameraTransform;
    public Rigidbody rb;

    private Vector2 moveInput;
    private bool isGrounded = true;

    public LayerMask groundMask;
    public Transform groundCheck;
    public float groundCheckRadius = 0.3f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            Jump();
        }
    }


    void FixedUpdate()
    {
        GroundCheck();
        Move();
    }

    void Move()
    {
        Vector3 inputDir = new Vector3(moveInput.x, 0, moveInput.y).normalized;

        if (inputDir.magnitude < 0.1f)
            return;

        // Calculate direction relative to camera
        Vector3 moveDirection = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0) * inputDir;

        // Smoothly rotate player toward move direction
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        Quaternion smoothRotation = Quaternion.Slerp(rb.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(smoothRotation);

        // Move the player
        Vector3 newPosition = rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }
    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // Reset vertical velocity
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void GroundCheck()
    {
        if (groundCheck == null) return;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);
    }
}
