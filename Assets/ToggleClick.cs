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
    bool state;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<CompressableButton>();
        toggleOnEvent = button.GetStateEvents<ToggleOnEvents>("ToggleOn");
        toggleOffEvent = button.GetStateEvents<ToggleOffEvents>("ToggleOff");
        state = toggleOnEvent.IsSelectedOnStart;
        Debug.Log(state);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckState()
    {
        if (state) toggleOnEvent.OnToggleOn.Invoke();
        else toggleOffEvent.OnToggleOff.Invoke();
    }

    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0)) // 0 = left mouse button
        {
           if (state)
            {
                toggleOffEvent.OnToggleOff.Invoke();
                state = false;
                Debug.Log("TOGGLED OFF");
            }
           else
            {
                toggleOnEvent.OnToggleOn.Invoke();
                state = true;
                Debug.Log("TOGGLED ON");
            }


        }
    }
}
