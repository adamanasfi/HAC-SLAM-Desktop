using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 10f;       // Movement speed
    public float rotationSpeed = 10f;  // Rotation speed
    public float smoothTime = 0.2f;     // Time for smoothing motion

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotationVelocity = Vector3.zero;

    private void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        // Get input from QWEASD keys
        float moveX = Input.GetAxis("Horizontal");  // A/D keys for left/right
        float moveZ = Input.GetAxis("Vertical");    // W/S keys for forward/backward
        float moveY = Input.GetAxis("MoveY");

        // Move the camera
        Vector3 targetMovement = new Vector3(moveX, moveY, moveZ);
        targetMovement = transform.TransformDirection(targetMovement) * moveSpeed * Time.deltaTime;

        // Smooth movement
        transform.position = Vector3.SmoothDamp(transform.position, transform.position + targetMovement, ref velocity, smoothTime);
    }

    private void HandleRotation()
    {
        // Check if the right mouse button is held down
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X"); // Horizontal mouse movement
            float mouseY = Input.GetAxis("Mouse Y"); // Vertical mouse movement

            // Target rotation
            Vector3 targetRotation = new Vector3(-mouseY, mouseX, 0f) * rotationSpeed * Time.deltaTime;

            // Smooth rotation
            transform.eulerAngles = Vector3.SmoothDamp(transform.eulerAngles, transform.eulerAngles + targetRotation, ref rotationVelocity, smoothTime);
        }
    }
}
