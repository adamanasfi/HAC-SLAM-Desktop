using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputProvider : MonoBehaviour
{
    GameObject xCircleGlobal;
    GameObject yCircleGlobal;
    GameObject zCircleGlobal;
    bool rotatingXGlobal, rotatingYGlobal, rotatingZGlobal;
    bool rotatingXLocal, rotatingYLocal, rotatingZLocal;
    private Vector3 lastMousePosition;
    // Start is called before the first frame update
    void Start()
    {
        xCircleGlobal = transform.Find("xCircleGlobal").gameObject;
        yCircleGlobal = transform.Find("yCircleGlobal").gameObject;
        zCircleGlobal = transform.Find("zCircleGlobal").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                int objectLayer = hit.collider.gameObject.layer;
                string layerName = LayerMask.LayerToName(objectLayer);
                if (layerName == "Circles")
                {
                    MouseHandleRotation(hit);
                }

            }
        }
    }

    void MouseHandleRotation(RaycastHit hit)
    {
        if (hit.collider.CompareTag("xCircleGlobal"))
        {
            rotatingXGlobal = true;
        }
        if (hit.collider.CompareTag("yCircleGlobal"))
        {
            rotatingYGlobal = true;
        }
        if (hit.collider.CompareTag("zCircleGlobal"))
        {
            rotatingZGlobal = true;
        }

        if (hit.collider.CompareTag("xCircleLocal"))
        {
            rotatingXLocal = true;
        }
        if (hit.collider.CompareTag("yCircleLocal"))
        {
            rotatingYLocal = true;
        }
        if (hit.collider.CompareTag("zCircleLocal"))
        {
            rotatingZLocal = true;
        }
        lastMousePosition = Input.mousePosition;

        if (Input.GetMouseButton(0))
        {
            Vector3 currentMousePosition = Input.mousePosition;
            Vector3 delta = currentMousePosition - lastMousePosition;
            Ray rayStart = Camera.main.ScreenPointToRay(lastMousePosition);
            Ray rayEnd = Camera.main.ScreenPointToRay(currentMousePosition);
            // Rotate based on the quadrant
            if (rotatingXGlobal)
            {
                RotateAroundAxis(Vector3.right, rayStart, rayEnd, delta);
            }

            if (rotatingYGlobal)
            {
                RotateAroundAxis(Vector3.up, rayStart, rayEnd, delta);
            }

            if (rotatingZGlobal)
            {
                RotateAroundAxis(Vector3.forward, rayStart, rayEnd, delta);
            }

            if (rotatingXLocal)
            {
                RotateAroundAxis(transform.right, rayStart, rayEnd, delta);
            }

            if (rotatingYLocal)
            {
                RotateAroundAxis(transform.up, rayStart, rayEnd, delta);
            }

            if (rotatingZLocal)
            {
                RotateAroundAxis(transform.forward, rayStart, rayEnd, delta);
            }

            lastMousePosition = currentMousePosition;
        }
    }

    void RotateAroundAxis(Vector3 axis, Ray rayStart,Ray rayEnd, Vector3 delta)
    {
        // Create a plane at the object's position, with a normal equal to the rotation axis
        Plane rotationPlane = new Plane(axis, transform.position);

        
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
            transform.Rotate(axis, angle, Space.World);
        }
    }
}
