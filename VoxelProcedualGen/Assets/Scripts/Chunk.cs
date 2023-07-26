using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Chunk
{
    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();

    MeshRenderer meshRend;
    MeshFilter meshFilt;

    List<BlockTypes> notSolids = new List<BlockTypes>() { BlockTypes.Air, BlockTypes.Barrier };    
    private WorldMap worldMap;
    public Vector2Int coords { get; private set; }

    GameObject chunkObj;

    public Chunk (WorldMap world, Vector2Int coords)
    {
        worldMap = world;
        this.coords = coords;

        chunkObj = new GameObject();
        meshFilt = chunkObj.AddComponent<MeshFilter>();
        meshRend = chunkObj.AddComponent<MeshRenderer>();
        meshRend.material = worldMap.worldMaterial;
        chunkObj.transform.SetParent(worldMap.transform);
        chunkObj.transform.position = new Vector3(this.coords.x * ChunkData.chunkSize, 0f, this.coords.y * ChunkData.chunkSize);        
        chunkObj.name = $"chunk p({this.coords.x}, {this.coords.y})";

        CreateMesh(GenerateChunk());
    }
    public BlockTypes[,,] GenerateChunk()
    {
        int MaxH = ChunkData.borderHeight;
        int MaxS = ChunkData.borderSize;

        
        BlockTypes[,,] chunkLayers = new BlockTypes[MaxH, MaxS, MaxS];

        for (int pilar = 0; pilar < MaxH; pilar++)
        {
            for (int line = 0; line < MaxS; line++)
            {
                for (int colon = 0; colon < MaxS; colon++)
                {
                    chunkLayers[pilar, line, colon] = worldMap.GetVoxel(new Vector3Int(line, pilar, colon), chunkObj.transform.position);
                }
            }
        }

        return chunkLayers;
    }

    void CreateMesh(BlockTypes[,,] chunkLayers) {

        int MaxH = ChunkData.borderHeight;
        int MaxS = ChunkData.borderSize;

        Vector3[] Faces = new Vector3[]  
        {
            new Vector3(-1, 1, -1), new Vector3(1, 1, -1),  new Vector3(1, 1, 1), new Vector3(-1, 1, 1),    //TOP FACE
            new Vector3(-1, -1, 1), new Vector3(1, -1, 1), new Vector3(1, -1, -1), new Vector3(-1, -1, -1), //BOTTOM FACE
            new Vector3(1, -1, 1), new Vector3(1, 1, 1), new Vector3(1, 1, -1),  new Vector3(1, -1, -1),    // RIGHT FACE
            new Vector3(-1, 1, 1), new Vector3(-1, -1, 1), new Vector3(-1, -1, -1), new Vector3(-1, 1, -1), //LEFT FACE
            new Vector3(1, -1, 1), new Vector3(-1, -1, 1), new Vector3(-1, 1, 1), new Vector3(1, 1, 1),     // FRONT FACE
            new Vector3(-1, -1, -1), new Vector3(1, -1, -1), new Vector3(1, 1, -1), new Vector3(-1, 1, -1)  // BACK FACE
        };

        int[,] directions =
                        { { 1, 0, 0 },
                          { -1, 0, 0},
                          { 0, 1, 0 },
                          { 0, -1, 0},
                          { 0, 0, 1 },
                          { 0, 0, -1}
                        };

        for (int pilar = 0; pilar < MaxH; pilar++)
        {
            for (int line = 0; line < MaxS; line++)
            {
                for (int colon = 0; colon < MaxS; colon++)
                {
                    if (chunkLayers[pilar, line, colon] == BlockTypes.Solid)
                    {
                        for (int dir = 0; dir < 6; dir++)
                        {                            
                            if (notSolids.Contains(chunkLayers[pilar + directions[dir, 0], line + directions[dir, 1], colon + directions[dir, 2]]))
                                AddFace(new Vector3(line, pilar, colon), new Vector3[] { Faces[dir * 4], Faces[dir * 4 + 1], Faces[dir * 4 + 2], Faces[dir * 4 + 3] });                            
                        }
                    }
                }
            }
        }

        UpdateMesh();
        
        vertices.Clear(); triangles.Clear();
    }
    void AddFace(Vector3 position, Vector3[] vertexes)
    {
        vertices.Add(position + vertexes[0] /2f);
        vertices.Add(position + vertexes[1] /2f);
        vertices.Add(position + vertexes[2] /2f);
        vertices.Add(position + vertexes[3] /2f);

        triangles.Add(vertices.Count - 4); triangles.Add(vertices.Count -1);
        triangles.Add(vertices.Count - 2); triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 2); triangles.Add(vertices.Count - 3);
    }

    private void UpdateMesh()
    {
        meshFilt.mesh.Clear();        

        meshFilt.mesh.vertices = vertices.ToArray();
        meshFilt.mesh.triangles = triangles.ToArray();

        meshFilt.mesh.RecalculateNormals();        
    }

    public bool isActive
    {
        get { return chunkObj.activeSelf; }
        set { chunkObj.SetActive(value); }
    }
}
