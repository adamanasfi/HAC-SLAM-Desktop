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
