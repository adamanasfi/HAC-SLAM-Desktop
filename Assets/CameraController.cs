using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 0.1f;        // Movement speed
    public float rotationSpeed = 0.1f;   // Rotation speed

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

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? moveSpeed * 2f : moveSpeed;

        // Move the camera directly
        Vector3 movement = new Vector3(moveX, moveY, moveZ) * currentSpeed * Time.deltaTime;
        transform.Translate(movement, Space.Self);
    }

    private void HandleRotation()
    {
        // Check if the right mouse button is held down
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime; // Horizontal mouse movement
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime; // Vertical mouse movement

            // Rotate the camera
            transform.Rotate(Vector3.up, mouseX, Space.World);        // Yaw rotation
            transform.Rotate(Vector3.right, -mouseY, Space.Self);     // Pitch rotation
        }
    }
}
