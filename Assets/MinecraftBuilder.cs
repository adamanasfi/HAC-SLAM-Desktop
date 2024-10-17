using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Drawing;
using Unity.VisualScripting;
using System.Linq;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;


public class MinecraftBuilder : MonoBehaviour
{
    LayerMask voxelLayer;
    public MiniMap miniMap;
    public GameObject cube, holder, VoxelsParent, AdditonParent, DeletionParent, cube222;
    public Material[] materials;
    public RosPublisherExample pub;
    public RosSubscriberExample sub;
    public TextWriter txtwrtr;
    [NonSerialized]
    public float cubesize;
    static int Hor_angle_window = 18; //90; //36
    static int Ver_angle_window = 8; //46; //16
    static float angle_size = 2f;
    float Hor_angle_min = -((float)Hor_angle_window / 2);
    float Ver_angle_min = -((float)Ver_angle_window / 2);
    Vector3 Hor_Ray_direction;
    Vector3 Ver_Ray_direction;
    //[NonSerialized]
    public GameObject[][][] Taj;
    public GameObject parenttest;
    GameObject kube;
    int xSize;
    int ySize;
    int zSize;
    float distx_in_cm;
    float disty_in_cm;
    float distz_in_cm;
    int distx_in_cubes;
    int disty_in_cubes;
    int distz_in_cubes;
    //float min_hit;
    Vector3 nearest_pt, TransformedPoints;
    Vector3 nearest_pt2;
    Vector3[] pt = new Vector3[(int)(Hor_angle_window / angle_size)];
    float hit_distance;

    //float  for time 
    float[] grid_arr;
    int indexo;
    bool MappingSwitch;

    List<Vector3> VoxelPose;
    [NonSerialized]
    public List<Byte> VoxelByte, AddedVoxelByte, DeletedVoxelByte;
    List<float> VoxelProba;
    List<bool> VoxelExists;
    List<int> VoxelByteMap;
    Collider[] overlaps;
    Vector3 cubesizeScale;
    MeshRenderer VoxelMeshRenderer;
    //Dictionary<Vector3, float> VoxelProba;



    // Start is called before the first frame update
    void Start()
    {
        voxelLayer = LayerMask.NameToLayer("voxel");
        cubesize = cube.transform.localScale.x;
        VoxelByte = new List<Byte>();
        AddedVoxelByte = new List<Byte>();
        DeletedVoxelByte = new List<Byte>();
        cubesizeScale = new Vector3(cubesize - 0.001f, cubesize - 0.001f, cubesize - 0.001f);
    }

    public void UserVoxelAddition(Vector3 point)
    {
        distx_in_cm = Mathf.RoundToInt(point.x / cubesize) * cubesize;
        disty_in_cm = Mathf.RoundToInt(point.y / cubesize) * cubesize;
        distz_in_cm = Mathf.RoundToInt(point.z / cubesize) * cubesize;
        point = new Vector3(distx_in_cm, disty_in_cm, distz_in_cm);
        if (!Physics.CheckBox(point, cubesizeScale / 2, Quaternion.identity, 1 << voxelLayer))
        {
            kube = Instantiate(cube, point, Quaternion.identity);
            kube.transform.SetParent(AdditonParent.gameObject.transform);
            VoxelByte.AddRange(BitConverter.GetBytes(point.x));
            VoxelByte.AddRange(BitConverter.GetBytes(point.z));
            VoxelByte.AddRange(BitConverter.GetBytes(point.y));
            AddedVoxelByte.AddRange(BitConverter.GetBytes(point.x));
            AddedVoxelByte.AddRange(BitConverter.GetBytes(point.z));
            AddedVoxelByte.AddRange(BitConverter.GetBytes(point.y));
            kube.transform.SetParent(AdditonParent.gameObject.transform);
        }
    }

    public void UserVoxelDeletion(Vector3 point)
    {

        distx_in_cm = Mathf.RoundToInt(point.x / cubesize) * cubesize;
        disty_in_cm = Mathf.RoundToInt(point.y / cubesize) * cubesize;
        distz_in_cm = Mathf.RoundToInt(point.z / cubesize) * cubesize;
        point = new Vector3(distx_in_cm, disty_in_cm, distz_in_cm);
        Collider[] hitColliders = Physics.OverlapBox(point, cubesizeScale / 2, Quaternion.identity, 1 << voxelLayer);
        if (hitColliders.Length > 0) // there will be only one voxel
        {
            kube = hitColliders[0].gameObject;
            VoxelMeshRenderer = kube.GetComponent<MeshRenderer>();
            VoxelMeshRenderer.material = materials[2];
            kube.transform.SetParent(DeletionParent.gameObject.transform);
            // Destroy(hitColliders[0].gameObject);
            int index = FindVoxelIndex(point);
            VoxelByte.RemoveRange(index, 12);
            DeletedVoxelByte.AddRange(BitConverter.GetBytes(point.x));
            DeletedVoxelByte.AddRange(BitConverter.GetBytes(point.z));
            DeletedVoxelByte.AddRange(BitConverter.GetBytes(point.y));
            // kube = Instantiate(cube, point, Quaternion.identity);
        }
    }

    private int FindVoxelIndex(Vector3 position)
    {
        for (int i = 0; i <= VoxelByte.Count - 12; i += 12)
        {
            // Retrieve the x, y, z bytes from VoxelByte and convert them to floats
            float xVoxel = BitConverter.ToSingle(VoxelByte.GetRange(i, 4).ToArray(), 0);
            float yVoxel = BitConverter.ToSingle(VoxelByte.GetRange(i + 4, 4).ToArray(), 0);
            float zVoxel = BitConverter.ToSingle(VoxelByte.GetRange(i + 8, 4).ToArray(), 0);

            // Compare the floats directly
            if (xVoxel == position.x && yVoxel == position.y && zVoxel == position.z)
            {
                return i; // Return index of the byte where the voxel is found
            }
        }
        return -1; // Not found
    }



    public Vector3 TransformPCL(Vector3 Pooint)
    {

        Vector3 rotationAngles = new Vector3(-(float)sub.rx, -(float)sub.ry, -(float)sub.rz);
        Vector3 translation = new Vector3(-(float)sub.x, -(float)sub.y, -(float)sub.z);
        //Vector3 rotationAngles = new Vector3(0, 133.951803f, 0);
        //Vector3 translation = new Vector3(-2.622f, 1.4178f, -0.0768f);
        Quaternion rotationQuaternion = Quaternion.Euler(rotationAngles);
        //newPose = this.transform.position;
        Pooint = rotationQuaternion * Pooint + translation;
        return Pooint;
    }
}
