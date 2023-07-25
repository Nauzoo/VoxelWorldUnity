using System.Collections.Generic;
using UnityEngine;

public class WorldMap : MonoBehaviour
{
    public Material worldMaterial;

    public readonly static int worldSizeInChunks = 10;
    public readonly static int worldSizeInBlocks = worldSizeInChunks * ChunkData.chunkSize;
    public readonly static int fov = 2;

    List<Vector2Int> activeChunks = new List<Vector2Int>();

    public GameObject player;
    public Vector3 spawnPoint;
    private Vector2Int playerLastChunk;

    Chunk[,] worldChunks = new Chunk[worldSizeInChunks, worldSizeInChunks];
    void Start()
    {
        spawnPoint = new Vector3(worldSizeInBlocks / 2f, ChunkData.chunkHeight, worldSizeInBlocks / 2f);
        playerLastChunk = GetChunkCoordFromVector3(spawnPoint);
        GenerateWorld();
    }

    private void Update()
    {
        if (GetChunkCoordFromVector3(player.transform.position) != playerLastChunk)
            CheckFov();
    }

    void GenerateWorld()
    {
        for (int x = worldSizeInChunks/2 - fov; x < worldSizeInChunks / 2 + fov; x++)
        {
            for (int z = worldSizeInChunks / 2 - fov; z < worldSizeInChunks / 2 + fov; z++)
            {
                CreateChunk(x, z);
            }
        }

        player.transform.position = spawnPoint;
    }

    void CheckFov()
    {
        Vector2Int chunkCoord = GetChunkCoordFromVector3(player.transform.position);

        List<Vector2Int> previousActChunks = new List<Vector2Int>(activeChunks);

        for (int x = chunkCoord.x - fov; x < chunkCoord.x + fov; x++)
        {
            for (int z = chunkCoord.y - fov; z < chunkCoord.y + fov; z++)
            {
                if (IsChunkInWorld(x, z))
                {
                    if (worldChunks[x, z] == null) CreateChunk(x, z);
                    
                    else if (!worldChunks[x, z].isActive)
                    {
                        worldChunks[x, z].isActive = true;
                        activeChunks.Add(new Vector2Int(x, z));
                    }
                }

                for (int i = 0; i < previousActChunks.Count; i++)
                {
                    if (previousActChunks[i].x == x && previousActChunks[i].y == z)
                        previousActChunks.RemoveAt(i);
                    
                }
            }
        }
        foreach (Vector2Int chunk in previousActChunks) worldChunks[chunk.x, chunk.y].isActive = false;
    }
    void CreateChunk(int x, int z)
    {        
        worldChunks[x, z] = new Chunk(this, new Vector3(x, z));
        activeChunks.Add(new Vector2Int(x, z));
    }
    public BlockTypes GetVoxel(Vector3 chunkPos, Vector3 absolutePos)
    {
        Vector3 pos = chunkPos + absolutePos;
        List<int> worldBounds = new List<int>() {worldSizeInChunks * ChunkData.chunkSize, 0 };        

        List<int> chunkBounds = new List<int>() { ChunkData.borderSize - 1, 0 };

        if (worldBounds.Contains((int)pos.x)) return BlockTypes.Barrier;
        else if (pos.y == ChunkData.chunkSize || pos.y == 0) return BlockTypes.Barrier;
        else if (worldBounds.Contains((int)pos.z)) return BlockTypes.Barrier;

        if (chunkBounds.Contains((int)chunkPos.x)) return BlockTypes.None;
        else if (chunkPos.y == ChunkData.borderHeight-1 || chunkPos.y == 0) return BlockTypes.Barrier;
        else if (chunkBounds.Contains((int)chunkPos.z)) return BlockTypes.None;

        if (pos.y < ChunkData.chunkHeight)
            return BlockTypes.Solid;
        else
            return BlockTypes.Air;
    }

    bool IsChunkInWorld(int x, int z)
    {

        if (x > 0 && x < worldSizeInChunks - 1 && z > 0 && z < worldSizeInChunks - 1)
            return true;
        else
            return false;

    }

    Vector2Int GetChunkCoordFromVector3(Vector3 pos)
    {
        int x = Mathf.FloorToInt(pos.x / ChunkData.chunkSize);
        int z = Mathf.FloorToInt(pos.z / ChunkData.chunkSize);

        return new Vector2Int(x, z);
    }
}
