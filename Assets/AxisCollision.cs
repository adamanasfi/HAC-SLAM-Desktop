using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisCollision : MonoBehaviour
{
    private Vector3 planePerpendicular, movementAxis;
    private Vector3 dragStartPosition;
    GameObject Selector;
    bool dragging;
    void Start()
    {
        Selector = transform.parent.gameObject;

        if (transform.gameObject.name == "xArrowLocal")
        {
            movementAxis = Selector.transform.right;
            planePerpendicular = Selector.transform.up;
        }

        else if (transform.gameObject.name == "yArrowLocal")
        {
            movementAxis = Selector.transform.up;
            planePerpendicular = Selector.transform.right;
        }

        else if (transform.gameObject.name == "zArrowLocal")
        {
            movementAxis = Selector.transform.forward;
            planePerpendicular = Selector.transform.up;
        }

        else if (transform.gameObject.name == "xArrowGlobal")
        {
            movementAxis = Vector3.right;
            planePerpendicular = Vector3.up;
        }
        else if (transform.gameObject.name == "yArrowGlobal")
        {
            movementAxis = Vector3.up;
            planePerpendicular = Vector3.right;
        }
        else
        {
            movementAxis = Vector3.forward;
            planePerpendicular = Vector3.up;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // mouse click
        {
            Ray firstRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(firstRay, out hit))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    dragging = true;
                    dragStartPosition = hit.point;
                }
            }
        }

        if (Input.GetMouseButton(0)) // mouse hold
        {
            if (dragging)
            {
                Ray secondRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                Plane plane = new Plane(planePerpendicular, Selector.transform.position);
                float distance;
                if (plane.Raycast(secondRay, out distance))
                {
                    Vector3 hitPoint = secondRay.GetPoint(distance);
                    Vector3 displacement = hitPoint - dragStartPosition;
                    float movement = Vector3.Dot(displacement, movementAxis.normalized);
                    Selector.transform.position += movementAxis.normalized * movement;
                    dragStartPosition = hitPoint;
                }
            }
        }

        if (Input.GetMouseButtonUp(0)) // mouse release
        {
            dragging = false;
        }
    }

    void HandleMotion(Ray secondRay, Plane plane) 
    {
        float distance;
        if (plane.Raycast(secondRay, out distance))
        {
            Vector3 hitPoint = secondRay.GetPoint(distance);
            Vector3 displacement = hitPoint - dragStartPosition;
            float movement = Vector3.Dot(displacement, movementAxis.normalized);
            Selector.transform.position += movementAxis.normalized * movement;
            dragStartPosition = hitPoint;
        }
    }  

}
