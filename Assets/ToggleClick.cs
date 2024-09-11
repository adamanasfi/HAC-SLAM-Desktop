using Microsoft.MixedReality.Toolkit.Experimental.InteractiveElement;
using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ToggleClick : MonoBehaviour
{
    private CompressableButton button;
    private ToggleOnEvents toggleOnEvent;
    private ToggleOffEvents toggleOffEvent;
    int state;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<CompressableButton>();
        toggleOnEvent = button.GetStateEvents<ToggleOnEvents>("ToggleOn");
        toggleOffEvent = button.GetStateEvents<ToggleOffEvents>("ToggleOff");
        state = button.States[4].Value;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0)) // 0 = left mouse button
        {
           if (state == 0)
            {
                toggleOnEvent.OnToggleOn.Invoke();
                state = 1;
                Debug.Log("TOGGLE ON");
            }
           else
            {
                toggleOffEvent.OnToggleOff.Invoke();
                state = 0;
                Debug.Log("TOGGLE OFF");
            }


        }
    }
}
