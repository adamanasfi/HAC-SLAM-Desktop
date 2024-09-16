using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdjustSelectorCanvas : MonoBehaviour
{
    public TMP_InputField x_position;
    public TMP_InputField y_position;
    public TMP_InputField z_position;
    public TMP_InputField x_scale;
    public TMP_InputField y_scale;
    public TMP_InputField z_scale;
    private FingerPose fingerPose;
    Vector3 adjustedPose;
    Vector3 adjustedScale;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("start");
        x_position.onEndEdit.AddListener(HandleInputEndEdit_xPosition);
        y_position.onEndEdit.AddListener(HandleInputEndEdit_yPosition);
        z_position.onEndEdit.AddListener(HandleInputEndEdit_zPosition);
        x_scale.onEndEdit.AddListener(HandleInputEndEdit_xScale);
        y_scale.onEndEdit.AddListener(HandleInputEndEdit_yScale);
        z_scale.onEndEdit.AddListener(HandleInputEndEdit_zScale);
    }

    void OnEnable()
    {
        Debug.Log("OnEnable");
        fingerPose = GetComponentInParent<FingerPose>();
        x_position.text = fingerPose.Selector.transform.position.x.ToString();
        y_position.text = fingerPose.Selector.transform.position.y.ToString();
        z_position.text = fingerPose.Selector.transform.position.z.ToString();
        x_scale.text = fingerPose.Selector.transform.localScale.x.ToString();
        y_scale.text = fingerPose.Selector.transform.localScale.y.ToString();
        z_scale.text = fingerPose.Selector.transform.localScale.z.ToString();
        adjustedPose = fingerPose.Selector.transform.position;
        adjustedScale = fingerPose.Selector.transform.localScale;
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

    void HandleInputEndEdit_xScale(string inputText)
    {
        adjustedScale.x = float.Parse(inputText);
        fingerPose.Selector.transform.localScale = adjustedScale;
    }

    void HandleInputEndEdit_yScale(string inputText)
    {
        adjustedScale.y = float.Parse(inputText);
        fingerPose.Selector.transform.localScale = adjustedScale;
    }

    void HandleInputEndEdit_zScale(string inputText)
    {
        adjustedScale.z = float.Parse(inputText);
        fingerPose.Selector.transform.localScale = adjustedScale;
    }

}
