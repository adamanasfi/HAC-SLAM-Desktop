using UnityEngine;

public class AxisDrag : MonoBehaviour
{
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
        xArrow = Instantiate(arrowPrefab,Vector3.zero, Quaternion.identity);
        yArrow = Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity);
        zArrow = Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity);
        xArrow.transform.forward = transform.right;
        yArrow.transform.forward = transform.up;
        zArrow.transform.forward = transform.forward;
        // for x
        x_center = transform.position + xArrow.transform.forward * (0.5f);
        xArrow.transform.position = x_center;
        Renderer x_renderer = xArrow.GetComponent<Renderer>();
        x_renderer.material.color = Color.red;

        // for y
        y_center = transform.position + yArrow.transform.forward * (0.5f);
        yArrow.transform.position = y_center;
        Renderer y_renderer = yArrow.GetComponent<Renderer>();
        y_renderer.material.color = Color.green;

        // for z
        z_center = transform.position + zArrow.transform.forward * (0.5f);
        zArrow.transform.position = z_center;
        Renderer z_renderer = zArrow.GetComponent<Renderer>();
        z_renderer.material.color = Color.blue;

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


        // Handle mouse input
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if clicking near any axis
            if (Physics.Raycast(ray, out hit,Mathf.Infinity,1 << axisLayer))
            {
                float axisThreshold = 0.2f;

                // Check for X axis selection
                if (Vector3.Distance(hit.point, transform.position + transform.right) < axisThreshold)
                {
                    isDraggingX = true;
                    dragStartPosition = hit.point;
                }
                // Check for Y axis selection
                else if (Vector3.Distance(hit.point, transform.position + transform.up) < axisThreshold)
                {
                    isDraggingY = true;
                    dragStartPosition = hit.point;
                }
                // Check for Z axis selection
                else if (Vector3.Distance(hit.point, transform.position + transform.forward) < axisThreshold)
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
