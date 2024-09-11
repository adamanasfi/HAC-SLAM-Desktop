using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClick : MonoBehaviour
{
    private ButtonConfigHelper button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<ButtonConfigHelper>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0)) // 0 = left mouse button
        {
            button.OnClick.Invoke();
            
        }
    }
}
