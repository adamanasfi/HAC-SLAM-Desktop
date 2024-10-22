using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelManager : MonoBehaviour
{
    public static float voxelSize;
    public static GameObject voxelPrefab;
    public static float chunkSize;
    public static GameObject chunkPrefab;
    public GameObject VoxelPrefab;
    public GameObject ChunkPrefab;
    static Dictionary<Vector3, Chunk> ChunksDict;

    // Start is called before the first frame update
    void Start()
    {
        voxelSize = 0.05f;
        chunkSize = 3f;
        voxelPrefab = VoxelPrefab;
        chunkPrefab = ChunkPrefab;
        ChunksDict = new Dictionary<Vector3, Chunk>();
    }

    public static void AddVoxel(Vector3 point)
    {
        Vector3 voxelVector = RoundToVoxel(point);
        Vector3 chunkVector = RoundToChunk(voxelVector);
        if (!ChunksDict.ContainsKey(chunkVector)) {
            ChunksDict.Add(chunkVector, new Chunk(chunkVector));
        }
        if (!ChunksDict[chunkVector].VoxelsDict.ContainsKey(voxelVector)) {
            ChunksDict[chunkVector].VoxelsDict.Add(voxelVector, new Voxel(voxelVector, ChunksDict[chunkVector].gameobject));
        }
    }

    public static void DeleteVoxel(Vector3 point)
    {
        Vector3 voxelVector = RoundToVoxel(point);
        Vector3 boxSize = new Vector3(voxelSize - 0.001f, voxelSize - 0.001f, voxelSize - 0.001f);
        if (Physics.CheckBox(voxelVector, boxSize / 2, Quaternion.identity, 1 << 3))
        {
            Vector3 chunkVector = RoundToChunk(voxelVector);
            Chunk chunk = ChunksDict[chunkVector];
            Voxel voxel = chunk.VoxelsDict[voxelVector];
            Destroy(voxel.gameobject);
            chunk.VoxelsDict.Remove(voxelVector);
        }
    }

    public static Vector3 RoundToVoxel(Vector3 point)
    {
        Vector3 roundedVector = new Vector3();
        roundedVector.Set(Mathf.RoundToInt(point.x / voxelSize) * voxelSize,
            Mathf.RoundToInt(point.y / voxelSize) * voxelSize,
            Mathf.RoundToInt(point.z / voxelSize) * voxelSize);
        return roundedVector;
    }
    
    public static Vector3 RoundToChunk(Vector3 point)
    {
        Vector3 roundedVector = new Vector3();
        roundedVector.Set(Mathf.RoundToInt(point.x / chunkSize) * chunkSize,
            Mathf.RoundToInt(point.y / chunkSize) * chunkSize,
            Mathf.RoundToInt(point.z / chunkSize) * chunkSize);
        return roundedVector;
    }
}
