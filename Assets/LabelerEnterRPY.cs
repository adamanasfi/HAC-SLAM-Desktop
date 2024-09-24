using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LabelerEnterRPY : MonoBehaviour
{
    public TMP_InputField x_angle;
    public TMP_InputField y_angle;
    public TMP_InputField z_angle;
    private LabelerFingerPose labelerFingerPose;
    Vector3 adjustedRotation;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("start");
        x_angle.onEndEdit.AddListener(HandleInputEndEdit_xAngle);
        y_angle.onEndEdit.AddListener(HandleInputEndEdit_yAngle);
        z_angle.onEndEdit.AddListener(HandleInputEndEdit_zAngle);
    }

    void OnEnable()
    {
        Debug.Log("OnEnable");
        labelerFingerPose = GetComponentInParent<LabelerFingerPose>();
        x_angle.text = labelerFingerPose.Selector.transform.eulerAngles.x.ToString();
        y_angle.text = labelerFingerPose.Selector.transform.eulerAngles.y.ToString();
        z_angle.text = labelerFingerPose.Selector.transform.eulerAngles.z.ToString();
        adjustedRotation = labelerFingerPose.Selector.transform.eulerAngles;
    }

    void HandleInputEndEdit_xAngle(string inputText)
    {
        adjustedRotation.x = float.Parse(inputText);
        labelerFingerPose.Selector.transform.eulerAngles = adjustedRotation;
    }

    void HandleInputEndEdit_yAngle(string inputText)
    {
        adjustedRotation.y = float.Parse(inputText);
        labelerFingerPose.Selector.transform.eulerAngles = adjustedRotation;
    }

    void HandleInputEndEdit_zAngle(string inputText)
    {
        adjustedRotation.z = float.Parse(inputText);
        labelerFingerPose.Selector.transform.eulerAngles = adjustedRotation;
    }

}
