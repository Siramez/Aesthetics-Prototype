using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Look Settings")]
    public float lookSensitivity = 2f;
    public float maxLookAngle = 80f;

    private Rigidbody rb;
    private float verticalLookRotation = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Move();
        Look();
    }

    private void Move()
    {
        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow
        float moveZ = Input.GetAxis("Vertical");   // W/S or Up/Down Arrow

        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;
        Vector3 velocity = moveDirection * moveSpeed;

        Vector3 yVelocity = new Vector3(0, rb.velocity.y, 0); // Preserve Y velocity (gravity)
        rb.velocity = velocity + yVelocity;
    }

    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

        // Rotate the player horizontally
        transform.Rotate(Vector3.up * mouseX);

        // Rotate the camera vertically
        verticalLookRotation -= mouseY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -maxLookAngle, maxLookAngle);

        Camera playerCamera = GetComponentInChildren<Camera>();
        if (playerCamera != null)
        {
            playerCamera.transform.localRotation = Quaternion.Euler(verticalLookRotation, 0f, 0f);
        }
    }
}
