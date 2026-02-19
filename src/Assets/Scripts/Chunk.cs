using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Chunk
{
    private List<Vector3> landVertices = new List<Vector3>();
    private List<int> landTriangles = new List<int>();
    private List<Vector3> waterVertices = new List<Vector3>();
    private List<int> waterTriangles = new List<int>();
    private List<Vector2> UVs = new List<Vector2>();
    

    MeshRenderer landMeshRend;
    MeshRenderer waterMeshRender;
    MeshFilter landMeshFilt;
    MeshFilter waterMeshFilter;

    List<BlockTypes> notSolids = new List<BlockTypes>() { BlockTypes.Air, BlockTypes.Barrier, BlockTypes.Water };    
    private WorldMap worldMap;
    public Vector2Int coords { get; private set; }

    GameObject chunkObj;
    GameObject subChunk;

    public Chunk (WorldMap world, Vector2Int coords)
    {
        worldMap = world;
        this.coords = coords;        

        chunkObj = new GameObject();
        subChunk = new GameObject();        
        landMeshFilt = chunkObj.AddComponent<MeshFilter>();
        landMeshRend = chunkObj.AddComponent<MeshRenderer>();
        waterMeshFilter = subChunk.AddComponent<MeshFilter>();
        waterMeshRender = subChunk.AddComponent<MeshRenderer>();
        landMeshRend.material = worldMap.worldMaterial;
        waterMeshRender.material = worldMap.waterMaterial;
        chunkObj.transform.SetParent(worldMap.transform);
        subChunk.transform.SetParent(chunkObj.transform);
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
                    else if (chunkLayers[pilar, line, colon] == BlockTypes.Water)
                    {
                        if (chunkLayers[pilar + 1, line, colon] == BlockTypes.Air)
                            AddWater(new Vector3(line, pilar, colon), new Vector3[] { Faces[0], Faces[1], Faces[2], Faces[3] });
                    }

                }
            }
        }

        UpdateMesh();
        
        landVertices.Clear(); landTriangles.Clear();
    }
    void AddFace(Vector3 position, Vector3[] vertexes)
    {
        landVertices.Add(position + vertexes[0] / 2f);
        landVertices.Add(position + vertexes[1] / 2f);
        landVertices.Add(position + vertexes[2] / 2f);
        landVertices.Add(position + vertexes[3] / 2f);

        landTriangles.Add(landVertices.Count - 4); landTriangles.Add(landVertices.Count - 1);
        landTriangles.Add(landVertices.Count - 2); landTriangles.Add(landVertices.Count - 4);
        landTriangles.Add(landVertices.Count - 2); landTriangles.Add(landVertices.Count - 3);
    }
    void AddWater(Vector3 position, Vector3[] vertexes)
    {
        waterVertices.Add(position + vertexes[0] / 2f);
        waterVertices.Add(position + vertexes[1] / 2f);
        waterVertices.Add(position + vertexes[2] / 2f);
        waterVertices.Add(position + vertexes[3] / 2f);

        waterTriangles.Add(waterVertices.Count - 4); waterTriangles.Add(waterVertices.Count - 1);
        waterTriangles.Add(waterVertices.Count - 2); waterTriangles.Add(waterVertices.Count - 4);
        waterTriangles.Add(waterVertices.Count - 2); waterTriangles.Add(waterVertices.Count - 3);
    }


    private void UpdateMesh()
    {
        landMeshFilt.mesh.Clear();        

        landMeshFilt.mesh.vertices = landVertices.ToArray();
        landMeshFilt.mesh.triangles = landTriangles.ToArray();

        waterMeshFilter.mesh.Clear();
        waterMeshFilter.mesh.vertices = waterVertices.ToArray();
        waterMeshFilter.mesh.triangles = waterTriangles.ToArray();

        landMeshFilt.mesh.RecalculateNormals();
        waterMeshFilter.mesh.RecalculateNormals();
    }

    public bool isActive
    {
        get { return chunkObj.activeSelf; }
        set { chunkObj.SetActive(value); }
    }
}
