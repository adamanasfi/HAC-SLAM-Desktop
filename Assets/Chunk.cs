using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk 
{
    public GameObject gameobject;
    public Vector3 position;
    public Dictionary<Vector3, Voxel> VoxelsDict;

    public Chunk(Vector3 chunkPosition, bool state) 
    {
        position = chunkPosition;
        gameobject = UnityEngine.Object.Instantiate(VoxelManager.chunkPrefab, Vector3.zero, Quaternion.identity);
        gameobject.SetActive(state);
        VoxelsDict = new Dictionary<Vector3, Voxel>();
    }
}
