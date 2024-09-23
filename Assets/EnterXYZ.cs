using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnterXYZ : MonoBehaviour
{
    public TMP_InputField x_position;
    public TMP_InputField y_position;
    public TMP_InputField z_position;
    private FingerPose fingerPose;
    Vector3 adjustedPose;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("start");
        x_position.onEndEdit.AddListener(HandleInputEndEdit_xPosition);
        y_position.onEndEdit.AddListener(HandleInputEndEdit_yPosition);
        z_position.onEndEdit.AddListener(HandleInputEndEdit_zPosition);
    }

    void OnEnable()
    {
        Debug.Log("OnEnable");
        fingerPose = GetComponentInParent<FingerPose>();
        x_position.text = fingerPose.Selector.transform.position.x.ToString();
        y_position.text = fingerPose.Selector.transform.position.y.ToString();
        z_position.text = fingerPose.Selector.transform.position.z.ToString();
        adjustedPose = fingerPose.Selector.transform.position;
    }

    void HandleInputEndEdit_xPosition(string inputText)
    {
        adjustedPose.x = float.Parse(inputText);
        fingerPose.Selector.transform.position = adjustedPose;
    }

    void HandleInputEndEdit_yPosition(string inputText)
    {
        adjustedPose.y = float.Parse(inputText);
        fingerPose.Selector.transform.position = adjustedPose;
    }

    void HandleInputEndEdit_zPosition(string inputText)
    {
        adjustedPose.z = float.Parse(inputText);
        fingerPose.Selector.transform.position = adjustedPose;
    }

}
