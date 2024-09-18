using UnityEngine;

public class TaskbarPlacement : MonoBehaviour
{
    public Camera mainCamera;  // Reference to the camera
    public Vector3 offset;     // Offset from the camera

    void Start()
    {
        if (!mainCamera)
        {
            mainCamera = Camera.main;  // Automatically assign the main camera if not set
        }
    }

    void Update()
    {
        // Keep the HandMenu at a fixed position relative to the camera
        transform.position = mainCamera.transform.position + mainCamera.transform.forward * offset.z +
                             mainCamera.transform.right * offset.x +
                             mainCamera.transform.up * offset.y;
        transform.rotation = mainCamera.transform.rotation;
    }
}
