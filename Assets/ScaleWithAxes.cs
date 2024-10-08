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

        // Set up arrow positions
        SetupArrow(xArrow, transform.right);
        SetupArrow(yArrow, transform.up);
        SetupArrow(zArrow, transform.forward);
    }

    private void SetupArrow(GameObject arrow, Vector3 direction)
    {
        Vector3 center = transform.position + direction * 0.5f;
        arrow.transform.position = center;
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
                if (hit.collider.CompareTag("XArrow"))
                {
                    draggingX = true;
                    dragStartWorld = hit.point;
                }
                else if (hit.collider.CompareTag("YArrow"))
                {
                    draggingY = true;
                    dragStartWorld = hit.point;
                }
                else if (hit.collider.CompareTag("ZArrow"))
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
