using System.Collections.Generic;
using UnityEngine;

public class WorldMap : MonoBehaviour
{
    public Material worldMaterial;
    public Material waterMaterial;
    public int oceanHight;

    public readonly static int worldSizeInChunks = 100;
    public readonly static int worldSizeInBlocks = worldSizeInChunks * ChunkData.chunkSize;
    List<float> worldBounds = new List<float>() { (worldSizeInChunks * ChunkData.chunkSize), 0f };
    public readonly static int fov = 20;

    List<Vector2Int> activeChunks = new List<Vector2Int>();

    public GameObject player;
    public Vector3 spawnPoint;
    private Vector2Int playerLastChunk;

    Chunk[,] worldChunks = new Chunk[worldSizeInChunks, worldSizeInChunks];
    float[,] noisemap;

    public int SEED;
    public int octaves;
    public int noiseScale;
    public float lacunarity;
    public float persistence;
    public Vector2 offset;

    void Start()
    {       
        Random.InitState(SEED);

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
        noisemap = Noise.GenerateNoiseMap(worldSizeInBlocks, SEED, noiseScale, octaves, lacunarity, persistence, offset);
        //noisemap = RandomNoise.GenerateRandomMap(worldSizeInBlocks);
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
        worldChunks[x, z] = new Chunk(this, new Vector2Int(x, z));
        activeChunks.Add(new Vector2Int(x, z));
    }

    public BlockTypes GetVoxel(Vector3Int chunkPos, Vector3 absolutePos)
    {
        Vector3 pos = chunkPos + absolutePos;        

        if (!IsVoxelInWorld(pos))
            return BlockTypes.Barrier;


        //int terrainHight = Mathf.FloorToInt(ChunkData.chunkHeight * GetPerlianNoise2D(new Vector2(pos.x, pos.z), 250, 0.3f));
        //int terrainHight = Mathf.FloorToInt(Mathf.Sin(pos.x * 0.2f) * 25) + 50;
        //int terrainHight = Mathf.FloorToInt(((Mathf.Sin(pos.x * 0.2f) * 25)) + ((Mathf.Sin(pos.z * 0.2f) * 25) + 20));
        int terrainHight = Mathf.FloorToInt(noisemap[(int)pos.x, (int)pos.z] * ChunkData.chunkHeight);

        if (pos.y > terrainHight)
            if (pos.y <= oceanHight && IsVoxelInChunk(chunkPos)) return BlockTypes.Water;
            else return BlockTypes.Air;

        if (IsVoxelInChunk(chunkPos) && pos.y <= terrainHight)
            return BlockTypes.Solid;
        
        else
            return BlockTypes.None;

    }

    bool IsChunkInWorld(int x, int z)
    {

        if (x > 0 && x < worldSizeInChunks - 1 && z > 0 && z < worldSizeInChunks - 1)
            return true;
        else
            return false;

    }
    bool IsVoxelInWorld(Vector3 posInWorld)
    {
        if (worldBounds.Contains(posInWorld.x)) return false;
        else if (posInWorld.y == ChunkData.chunkSize || posInWorld.y == 0) return false;
        else if (worldBounds.Contains(posInWorld.z)) return false;

        else return true;
    }
    bool IsVoxelInChunk(Vector3Int posInChunk)
    {
        if (ChunkData.chunkBounds.Contains(posInChunk.x)) return false;
        else if (posInChunk.y == ChunkData.borderHeight - 1 || posInChunk.y == 0) return false;
        else if (ChunkData.chunkBounds.Contains(posInChunk.z)) return false;

        else return true;
    }

    Vector2Int GetChunkCoordFromVector3(Vector3 pos)
    {
        int x = Mathf.FloorToInt(pos.x / ChunkData.chunkSize);
        int z = Mathf.FloorToInt(pos.z / ChunkData.chunkSize);

        return new Vector2Int(x, z);
    }

    public float GetPerlianNoise2D(Vector2 pos, int offset, float scale)
    {
        return Mathf.PerlinNoise(pos.x / ChunkData.chunkSize * scale + offset, pos.y / ChunkData.chunkSize * scale + offset);
    }
}
