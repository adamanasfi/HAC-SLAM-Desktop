using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using UnityEngine.InputSystem;
using Microsoft.MixedReality.Toolkit.Examples.Demos;
using Unity.VisualScripting;
using System;

public class LabelerFingerPose : MonoBehaviour
{
    public GameObject BaseMenu, LabelerMenu;
    public GameObject IndicatorPrefab, Indicator;
    public GameObject EditorMenu;
    bool instantiatedIndicator;
    float zDepth;
    bool labelerOn, doneInstantiation, trackingLost, selectorInstantiated, fingersClosed, HandAngle,ToolTextBool;
    Microsoft.MixedReality.Toolkit.Utilities.MixedRealityPose poseLeft;
    Microsoft.MixedReality.Toolkit.Utilities.MixedRealityPose poseLeftIndex;
    Microsoft.MixedReality.Toolkit.Utilities.MixedRealityPose poseLeftThumb;
    float fingersThreshold, HandAngleThreshold, cubesize;
    Vector3 InitialPose, FinalPose, PrismCenter, Scale_incubes, coliderPose, cubesizeScale, ToolTipAnchor, TransformedPoints;
    Vector3Int InitialPose_incubes, FinalPose_incubes, minbound_inCubes, maxbound_inCubes;
    public MinecraftBuilder _minecraftbuilder;
    public RosPublisherExample Pub;
    public GameObject Selector, tool;
    public GameObject appBar, Prism, tooltip;
    MeshCollider _meshCollider;
    Renderer selectorMesh;
    Collider[] overlaps;
    ToolTip tooltipText; //just now
    //ToolTipConnector tooltipconnector;  //just now
    int counterForVoxels;
    public Material SelectedMaterial;
    MeshRenderer VoxelMeshRenderer;
    SystemKeyboardExample key;
    public HoloKeyboard holoKey;
    byte[] InstanceCounter = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //Should be dynamic but here it's just a proof of the concept
    public GameObject[] Buttons = new GameObject[6];
    public GameObject Parent;
    ButtonConfigHelper ButtonText;
    LabelAndInstance LabelInstance;
    byte label;
    public GameObject keyboard;

    // Start is called before the first frame update
    void Start()
    {
        instantiatedIndicator = false;
        zDepth = Camera.main.nearClipPlane;
        fingersThreshold = 0.04f;
        HandAngleThreshold = 30;
        cubesize = _minecraftbuilder.cubesize;
        cubesizeScale.Set(cubesize - 0.001f, cubesize - 0.001f, cubesize - 0.001f);
        ToolTipAnchor = Vector3.zero;
        _meshCollider = Prism.GetComponent<MeshCollider>();
        _meshCollider.convex = true;
        counterForVoxels = 0;
        labelerOn = true;
        doneInstantiation = false;
        ToolTextBool = false;
        label = 5;
        //InstanceCounter = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        //tooltipText = tooltip.GetComponent<ToolTip>();  //just now
        //tooltipconnector = tooltip.GetComponent<ToolTipConnector>();
        //Debug.Log(InstanceCounter[1]); prints 0
    }



    public void Update()
    {
        if (labelerOn)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            zDepth += scroll;
            if (!doneInstantiation)
            {
                if (selectorInstantiated)
                {
                    //Update size of selector
                    Vector3 mousePosition = Input.mousePosition;
                    mousePosition.z = zDepth;
                    Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                    FinalPose = mouseWorldPosition;
                    Indicator.transform.position = mouseWorldPosition;
                    //converting units to cubes
                    FinalPose_incubes.Set(Mathf.RoundToInt(FinalPose.x / cubesize), Mathf.RoundToInt(FinalPose.y / cubesize), Mathf.RoundToInt(FinalPose.z / cubesize));
                    PrismCenter = (InitialPose_incubes + FinalPose_incubes);

                    //without extra cubesize
                    Scale_incubes.x = Mathf.Max(Mathf.Abs((InitialPose_incubes.x - FinalPose_incubes.x) * cubesize), cubesize);
                    Scale_incubes.y = Mathf.Max(Mathf.Abs((InitialPose_incubes.y - FinalPose_incubes.y) * cubesize), cubesize);
                    Scale_incubes.z = Mathf.Max(Mathf.Abs((InitialPose_incubes.z - FinalPose_incubes.z) * cubesize), cubesize);


                    //transform selector
                    Selector.transform.position = PrismCenter * cubesize / 2;
                    Selector.transform.localScale = Scale_incubes;


                    if (!Input.GetMouseButton(0))   //successful instantiation process
                    {
                        //Set selector script
                        //selectorInstantiated = false; // this should happen in the else of doneInstantiation
                        doneInstantiation = true;
                        //selectorInstantiated = false;
                        BaseMenu.SetActive(false);
                        LabelerMenu.SetActive(true);                        
                    }


                }
                else  //here the instantation happens
                {
                    Vector3 mousePosition = Input.mousePosition;
                    mousePosition.z = zDepth;
                    Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                    if (!instantiatedIndicator)
                    {
                        Indicator = Instantiate(IndicatorPrefab, mouseWorldPosition, Quaternion.identity);
                        instantiatedIndicator = true;
                    }
                    else
                    {
                        Indicator.transform.position = mouseWorldPosition;
                    }
                    Debug.Log("Waiting For Instantiation");

                    if (Input.GetKey(KeyCode.Space) && Input.GetMouseButtonDown(0))
                    {
                        Debug.Log("INSTANTIATION HAPPENING");

                        //selector instantiation
                        InitialPose = mouseWorldPosition;
                        InitialPose_incubes.Set(Mathf.RoundToInt(InitialPose.x / cubesize), Mathf.RoundToInt(InitialPose.y / cubesize), Mathf.RoundToInt(InitialPose.z / cubesize));
                        Vector3 center = new Vector3();
                        center.Set(InitialPose_incubes.x * cubesize, InitialPose_incubes.y * cubesize, InitialPose_incubes.z * cubesize);
                        Selector = Instantiate(Prism, center, Quaternion.identity);
                        Selector.name = "Prism";
                        selectorInstantiated = true;
                    }
                }
            }
            else
            {
                selectorInstantiated = false;
                instantiatedIndicator = false;
                Destroy(Indicator);
            }

            if (ToolTextBool)
            {
                foreach (char c in Input.inputString)
                {
                    Debug.Log("TEST");
                    // Handle backspace
                    if (c == '\b' && tooltipText.ToolTipText.Length > 0)
                    {
                        // Remove the last character
                        tooltipText.ToolTipText = tooltipText.ToolTipText.Substring(0, tooltipText.ToolTipText.Length - 1);
                    }
                    // Handle enter/return key (new line)
                    else if (c == '\n' || c == '\r')
                    {
                        // Do nothing or add a new line if required
                        ToolTextBool = false;
                    }
                    // Handle regular input
                    else
                    {
                        tooltipText.ToolTipText += c; // Add the character to the current text
                    }
                }
                //////Enable when trying in editor
                //tooltipText.ToolTipText = "Akal";
            }

        }

    }

    public void labelVoxelizer(byte labely, byte instancey)
    {

        selectorMesh = Selector.GetComponent<Renderer>();

        //Rounding of the bounds to units of cubes:
        minbound_inCubes.Set(Mathf.RoundToInt(selectorMesh.bounds.min.x / cubesize),
                             Mathf.RoundToInt(selectorMesh.bounds.min.y / cubesize),
                             Mathf.RoundToInt(selectorMesh.bounds.min.z / cubesize));

        maxbound_inCubes.Set(Mathf.RoundToInt(selectorMesh.bounds.max.x / cubesize),
                             Mathf.RoundToInt(selectorMesh.bounds.max.y / cubesize),
                             Mathf.RoundToInt(selectorMesh.bounds.max.z / cubesize));


        //Loop from min to max bound:
        for (int i = minbound_inCubes.x; i <= maxbound_inCubes.x; i++)
        {
            for (int j = minbound_inCubes.y; j <= maxbound_inCubes.y; j++)
            {
                for (int k = minbound_inCubes.z; k <= maxbound_inCubes.z; k++)
                {
                    coliderPose.Set(i, j, k);
                    coliderPose = coliderPose * cubesize;

                    overlaps = Physics.OverlapBox(coliderPose, cubesizeScale / 2);
                    if (overlaps != null)
                    {
                        foreach (Collider overlap in overlaps)
                        {
                            if (overlap.gameObject.name == "Prism")
                            {
                                
                                foreach (Collider overlap2 in overlaps)
                                {
                                    if (overlap2.gameObject.name.Contains("Voxel"))
                                    {
                                        VoxelMeshRenderer = overlap2.gameObject.GetComponent<MeshRenderer>();
                                        VoxelMeshRenderer.material = SelectedMaterial;
                                        overlap2.gameObject.name = "VoxelLabeled";
                                        overlap2.transform.SetParent(Parent.transform);
                                        //ToolTipAnchor += overlap2.gameObject.transform.position;
                                        //counterForVoxels++;
                                        TransformedPoints = _minecraftbuilder.TransformPCL(overlap2.gameObject.transform.position);
                                        Pub.LabeledPointCloudPopulater(TransformedPoints, labely , instancey);
                                        //Debug.Log(labely);
                                        break;
                                        //Destroy(overlap2.gameObject);   //this works
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /////ToolTipAnchor = ToolTipAnchor / counterForVoxels; //center of mass for the selcted voxels to become the anchor for the tooltip
        //key.debugMessage.text = tooltipText.ToolTipText;
        //tooltipText.ToolTipText = "Chair";
        //tooltipconnector.Target = Selector; //The ToolTipAchor value should go here


        /*tool = Instantiate(tooltip, Selector.transform.position + new Vector3(0, (Selector.transform.localScale.y)/2,0), Quaternion.identity);
        tooltipText = tool.GetComponent<ToolTip>();*/   //Commented this and added it to the label selector

        //Destroy(Selector);
        Pub.LabelPublisher();

    }

    public void setAxes(bool state)
    {
        Selector.GetComponent<AxisDrag>().enabled = state;
        if (Selector.GetComponent<AxisDrag>().xArrow != null)
        {
            Selector.GetComponent<AxisDrag>().xArrow.SetActive(state);
            Selector.GetComponent<AxisDrag>().yArrow.SetActive(state);
            Selector.GetComponent<AxisDrag>().zArrow.SetActive(state);
        }
    }

    public void allowMotionLocalAxes(bool state)
    {
        Selector.GetComponent<AxisDrag>().allowedMotion = state;
    }

    public void setAxesGlobal(bool state)
    {
        Selector.GetComponent<AxisDragGlobal>().enabled = state;
        if (Selector.GetComponent<AxisDragGlobal>().xArrow != null)
        {
            Selector.GetComponent<AxisDragGlobal>().xArrow.SetActive(state);
            Selector.GetComponent<AxisDragGlobal>().yArrow.SetActive(state);
            Selector.GetComponent<AxisDragGlobal>().zArrow.SetActive(state);
        }

    }

    public void allowMotionGloballAxes(bool state)
    {
        Selector.GetComponent<AxisDragGlobal>().allowedMotion = state;
    }

    public void setScaleAxes(bool state)
    {
        Selector.GetComponent<ScaleWithAxes>().enabled = state;
        if (Selector.GetComponent<ScaleWithAxes>().xArrow != null)
        {
            Selector.GetComponent<ScaleWithAxes>().xArrow.SetActive(state);
            Selector.GetComponent<ScaleWithAxes>().yArrow.SetActive(state);
            Selector.GetComponent<ScaleWithAxes>().zArrow.SetActive(state);
        }
    }

    public void setCircles(bool state)
    {
        Selector.GetComponent<RotateWithCircles>().enabled = state;
        if (Selector.GetComponent<RotateWithCircles>().xCircle != null)
        {
            Selector.GetComponent<RotateWithCircles>().xCircle.SetActive(state);
            Selector.GetComponent<RotateWithCircles>().yCircle.SetActive(state);
            Selector.GetComponent<RotateWithCircles>().zCircle.SetActive(state);
        }
    }

    public void setCirclesGlobal(bool state)
    {
        Selector.GetComponent<RotateGlobal>().enabled = state;
        if (Selector.GetComponent<RotateGlobal>().xCircle != null)
        {
            Selector.GetComponent<RotateGlobal>().xCircle.SetActive(state);
            Selector.GetComponent<RotateGlobal>().yCircle.SetActive(state);
            Selector.GetComponent<RotateGlobal>().zCircle.SetActive(state);
        }
    }

    public void allowMotionGlobalCircles(bool state)
    {
        Selector.GetComponent<RotateGlobal>().allowedMotion = state;
    }



    public void deleteAxes()
    {
        Debug.Log("deleting local axes");
        if (Selector.GetComponent<AxisDrag>().xArrow != null) {
            Destroy(Selector.GetComponent<AxisDrag>().xArrow);
            Destroy(Selector.GetComponent<AxisDrag>().yArrow);
            Destroy(Selector.GetComponent<AxisDrag>().zArrow);
        }
        Selector.GetComponent<AxisDrag>().enabled = false;
    }

    public void deleteAxesGlobal()
    {
        Debug.Log("deleting global axes");
        if (Selector.GetComponent<AxisDragGlobal>().xArrow != null)
        {
            Destroy(Selector.GetComponent<AxisDragGlobal>().xArrow);
            Destroy(Selector.GetComponent<AxisDragGlobal>().yArrow);
            Destroy(Selector.GetComponent<AxisDragGlobal>().zArrow);
        }
        Selector.GetComponent<AxisDragGlobal>().enabled = false;
    }

    public void deleteScaleAxes()
    {
        Debug.Log("deleting scale axes");
        if (Selector.GetComponent<ScaleWithAxes>().xArrow != null)
        {
            Destroy(Selector.GetComponent<ScaleWithAxes>().xArrow);
            Destroy(Selector.GetComponent<ScaleWithAxes>().yArrow);
            Destroy(Selector.GetComponent<ScaleWithAxes>().zArrow);
        }
        Selector.GetComponent<ScaleWithAxes>().enabled = false;
    }

    public void deleteCircles()
    {
        Debug.Log("deleting local circles");
        if (Selector.GetComponent<RotateWithCircles>().xCircle != null){
            Destroy(Selector.GetComponent<RotateWithCircles>().xCircle);
            Destroy(Selector.GetComponent<RotateWithCircles>().yCircle);
            Destroy(Selector.GetComponent<RotateWithCircles>().zCircle);
        }
        Selector.GetComponent<RotateWithCircles>().enabled = false;
    }

    public void deleteCirclesGlobal()
    {
        Debug.Log("deleting global circles");
        if (Selector.GetComponent<RotateGlobal>().xCircle != null)
        {
            Destroy(Selector.GetComponent<RotateGlobal>().xCircle);
            Destroy(Selector.GetComponent<RotateGlobal>().yCircle);
            Destroy(Selector.GetComponent<RotateGlobal>().zCircle);
        }
        Selector.GetComponent<RotateGlobal>().enabled = false;
    }

    public void abortSelector()
    {
        Destroy(Selector);
        appBar.SetActive(false);
        doneInstantiation = false;
    }

    public void confirmSelector()

    {
        //labelVoxelizer(); commented this for testing
        //Destroy(Selector);
        //tooltipText.ToolTipText = holoKey.texty;


        /////Enable when deploying to hololens:
        ToolTextBool = false;
        Buttons[label-5].gameObject.SetActive(true);


        Buttons[label-5].GetComponent<ButtonConfigHelper>().MainLabelText = tooltipText.ToolTipText;
        
        ////comment the below for Hololens deployment
        ///Buttons[label].GetComponent<ButtonConfigHelper>().MainLabelText = "Test " + label;
        label++;
        //Debug.Log("Label: " + label);
        labelVoxelizer(label, 0);
        LabelInstance.label = label;
        LabelInstance.instance = 0;
        Destroy(Selector);
        appBar.SetActive(false);
        doneInstantiation = false;
    }

    public void adjustSelector()
    {
        Selector.GetComponent<ObjectManipulator>().enabled = true;
        //Selector.GetComponent<BoxCollider>().enabled = true;
        //Selector.GetComponent<BoundsControl>().enabled = true;
    }

    public void doneSelector()
    {
        Selector.GetComponent<ObjectManipulator>().enabled = false;
        //Selector.GetComponent<BoxCollider>().enabled = false;
        //Selector.GetComponent<BoundsControl>().enabled = false;
    }

    public void NewLabelSelector()
    {
        // TODO: pressing back and then pressing label again will create 2 tooltips
        // TODO: after pressing add new label, the abort will not delete the changes and tooltips
        tool = Instantiate(tooltip, Selector.transform.position + new Vector3(0, (Selector.transform.localScale.y) / 2, 0), Quaternion.identity);
        tooltipText = tool.GetComponent<ToolTip>();
        LabelInstance = tool.GetComponent<LabelAndInstance>();
        // labelVoxelizer();
        

        //////// Enable when deploying to HoloLens
        holoKey.OpenKeyboard();

        ToolTextBool = true;
    }

    public void AssetToolTip(Vector3 pose, string name, byte Label, byte Instance)
    {
        tool = Instantiate(tooltip, pose + new Vector3(0, 0.2f, 0), Quaternion.identity);
        tooltipText = tool.GetComponent<ToolTip>();
        LabelInstance = tool.GetComponent<LabelAndInstance>();
        LabelInstance.label = Label;
        LabelInstance.instance = Instance;
        tooltipText.ToolTipText = name;
    }

    public void PreviouslyLabeled(int i)
    {
        byte b = (byte)i;
        //Debug.Log("hay lbyte: " + b);
        tool = Instantiate(tooltip, Selector.transform.position + new Vector3(0, (Selector.transform.localScale.y) / 2, 0), Quaternion.identity);
        tooltipText = tool.GetComponent<ToolTip>();
        LabelInstance = tool.GetComponent<LabelAndInstance>();
        ButtonText = Buttons[b-5].GetComponent<ButtonConfigHelper>();
        tooltipText.ToolTipText = ButtonText.MainLabelText;
        InstanceCounter[b]++;
        LabelInstance.label = b;
        LabelInstance.instance = InstanceCounter[b];
        
        b++;

        labelVoxelizer(b, InstanceCounter[b-1]);
        //Debug.Log("Insta: " + InstanceCounter[b]);
        Destroy(Selector);
        appBar.SetActive(false);
        doneInstantiation = false;

    }

    public byte AssetInstance(byte labelo)
    {
        
        labelo = (byte) (labelo - 1);
        
        byte CurrentInstance = InstanceCounter[labelo];
        //Debug.Log(InstanceCounter[labelo]);
        InstanceCounter[labelo]++;
        return CurrentInstance;
        
    }

}
