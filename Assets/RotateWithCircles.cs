using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithCircles : MonoBehaviour
{
    public GameObject circlePrefab;
    public GameObject xCircle, yCircle, zCircle;
    private LayerMask circlesLayer;
    private bool rotatingX, rotatingY, rotatingZ;
    private Vector3 lastMousePosition;

    void OnEnable()
    {
        rotatingX = false;
        rotatingY = false;
        rotatingZ = false;

        if (xCircle == null)
        {
            circlesLayer = LayerMask.NameToLayer("circles");
            if (circlesLayer == -1) Debug.Log("NO CIRCLES LAYER!");

            xCircle = Instantiate(circlePrefab, Vector3.zero, Quaternion.identity);
            xCircle.tag = "XCircle";

            yCircle = Instantiate(circlePrefab, Vector3.zero, Quaternion.identity);
            yCircle.tag = "YCircle";

            zCircle = Instantiate(circlePrefab, Vector3.zero, Quaternion.identity);
            zCircle.tag = "ZCircle";

            Transform x_child = xCircle.transform.Find("default");
            Renderer x_renderer = x_child.GetComponent<Renderer>();
            x_renderer.material.SetColor("_Color", Color.red);

            Transform y_child = yCircle.transform.Find("default");
            Renderer y_renderer = y_child.GetComponent<Renderer>();
            y_renderer.material.SetColor("_Color", Color.green);

            Transform z_child = zCircle.transform.Find("default");
            Renderer z_renderer = z_child.GetComponent<Renderer>();
            z_renderer.material.SetColor("_Color", Color.blue);
        }

        xCircle.transform.up = transform.right;
        

        yCircle.transform.up = transform.up;
       
        zCircle.transform.up = transform.forward;
        

        // for x
        xCircle.transform.position = transform.position;
        
        // for y
        yCircle.transform.position = transform.position;

        // for z
        zCircle.transform.position = transform.position;
    }

    void Update()
    {
        xCircle.transform.position = transform.position;
        yCircle.transform.position = transform.position;
        zCircle.transform.position = transform.position;

        // On mouse down, check if a circle is clicked
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << circlesLayer))
            {
                if (hit.collider.CompareTag("XCircle"))
                {
                    rotatingX = true;
                }
                if (hit.collider.CompareTag("YCircle"))
                {
                    rotatingY = true;
                }
                if (hit.collider.CompareTag("ZCircle"))
                {
                    rotatingZ = true;
                }

                lastMousePosition = Input.mousePosition;
            }
        }

        // On mouse drag, rotate the object
        if (Input.GetMouseButton(0))
        {
            Vector3 currentMousePosition = Input.mousePosition;
            Vector3 delta = currentMousePosition - lastMousePosition;

            // Rotate based on the quadrant
            if (rotatingX)
            {
                RotateAroundAxis(transform.right, currentMousePosition, delta);
            }

            if (rotatingY)
            {
                RotateAroundAxis(transform.up, currentMousePosition, delta);
            }

            if (rotatingZ)
            {
                RotateAroundAxis(transform.forward, currentMousePosition, delta);
            }

            lastMousePosition = currentMousePosition;
        }

        // On mouse release, stop rotating
        if (Input.GetMouseButtonUp(0))
        {
            rotatingX = rotatingY = rotatingZ = false;
        }
    }

    void RotateAroundAxis(Vector3 axis, Vector3 currentMousePosition, Vector3 delta)
    {
        // Create a plane at the object's position, with a normal equal to the rotation axis
        Plane rotationPlane = new Plane(axis, transform.position);

        // Raycast from the mouse position onto the plane
        Ray rayStart = Camera.main.ScreenPointToRay(lastMousePosition);
        Ray rayEnd = Camera.main.ScreenPointToRay(currentMousePosition);

        float enterStart, enterEnd;
        if (rotationPlane.Raycast(rayStart, out enterStart) && rotationPlane.Raycast(rayEnd, out enterEnd))
        {
            // Get the world positions where the mouse ray intersects the plane
            Vector3 worldStart = rayStart.GetPoint(enterStart);
            Vector3 worldEnd = rayEnd.GetPoint(enterEnd);

            // Get direction vectors from the object's center to the intersection points
            Vector3 fromCenterToLastPos = worldStart - transform.position;
            Vector3 fromCenterToCurrentPos = worldEnd - transform.position;

            // Calculate the angle between the vectors and rotate the object
            float angle = Vector3.SignedAngle(fromCenterToLastPos, fromCenterToCurrentPos, axis);
            transform.Rotate(axis, angle,Space.World);
            if (axis == transform.right) // x axis
            {
                yCircle.transform.Rotate(axis, angle, Space.World);
                zCircle.transform.Rotate(axis, angle, Space.World);
            }
            else if (axis == transform.up) // y axis
            {
                xCircle.transform.Rotate(axis, angle, Space.World);
                zCircle.transform.Rotate(axis, angle, Space.World);
            }
            else // z axis
            {
                xCircle.transform.Rotate(axis, angle, Space.World);
                yCircle.transform.Rotate(axis, angle, Space.World);
            }

        }
    }
}

