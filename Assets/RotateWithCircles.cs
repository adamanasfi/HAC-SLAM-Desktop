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

    void Start()
    {
        circlesLayer = LayerMask.NameToLayer("circles");
        if (circlesLayer == -1) Debug.Log("NO CIRCLES LAYER!");

        rotatingX = false;
        rotatingY = false;
        rotatingZ = false;

        xCircle = Instantiate(circlePrefab, Vector3.zero, Quaternion.identity);
        xCircle.tag = "XCircle";

        yCircle = Instantiate(circlePrefab, Vector3.zero, Quaternion.identity);
        yCircle.tag = "YCircle";

        zCircle = Instantiate(circlePrefab, Vector3.zero, Quaternion.identity);
        zCircle.tag = "ZCircle";

        xCircle.transform.forward = transform.up;
        xCircle.transform.Rotate(0, 0, 90);
        

        yCircle.transform.forward = transform.right;
       

        zCircle.transform.forward = transform.up;
        

        // for x
        xCircle.transform.position = transform.position;
        Renderer x_renderer = xCircle.GetComponent<Renderer>();
        x_renderer.material.color = Color.red;

        // for y
        yCircle.transform.position = transform.position;
        Renderer y_renderer = yCircle.GetComponent<Renderer>();
        y_renderer.material.color = Color.green;

        // for z
        zCircle.transform.position = transform.position;
        Renderer z_renderer = zCircle.GetComponent<Renderer>();
        z_renderer.material.color = Color.blue;
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
                Debug.Log("HIT!");
                if (hit.collider.CompareTag("XCircle"))
                {
                    Debug.Log("XCIRCLE!!!!!!!!!!");
                    rotatingX = true;
                }
                if (hit.collider.CompareTag("YCircle"))
                {
                    Debug.Log("YCIRCLE!!!!!!!!!!");
                    rotatingY = true;
                }
                if (hit.collider.CompareTag("ZCircle"))
                {
                    Debug.Log("ZCIRCLE!!!!!!!!!!");
                    rotatingZ = true;
                }

                lastMousePosition = Input.mousePosition;
            }
        }

        // On mouse drag, rotate the object
        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            if (rotatingX) transform.Rotate(Vector3.right, delta.y);
            if (rotatingY) transform.Rotate(Vector3.up, delta.x);
            if (rotatingZ) transform.Rotate(Vector3.forward, delta.x);

            lastMousePosition = Input.mousePosition;
        }

        // On mouse release, stop rotating
        if (Input.GetMouseButtonUp(0))
        {
            rotatingX = rotatingY = rotatingZ = false;
        }
    }
}

