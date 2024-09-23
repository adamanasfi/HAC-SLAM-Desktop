using UnityEngine;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine.UI;
using System;
using TMPro;

public class FingerPose : MonoBehaviour
{
    bool AddedAsset;
    public GameObject IndicatorPrefab, Indicator;
    public GameObject AddVoxelsMenu, DeleteVoxelsMenu, AddAssetsMenu, AdjustConfirmAbortMenu;
    bool instantiatedIndicator;
    float zDepth;
    Vector3 InitialPose, FinalPose, PrismCenter, Scale_incubes, AssetPose, AssetRot;
    Vector3Int InitialPose_incubes, FinalPose_incubes;
    GameObject Prism, ModelTarget;
    public GameObject[] Selectors = new GameObject[8];
    public GameObject[] VuforiaTargets = new GameObject[6];
    public LabelerFingerPose Labeler;
    public MinecraftBuilder _MinecraftBuilder;
    public RosPublisherExample _RosPublisher;
    Microsoft.MixedReality.Toolkit.Utilities.MixedRealityPose poseLeft;
    Microsoft.MixedReality.Toolkit.Utilities.MixedRealityPose poseLeftIndex; //new
    Microsoft.MixedReality.Toolkit.Utilities.MixedRealityPose poseLeftThumb; //new
    IMixedRealityHandJointService handJointService;
    float cubesize, HandAngleThreshold;
    float fingersThreshold = 0.04f;
    public GameObject Selector;
    bool EditorActivator, EditorActivatorOld, selectorInstantiated, trackingLost, fingersClosed, doneInstantiation, testingBool, ConvexityState, DeletingVoxels, AddingAssets, VuforiaEnabled, VuforiaFound, HandAngle;

    Renderer selectorMesh;
    Vector3 minbound, maxbound; //delete 

    Vector3Int minbound_inCubes, maxbound_inCubes;
    Vector3 coliderPose, cubesizeScale;
    Collider[] overlaps;
    public GameObject appBar;
    byte AssetLabel, AssetInstance;
 
    //MixedRealityInputAction selectAction;
    bool EnablePrism;
    MeshCollider _meshCollider;
    InputActionHandler _inputActionHandler;
    string AssetName;
    
    private void Start()
    {
        AddedAsset = false;
        instantiatedIndicator = false;
        zDepth = Camera.main.nearClipPlane;
        handJointService = CoreServices.GetInputSystemDataProvider<IMixedRealityHandJointService>();
        cubesize = _MinecraftBuilder.cubesize;
        HandAngleThreshold = 30;
        EnablePrism = false;  //enabled when the user gestures a pinch
        EditorActivator = false; //enabled from the 'Edit Voxels' button
        EditorActivatorOld = false;
        doneInstantiation = false;
        testingBool = true;
        DeletingVoxels = false;
        AddingAssets = false;
        cubesizeScale.Set(cubesize, cubesize, cubesize);
        fingersThreshold = 0.04f;
        Prism = Selectors[3];
        _meshCollider = Prism.GetComponent<MeshCollider>();
        _inputActionHandler = gameObject.GetComponent<InputActionHandler>();
        
        Debug.Log("start");

        //_meshCollider.convex = true;  // We need to make this as a kabse later.
    }



    public void Update()
    {
        if (EditorActivator)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            zDepth += scroll;
            if (!doneInstantiation)
            {
                if (selectorInstantiated)
                {
                    Debug.Log("DRAWING CUBE");
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
                        if (DeletingVoxels) DeleteVoxelsMenu.SetActive(false);
                        else AddVoxelsMenu.SetActive(false);
                        AdjustConfirmAbortMenu.SetActive(true);
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
            
        }

        if (VuforiaFound)
        {
            Selector.transform.position = ModelTarget.transform.position;
            Selector.transform.rotation = ModelTarget.transform.rotation;
        }

        else if (AddingAssets)
        {
            if (!AddedAsset)
            {
                float scroll = Input.GetAxis("Mouse ScrollWheel");
                zDepth += scroll;
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
                if (Input.GetKey(KeyCode.Space) && Input.GetMouseButtonDown(0))
                {
                    AssetPose = mouseWorldPosition;
                    // AssetRot.Set(0, Camera.main.transform.localRotation.eulerAngles.y, 0);
                    Selector = Instantiate(Prism, AssetPose, Quaternion.identity);
                    Selector.name = "Prism";
                    instantiatedIndicator = false;
                    Destroy(Indicator);
                    AddAssetsMenu.SetActive(false);
                    AdjustConfirmAbortMenu.SetActive(true);
                    AddedAsset = true;

                }
            }
        }


        /////// here is the old part
        /*if (EnablePrism == true && HandJointUtils.TryGetJointPose(Microsoft.MixedReality.Toolkit.Utilities.TrackedHandJoint.IndexTip, Microsoft.MixedReality.Toolkit.Utilities.Handedness.Right, out poseLeft))
        {

            FinalPose = poseLeft.Position;
            
            //Converting units to cubes:
            FinalPose_incubes.Set(Mathf.RoundToInt(FinalPose.x / cubesize), Mathf.RoundToInt(FinalPose.y / cubesize), Mathf.RoundToInt(FinalPose.z / cubesize));
            PrismCenter = (InitialPose_incubes + FinalPose_incubes);
            Scale_incubes.x = Mathf.Max(Mathf.Abs((InitialPose_incubes.x - FinalPose_incubes.x) * cubesize) + cubesize, cubesize);
            Scale_incubes.y = Mathf.Max(Mathf.Abs((InitialPose_incubes.y - FinalPose_incubes.y) * cubesize) + cubesize, cubesize);
            Scale_incubes.z = Mathf.Max(Mathf.Abs((InitialPose_incubes.z - FinalPose_incubes.z) * cubesize) + cubesize, cubesize);

            Selector.transform.position = PrismCenter * cubesize/2;
            Selector.transform.localScale = Scale_incubes;
            
        }*/
    }


    public void editor3D()  //instantiator
    {
        if (EditorActivatorOld)
        {
            if (HandJointUtils.TryGetJointPose(Microsoft.MixedReality.Toolkit.Utilities.TrackedHandJoint.IndexTip, Microsoft.MixedReality.Toolkit.Utilities.Handedness.Right, out poseLeft))
            {
                EnablePrism = !EnablePrism;
                InitialPose = poseLeft.Position;
                InitialPose_incubes.Set(Mathf.RoundToInt(InitialPose.x / cubesize), Mathf.RoundToInt(InitialPose.y / cubesize), Mathf.RoundToInt(InitialPose.z / cubesize));

                if (EnablePrism == true)
                {
                    Selector = Instantiate(Prism, InitialPose_incubes, Quaternion.identity);
                    Selector.name = "Prism";
                }
            }
        }
        
}

    public void CubeAdder()
    {
        if (EditorActivatorOld)
        {
            _MinecraftBuilder.InstantiateEditor(InitialPose_incubes, FinalPose_incubes);
            Destroy(Selector);
        }
        
    }

    public void CubeRemover()
    {
        if (EditorActivatorOld)
        {
            _MinecraftBuilder.DestroyEditor(InitialPose_incubes, FinalPose_incubes);
            Destroy(Selector);
        }
        
    }
   
    public void ActivateEditor(bool state)
    {
        EditorActivator = state;
    }

    public void vertexExtractor()
    {
        if (testingBool)
        {
            selectorMesh = Selector.GetComponent<Renderer>();
            minbound = selectorMesh.bounds.min;
            maxbound = selectorMesh.bounds.max;
            //Instantiate(sfiro, minbound, Quaternion.identity); //watch out for the position it should be transformed
            //Instantiate(sfiro, maxbound, Quaternion.identity);
        }   
    }
    
    public void officialVoxelizer()
    {
        Debug.Log("ENTERED VOXELIZER");
        selectorMesh = Selector.GetComponent<Renderer>();

        //Rounding of the bounds to units of cubes:
        minbound_inCubes.Set(Mathf.RoundToInt(selectorMesh.bounds.min.x / cubesize), 
                             Mathf.RoundToInt(selectorMesh.bounds.min.y / cubesize), 
                             Mathf.RoundToInt(selectorMesh.bounds.min.z / cubesize));
        
        maxbound_inCubes.Set(Mathf.RoundToInt(selectorMesh.bounds.max.x / cubesize),
                             Mathf.RoundToInt(selectorMesh.bounds.max.y / cubesize),
                             Mathf.RoundToInt(selectorMesh.bounds.max.z / cubesize));


        //Loop from min to max bound:
        for(int i = minbound_inCubes.x; i <= maxbound_inCubes.x; i++)
        {
            //test
            for (int j = minbound_inCubes.y; j <= maxbound_inCubes.y; j++)
            {
                for (int k = minbound_inCubes.z; k <= maxbound_inCubes.z; k++)
                {
                    coliderPose.Set(i, j, k);
                    coliderPose = coliderPose * cubesize;

                    overlaps = Physics.OverlapBox(coliderPose, cubesizeScale / 2);
                    if (overlaps != null)
                    {
                        foreach(Collider overlap in overlaps)
                        {
                            if(overlap.gameObject.name == "Prism")
                            {
                                //coliderPose = coliderPose / cubesize;
                                //coliderPose = coliderPose * 0.0499f;
                                //_MinecraftBuilder.Instantiator(coliderPose, true);
                                if (AddingAssets || VuforiaEnabled)
                                {
                                    _MinecraftBuilder.UserAssetAddition(coliderPose);
                                    _RosPublisher.LabeledPointCloudPopulater(coliderPose, AssetLabel, AssetInstance);
                                }
                                else if (DeletingVoxels)
                                {
                                    _MinecraftBuilder.UserVoxelDeletion(coliderPose);
                                }
                                else
                                {
                                    _MinecraftBuilder.UserVoxelAddition(coliderPose);
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }


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

    public void deleteAxes()
    {
        Debug.Log("deleting local axes");
        if (Selector.GetComponent<AxisDrag>().xArrow != null)
        {
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

    public void deleteCircles()
    {
        Debug.Log("deleting local circles");
        if (Selector.GetComponent<RotateWithCircles>().xCircle != null)
        {
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
        if (Selector != null) Destroy(Selector);
        appBar.SetActive(false);
        doneInstantiation = false;
        if (AddingAssets)
        {
            AddAssetsMenu.SetActive(true);
            AddedAsset = false;
            _inputActionHandler.enabled = true;
        }
        else if (VuforiaEnabled)
        {
            VuforiaFound = false;
            ModelTarget.SetActive(false);
        }
        else if (DeletingVoxels)
        {
            DeleteVoxelsMenu.SetActive(true);
        }
        else // Adding Voxels
        {
            AddVoxelsMenu.SetActive(true);
        }
    }

    public void confirmSelector()
    {
        if (AddingAssets)
        {
            AddAssetsMenu.SetActive(true);
            // _inputActionHandler.enabled = true;      NO NEED ON DESKTOP
            AssetInstance = Labeler.AssetInstance(AssetLabel);
            Labeler.AssetToolTip(Selector.transform.position, AssetName, AssetLabel, AssetInstance);
            _MinecraftBuilder.AddedVoxelByte.Clear();
            Debug.Log("NOW VOXELIZE ASSET!");
            officialVoxelizer();
            AddedAsset = false;
            _RosPublisher.PublishEditedPointCloudMsg();
            _RosPublisher.LabelPublisher();
        }

        else if (DeletingVoxels)
        {
            DeleteVoxelsMenu.SetActive(true);
            _MinecraftBuilder.DeletedVoxelByte.Clear();
            officialVoxelizer();
            _RosPublisher.PublishDeletedVoxels();
            doneInstantiation = false;
        }


        else if (VuforiaEnabled)
        {
            AssetInstance = Labeler.AssetInstance(AssetLabel);
            Labeler.AssetToolTip(Selector.transform.position, AssetName, AssetLabel, AssetInstance);
            _MinecraftBuilder.AddedVoxelByte.Clear();
            officialVoxelizer();
            _RosPublisher.PublishEditedPointCloudMsg();
            _RosPublisher.LabelPublisher();
            VuforiaFound = false;
            ModelTarget.SetActive(false);

        }
        else
        {
            AddVoxelsMenu.SetActive(true);
            _MinecraftBuilder.AddedVoxelByte.Clear();
            officialVoxelizer();
            _RosPublisher.PublishEditedPointCloudMsg();
            doneInstantiation = false;
        }
        
        Destroy(Selector);
        appBar.SetActive(false);
        
    }

    public void adjustSelector()
    {
        //Selector.GetComponent<BoxCollider>().enabled = true;
        //Selector.GetComponent<BoundsControl>().enabled = true;
        Selector.GetComponent<ObjectManipulator>().enabled = true;
    }

    public void doneSelector()
    {
        //Selector.GetComponent<BoxCollider>().enabled = false;
        //Selector.GetComponent<BoundsControl>().enabled = false;
        Selector.GetComponent<ObjectManipulator>().enabled = false;
    }

    public void requestSelectorShape(int index)
    {
        Debug.Log("CHANGING SHAPE");
        Prism = Selectors[index];
        _meshCollider = Selectors[index].GetComponent<MeshCollider>();
        _meshCollider.convex = ConvexityState;
    }

    public void Convexity(bool state)
    {
        _meshCollider.convex = state;
        ConvexityState = state;
    }

    public void AssetInstantiator()
    {
        if (Selector != null) Destroy(Selector);
        AssetPose.x = Camera.main.transform.localPosition.x + 2 * Mathf.Sin(Camera.main.transform.localRotation.eulerAngles.y * Mathf.Deg2Rad);
        AssetPose.y = Camera.main.transform.localPosition.y - 1.5f;
        AssetPose.z = Camera.main.transform.localPosition.z + 2 * Mathf.Cos(Camera.main.transform.localRotation.eulerAngles.y * Mathf.Deg2Rad);
        
        AssetRot.Set(0, Camera.main.transform.localRotation.eulerAngles.y, 0);
        
        Selector = Instantiate(Prism, AssetPose, Quaternion.Euler(AssetRot));
        Selector.name = "Prism";
        appBar.SetActive(true);
        _inputActionHandler.enabled = false;
    }

    public void EnableAssetAddition(bool state)
    {
        AddingAssets = state;
       // _inputActionHandler.enabled = state;          NO NEED ON DESKTOP APPLICATION!
    }

    public void AssetLabelNumber(int label)
    {
        AssetLabel = (byte)label;
    }

    public void AssetNameForTooltip(string name)
    {
        AssetName = name;
    }

    public void EnableDeletion(bool state)
    {
        DeletingVoxels = state;
    }

    public void EnableVuforia(bool state)
    {
        VuforiaEnabled = state;

        if (!state)
        {
            VuforiaFound = false;
            ModelTarget.SetActive(false);
        }
    }

    public void VuforiaTargetRequest(int index)
    {
        VuforiaTargets[index].SetActive(true);
        ModelTarget = VuforiaTargets[index];
    }

    public void ModelTracked(GameObject Object)
    {
        if (!VuforiaFound)
        {
            Prism = Object;
            Selector = Instantiate(Prism, ModelTarget.transform.position, ModelTarget.transform.rotation);
            Selector.name = "Prism";
            appBar.SetActive(true);
            VuforiaFound = true;
        }

    }

}