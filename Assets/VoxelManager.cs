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
    Vector3 cameraPosition, oldCameraPosition;
    public static bool done;
    // Start is called before the first frame update
    void Start()
    {
        voxelSize = 0.1f;
        chunkSize = 0.5f;
        voxelPrefab = VoxelPrefab;
        chunkPrefab = ChunkPrefab;
        chunkParent = ChunkParent;
        ChunksDict = new Dictionary<Vector3, Chunk>();
        oldCameraPosition = Camera.main.transform.position;
        done = false;
    }

    private void Update()
    {
        if (done)
        {
            if (CameraPositionChanged())
            {
                DeleteOldVoxels();
                BuildCurrentVoxels();
            }
        }
    }

    public bool CameraPositionChanged()
    {
        cameraPosition = RoundToChunk(Camera.main.transform.position);
        return cameraPosition != oldCameraPosition;
    }

    public void DeleteOldVoxels()
    {
        Vector3 increment = new Vector3();
        for (float i = -chunkSize; i <= chunkSize; i += chunkSize)
        {
            for (float j = 0; j <= 2 * chunkSize; j += chunkSize)
            {
                for (float k = -chunkSize; k <= chunkSize; k += chunkSize)
                {
                    increment.Set(i, j, k);
                    if (ChunksDict.ContainsKey(oldCameraPosition + increment))
                    {
                        ChunksDict[oldCameraPosition + increment].gameobject.SetActive(false);
                    }
                }
            }
        }
    }

    public void BuildCurrentVoxels()
    {
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
                        ChunksDict[cameraPosition + increment].gameobject.SetActive(true);
                    }
                }
            }
        }
        oldCameraPosition = cameraPosition;
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
