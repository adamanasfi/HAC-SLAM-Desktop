using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LabelerEnterXYZ : MonoBehaviour
{
    public TMP_InputField x_position;
    public TMP_InputField y_position;
    public TMP_InputField z_position;
    private LabelerFingerPose labelerFingerPose;
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
        labelerFingerPose = GetComponentInParent<LabelerFingerPose>();
        x_position.text = labelerFingerPose.Selector.transform.position.x.ToString();
        y_position.text = labelerFingerPose.Selector.transform.position.y.ToString();
        z_position.text = labelerFingerPose.Selector.transform.position.z.ToString();
        adjustedPose = labelerFingerPose.Selector.transform.position;
    }

    void HandleInputEndEdit_xPosition(string inputText)
    {
        adjustedPose.x = float.Parse(inputText);
        labelerFingerPose.Selector.transform.position = adjustedPose;
    }

    void HandleInputEndEdit_yPosition(string inputText)
    {
        adjustedPose.y = float.Parse(inputText);
        labelerFingerPose.Selector.transform.position = adjustedPose;
    }

    void HandleInputEndEdit_zPosition(string inputText)
    {
        adjustedPose.z = float.Parse(inputText);
        labelerFingerPose.Selector.transform.position = adjustedPose;
    }

}
