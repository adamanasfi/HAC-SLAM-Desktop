using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelManager : MonoBehaviour
{
    public static float voxelSize;
    public static GameObject voxelPrefab;
    public static GameObject chunkParent;
    public static float chunkSize;
    public static GameObject chunkPrefab;
    public GameObject VoxelPrefab;
    public GameObject ChunkPrefab;
    public GameObject ChunkParent;
    static Dictionary<Vector3, Chunk> ChunksDict;
    static List<Chunk> ActivatedChunks;
    Vector3 cameraPosition, oldCameraPosition;
    public static bool done;
    // Start is called before the first frame update
    void Start()
    {
        voxelSize = 0.1f;
        chunkSize = 3f;
        voxelPrefab = VoxelPrefab;
        chunkPrefab = ChunkPrefab;
        chunkParent = ChunkParent;
        ChunksDict = new Dictionary<Vector3, Chunk>();
        ActivatedChunks = new List<Chunk>();
        oldCameraPosition = Camera.main.transform.position;
        done = false;
    }

    private void Update()
    {
        if (done)
        {
            BuildCurrentVoxels();
            DeleteOldVoxels(); 
        }
    }

    public void DeleteOldVoxels()
    {
        cameraPosition = RoundToChunk(Camera.main.transform.position);
        List<Chunk> chunksToDeactivate = new List<Chunk>();
        foreach (Chunk chunk in ActivatedChunks)
        {
            if (Mathf.Abs((chunk.position - cameraPosition).magnitude) > 3 * chunkSize)
            {
                chunksToDeactivate.Add(chunk);
            }
        }
        foreach (Chunk chunk in chunksToDeactivate)
        {
            chunk.gameobject.SetActive(false);
            ActivatedChunks.Remove(chunk);
        }
    }

    public void BuildCurrentVoxels()
    {
        cameraPosition = RoundToChunk(Camera.main.transform.position);
        Vector3 increment = new Vector3();
        for (float i = -chunkSize; i <= chunkSize; i += chunkSize)
        {
            for (float j = 0; j <= 2 * chunkSize; j += chunkSize)
            {
                for (float k = -chunkSize; k <= chunkSize; k += chunkSize)
                {
                    increment.Set(i, j, k);
                    if (ChunksDict.ContainsKey(cameraPosition + increment))
                    {
                        Chunk chunk = ChunksDict[cameraPosition + increment];
                        if (!chunk.gameobject.activeInHierarchy)
                        {
                            chunk.gameobject.SetActive(true);
                            ActivatedChunks.Add(chunk);
                        }
                    }
                }
            }
        }
    }

    public static void AddVoxel(Vector3 point, bool state)
    {
        Vector3 voxelVector = RoundToVoxel(point);
        Vector3 chunkVector = RoundToChunk(voxelVector);
        if (!ChunksDict.ContainsKey(chunkVector)) {
            ChunksDict.Add(chunkVector, new Chunk(chunkVector,state));
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
