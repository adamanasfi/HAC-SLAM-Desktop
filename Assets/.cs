using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0)) // 0 = left mouse button
        {
            Debug.Log("Left click detected!");
        }
    }
}
