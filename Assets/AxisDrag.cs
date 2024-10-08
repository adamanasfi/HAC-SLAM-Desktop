using UnityEngine;

public class AxisDrag : MonoBehaviour
{
    public bool allowedMotion;
    private bool isDraggingX = false;
    private bool isDraggingY = false;
    private bool isDraggingZ = false;
    private LayerMask axisLayer;
    private Vector3 dragStartPosition;
    private Camera mainCamera;
    public GameObject xArrow;
    public GameObject yArrow;
    public GameObject zArrow;
    public GameObject arrowPrefab;
    Vector3 x_center, y_center, z_center;


    void OnEnable()
    {
        axisLayer = LayerMask.NameToLayer("axes");
        if (axisLayer == -1) Debug.Log("NO AXES LAYER!");
        mainCamera = Camera.main;
        if (xArrow == null)
        {
            xArrow = Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity);
            xArrow.tag = "XArrow";
            yArrow = Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity);
            yArrow.tag = "YArrow";
            zArrow = Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity);
            zArrow.tag = "ZArrow";
            Renderer x_renderer = xArrow.GetComponent<Renderer>();
            x_renderer.material.color = Color.red;
            Renderer y_renderer = yArrow.GetComponent<Renderer>();
            y_renderer.material.color = Color.green;
            Renderer z_renderer = zArrow.GetComponent<Renderer>();
            z_renderer.material.color = Color.blue;
        }

        xArrow.transform.forward = transform.right;
        yArrow.transform.forward = transform.up;
        zArrow.transform.forward = transform.forward;
        // for x
        x_center = transform.position + xArrow.transform.forward * (0.5f);
        xArrow.transform.position = x_center;


        // for y
        y_center = transform.position + yArrow.transform.forward * (0.5f);
        yArrow.transform.position = y_center;


        // for z
        z_center = transform.position + zArrow.transform.forward * (0.5f);
        zArrow.transform.position = z_center;


    }



    void Update()
    {
        // for x
        x_center = transform.position + xArrow.transform.forward * (0.5f);
        xArrow.transform.position = x_center;
        // for y
        y_center = transform.position + yArrow.transform.forward * (0.5f);
        yArrow.transform.position = y_center;
        // for z
        z_center = transform.position + zArrow.transform.forward * (0.5f);
        zArrow.transform.position = z_center;

        xArrow.transform.forward = transform.right;
        yArrow.transform.forward = transform.up;
        zArrow.transform.forward = transform.forward;

        if (allowedMotion)
        {
            // Handle mouse input
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // Check if clicking near any axis
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << axisLayer))
                {
                    // Check for X axis selection
                    if (hit.collider.CompareTag("XArrow"))
                    {
                        isDraggingX = true;
                        dragStartPosition = hit.point;
                    }
                    // Check for Y axis selection
                    else if (hit.collider.CompareTag("YArrow"))
                    {
                        isDraggingY = true;
                        dragStartPosition = hit.point;
                    }
                    // Check for Z axis selection
                    else if (hit.collider.CompareTag("ZArrow"))
                        {
                        isDraggingZ = true;
                        dragStartPosition = hit.point;
                    }
                }
            }

            if (Input.GetMouseButton(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                Plane plane;

                if (isDraggingX)
                {
                    plane = new Plane(transform.up, transform.position); // Movement plane perpendicular to Y axis
                    MoveAlongAxis(ray, plane, transform.right);
                }
                else if (isDraggingY)
                {
                    plane = new Plane(transform.right, transform.position); // Movement plane perpendicular to X axis
                    MoveAlongAxis(ray, plane, transform.up);
                }
                else if (isDraggingZ)
                {
                    plane = new Plane(transform.up, transform.position); // Movement plane perpendicular to Y axis
                    MoveAlongAxis(ray, plane, transform.forward);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                // Stop dragging when mouse is released
                isDraggingX = isDraggingY = isDraggingZ = false;
            }
        }
    }

    void MoveAlongAxis(Ray ray, Plane plane, Vector3 axis)
    {
        float distance;
        if (plane.Raycast(ray, out distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);
            Vector3 displacement = hitPoint - dragStartPosition;

            // Project displacement onto the axis
            float movement = Vector3.Dot(displacement, axis.normalized);
            transform.position += axis.normalized * movement;
            dragStartPosition = hitPoint;
        }
    }

}
