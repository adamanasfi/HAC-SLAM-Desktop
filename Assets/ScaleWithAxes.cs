using UnityEngine;

public class ScaleWithAxes : MonoBehaviour
{
    public float scaleSpeed = 0.1f; // Speed multiplier for scaling
    private Vector3 originalScale;  // To store the initial scale
    private Vector3 dragStartWorld; // To store the drag start position in world space
    private LayerMask axisLayer;
    public GameObject xArrow;
    public GameObject yArrow;
    public GameObject zArrow;
    public GameObject arrowPrefab;
    Vector3 x_center, y_center, z_center;
    private bool draggingX = false, draggingY = false, draggingZ = false;

    void OnEnable()
    {
        axisLayer = LayerMask.NameToLayer("axes");
        if (axisLayer == -1) Debug.Log("NO AXES LAYER!");

        xArrow = Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity);
        yArrow = Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity);
        zArrow = Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity);

        xArrow.transform.forward = transform.right;
        yArrow.transform.forward = transform.up;
        zArrow.transform.forward = transform.forward;

        // Set up arrow positions and colors
        SetupArrow(xArrow, transform.right, Color.red);
        SetupArrow(yArrow, transform.up, Color.green);
        SetupArrow(zArrow, transform.forward, Color.blue);
    }

    private void SetupArrow(GameObject arrow, Vector3 direction, Color color)
    {
        Vector3 center = transform.position + direction * 0.5f;
        arrow.transform.position = center;
        Renderer renderer = arrow.GetComponent<Renderer>();
        renderer.material.color = color;
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

        // Handle Mouse Input
        if (Input.GetMouseButtonDown(0))  // On left mouse button click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << axisLayer))
            {
                float axisThreshold = 0.2f;
                
                if (Vector3.Distance(hit.point, transform.position + transform.right) < axisThreshold)
                {
                    draggingX = true;
                    dragStartWorld = hit.point;
                }
                else if (Vector3.Distance(hit.point, transform.position + transform.up) < axisThreshold)
                {
                    draggingY = true;
                    dragStartWorld = hit.point;
                }
                else if (Vector3.Distance(hit.point, transform.position + transform.forward) < axisThreshold)
                {
                    draggingZ = true;
                    dragStartWorld = hit.point;
                }
            }
        }

        // Handle Dragging
        if (Input.GetMouseButton(0))  // While holding the mouse button
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane plane;
            if (draggingX)
            {
                plane = new Plane(transform.up, transform.position);
                ScaleAlongAxis(ray, plane, transform.right);
            }
            else if (draggingY)
            {
                plane = new Plane(transform.right, transform.position);
                ScaleAlongAxis(ray, plane, transform.up);
            }
            else if (draggingZ)
            {
                plane = new Plane(transform.up, transform.position);
                ScaleAlongAxis(ray, plane, transform.forward);
            }
        }

        // Stop Dragging
        if (Input.GetMouseButtonUp(0))  // On left mouse button release
        {
            draggingX = false;
            draggingY = false;
            draggingZ = false;
        }
    }

    private void ScaleAlongAxis(Ray ray, Plane plane, Vector3 axis)
    {
        float enter;
        if (plane.Raycast(ray, out enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            Vector3 dragDelta = hitPoint - dragStartWorld;

            // Project the dragDelta onto the axis to get the movement along that axis
            float movement = Vector3.Dot(dragDelta, axis.normalized);
            Vector3 newScale = transform.localScale;
            // Apply the new scale
            if (axis == transform.right)
            {
                newScale.x += movement;
            }
            else if (axis == transform.up)
            {
                newScale.y += movement;
            }
            else if (axis == transform.forward)
            {
                newScale.z += movement;
            }
            transform.localScale = newScale;
            dragStartWorld = hitPoint;
        }
    }
}
