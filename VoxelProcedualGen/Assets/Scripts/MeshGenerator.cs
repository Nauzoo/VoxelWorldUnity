using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshGenerator : MonoBehaviour
{
    private List<Vector3> vertices;
    private List<int> triangles;
    
    private BlockTypes[,,] chunkData;

    public GameObject cubePrefab;

    public float seed;
    private int MaxH;
    private int MaxS;
    List<BlockTypes> notSolids = new List<BlockTypes>() { BlockTypes.Air, BlockTypes.None };
    Mesh mesh;

    public void Start()
    {
        chunkData = ChunkGenerator.GenerateChunk(seed);
        vertices = new List<Vector3>();
        triangles = new List<int>();

        MaxH = ChunkData.chunkHeight;
        MaxS = ChunkData.chunkSize;

        mesh = new Mesh();
        //StartCoroutine(CreateShape(chunkData));
        CreateMesh(chunkData);
        UpdateMesh();

        GetComponent<MeshFilter>().mesh = mesh;
        
        vertices.Clear(); triangles.Clear();
    }    

    IEnumerator CreateShape(BlockTypes[,,] chunkLayers)
    {
        
        for (int pilar = 0; pilar < MaxH; pilar++)
        {
            for (int line = 0; line < MaxS; line++)
            {
                for (int colon = 0; colon < MaxS; colon++)
                {
                    if (chunkLayers[pilar, line, colon] == BlockTypes.Solid)
                    {
                        cubePrefab.transform.position = new Vector3(line, pilar, colon);
                        Instantiate(cubePrefab);
                        yield return new WaitForSeconds(.01f);
                    }
                }                
            }
        }
    }

    void CreateMesh(BlockTypes[,,] chunkLayers)
    {
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
        mesh.Clear();        

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        
        mesh.RecalculateNormals();        
    }
}
