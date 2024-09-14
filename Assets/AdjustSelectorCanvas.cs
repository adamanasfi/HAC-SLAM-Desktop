using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdjustSelectorCanvas : MonoBehaviour
{
    public TMP_InputField x;
    public TMP_InputField y;
    public TMP_InputField z;
    private FingerPose fingerPose;
    Vector3 adjustedPose;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("start");
        x.onEndEdit.AddListener(HandleInputEndEdit_x);
        y.onEndEdit.AddListener(HandleInputEndEdit_y);
        z.onEndEdit.AddListener(HandleInputEndEdit_z);
    }

    void OnEnable()
    {
        Debug.Log("OnEnable");
        fingerPose = GetComponentInParent<FingerPose>();
        x.text = fingerPose.Selector.transform.position.x.ToString();
        y.text = fingerPose.Selector.transform.position.y.ToString();
        z.text = fingerPose.Selector.transform.position.z.ToString();
        adjustedPose = fingerPose.Selector.transform.position;
    }

    void HandleInputEndEdit_x(string inputText)
    {
        adjustedPose.x = float.Parse(inputText);
        fingerPose.Selector.transform.position = adjustedPose;
    }

    void HandleInputEndEdit_y(string inputText)
    {
        adjustedPose.y = float.Parse(inputText);
        fingerPose.Selector.transform.position = adjustedPose;
    }

    void HandleInputEndEdit_z(string inputText)
    {
        adjustedPose.z = float.Parse(inputText);
        fingerPose.Selector.transform.position = adjustedPose;
    }

}
