using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;   // make sure Cinemachine is in your project

public class ThirdPersonController1 : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float turnSpeed = 10f; // how quickly the player model rotates toward move direction

    [Header("References")]
    public Transform cameraTransform;   // optional: for movement direction, but camera rotation is handled by FreeLook
    public Rigidbody rb;
    public Transform playerModel;       // for rotating the visible model, if you have one

    [Header("Camera / Look Settings")]
    public CinemachineFreeLook freeLookCamera; // assign your Cinemachine FreeLook here
    public float lookSensitivity = 1f;         // multiplier for how fast camera orbits
    public bool invertX = false;               // toggle in Inspector if horizontal feels backwards
    public bool invertY = false;               // toggle if vertical feels backwards

    private Vector2 moveInput;
    private Vector2 lookInput;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Optional: initialize Cinemachine axes from current camera orientation
        if (freeLookCamera != null)
        {
            // Initialize yaw/pitch from the current FreeLook orientation if desired.
            // CinemachineFreeLook m_XAxis.Value is the current horizontal angle,
            // and m_YAxis.Value is the current vertical axis (0..1).
            // For simplicity we leave as-is; input will modify from the starting position.
        }
    }

    // Called by the Input System when Move action triggers
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    // Called by the Input System when Look action triggers
    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        Move();
    }

    void LateUpdate()
    {
        HandleLook();
    }

    private void Move()
    {
        Vector3 inputDir = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
        if (inputDir.sqrMagnitude < 0.01f)
            return;

        // Rotate movement direction relative to camera's Y rotation
        if (cameraTransform != null)
        {
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;
            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();
            Vector3 moveDirection = camForward * inputDir.z + camRight * inputDir.x;

            // Smoothly rotate player model toward move direction
            if (moveDirection != Vector3.zero && playerModel != null)
            {
                Quaternion targetRot = Quaternion.LookRotation(moveDirection);
                Quaternion smoothRot = Quaternion.Slerp(playerModel.rotation, targetRot, turnSpeed * Time.fixedDeltaTime);
                rb.MoveRotation(smoothRot);
            }

            // Move the player
            Vector3 newPos = rb.position + (cameraTransform.TransformDirection(inputDir) * 0f); 
            // Note: above line is placeholder; we already computed moveDirection
            newPos = rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPos);
        }
        else
        {
            // Fallback: move relative to world axes
            Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
            if (moveDirection != Vector3.zero && playerModel != null)
            {
                Quaternion targetRot = Quaternion.LookRotation(moveDirection);
                Quaternion smoothRot = Quaternion.Slerp(playerModel.rotation, targetRot, turnSpeed * Time.fixedDeltaTime);
                rb.MoveRotation(smoothRot);
            }
            Vector3 newPos = rb.position + new Vector3(moveInput.x, 0f, moveInput.y).normalized * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPos);
        }
    }

    private void HandleLook()
    {
        if (freeLookCamera == null)
            return;

        // Read look input (mouse delta or right stick). Already stored in lookInput from OnLook.
        // Scale by sensitivity and Time.deltaTime
        float deltaX = lookInput.x * lookSensitivity * Time.deltaTime;
        float deltaY = lookInput.y * lookSensitivity * Time.deltaTime;

        // Apply inversion if toggled
        if (invertX) deltaX = -deltaX;
        if (invertY) deltaY = -deltaY;

        // CinemachineFreeLook:
        //   m_XAxis.Value is the horizontal axis (heading). Adding deltaX rotates horizontally.
        //   m_YAxis.Value is vertical: range [0,1] from top rig to bottom rig. Adding/subtracting moves the camera up/down.
        // Adjust signs on m_YAxis depending on desired up/down behavior.
        freeLookCamera.m_XAxis.Value += deltaX;

        // For vertical: moving mouse up (lookInput.y positive) should typically decrease m_YAxis.Value to look up,
        // because in FreeLook, smaller YAxis.Value moves toward top rig. But it can vary; test and invert if needed.
        freeLookCamera.m_YAxis.Value += deltaY;
        // If vertical is inverted, or it behaves opposite, swap sign:
        // freeLookCamera.m_YAxis.Value -= deltaY;

        // Optionally clamp m_YAxis.Value to [0,1], though Cinemachine does internally clamp based on its rig settings.
        // Example:
        freeLookCamera.m_YAxis.Value = Mathf.Clamp01(freeLookCamera.m_YAxis.Value);
    }
}
