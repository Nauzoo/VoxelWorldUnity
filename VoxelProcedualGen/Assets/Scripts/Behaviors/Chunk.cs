using System;
using UnityEngine;

public static class Chunk
{
    public static void CheckAllVoxels(ChunkData chunkData, Action<int, int, int> perfrom)
    {
        for (int i = 0; i < chunkData.voxelList.Length; i++) {
            Vector3Int position = getPosOfIndex(chunkData, i);
            perfrom(position.x, position.y, position.z);
        }
    }

    private static Vector3Int getPosOfIndex(ChunkData chunkData, int i)
    {
        int x = i % chunkData.size;
        int y = (i / chunkData.size) % chunkData.height;
        int z = i / (chunkData.size * chunkData.height);
        return new Vector3Int(x, y, z);
    }
    private static bool InHRange(ChunkData chunkData, int hCoord)
    {
        if (hCoord < 0 || hCoord >= chunkData.size)
            return false;
        else
            return true;
    }
    private static bool InVRange(ChunkData chunkData, int vCoord)
    {
        if (vCoord < 0 || vCoord >= chunkData.height)
            return false;
        else
            return true;
    }
    public static VoxelConf GetVoxelFromChunkCoords(ChunkData chunkData, int x, int y, int z)
    {
        if (InHRange(chunkData, x) && InHRange(chunkData, z) && InVRange(chunkData, y))
        {
            int i = GetVoxelIndex(chunkData, x, y, z);
            return chunkData.voxelList[i];
        }
        throw new Exception("Incorrect chunk asked");
    }
    public static VoxelConf GetVoxelFromChunkCoords(ChunkData chunkData, Vector3Int chunkCoords)
    {
        return GetVoxelFromChunkCoords(chunkData, chunkCoords.x, chunkCoords.y, chunkCoords.z);
    }
    public static void SetBlock(ChunkData chunkData, Vector3Int pos, VoxelConf block)
    {
        if (InHRange(chunkData, pos.x) && InHRange(chunkData, pos.z) && InVRange(chunkData, pos.y))
        {
            int i = GetVoxelIndex(chunkData, pos.x, pos.y, pos.z);
            chunkData.voxelList[i] = block;
        }
    }

    private static int GetVoxelIndex(ChunkData chunkData, int x, int y, int z)
    {
        return x + chunkData.size * y + chunkData.size * chunkData.height * z;
    }
    public static Vector3Int GetVoxelInChunkCoords(ChunkData chunkData, Vector3Int pos)
    {
        return new Vector3Int { 
            x = pos.x - chunkData.mapPos.x,
            y = pos.y - chunkData.mapPos.y,
            z = pos.z - chunkData.mapPos.z
        };
    }
    public static MeshData GetChunkMesh(ChunkData chunkData)
    {
        MeshData meshData = new MeshData(true);

        return meshData;
    }
}