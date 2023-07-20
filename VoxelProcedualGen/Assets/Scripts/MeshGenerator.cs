using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    [SerializeField] private List<Vector3> vertices;
    [SerializeField] private List<int> triangles;
    private BlockTypes[,,] chunkData;

    public GameObject cubePrefab;

    public float seed = 18;
    Mesh mesh;

    public void Start()
    {
        chunkData = ChunkGenerator.GenerateChunk(seed);
        //cubePrefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
        mesh = new Mesh();
        //StartCoroutine(CreateShape(chunkData));
        CreateMesh(chunkData);
        UpdateMesh();

        GetComponent<MeshFilter>().mesh = mesh;
    }    

    IEnumerator CreateShape(BlockTypes[,,] chunkLayers)
    {
        
        for (int pilar = 0; pilar < ChunkData.chunkHeight; pilar++)
        {
            for (int line = 0; line < ChunkData.chunkSize; line++)
            {
                for (int colon = 0; colon < ChunkData.chunkSize; colon++)
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

    private Vector3 vertice0 = new Vector3(-1, -1, -1);
    private Vector3 vertice1 = new Vector3(1, -1, -1);
    private Vector3 vertice2 = new Vector3(1, -1, 1);
    private Vector3 vertice3 = new Vector3(-1, -1, 1);
    private Vector3 vertice4 = new Vector3(-1, 1, -1);
    private Vector3 vertice5 = new Vector3(1, 1, -1);
    private Vector3 vertice6 = new Vector3(1, 1, 1);
    private Vector3 vertice7 = new Vector3(-1, 1, 1);
    void CreateMesh(BlockTypes[,,] chunkLayers)
    {
        for (int pilar = 0; pilar < ChunkData.chunkHeight; pilar++)
        {
            for (int line = 0; line < ChunkData.chunkSize; line++)
            {
                for (int colon = 0; colon < ChunkData.chunkSize; colon++)
                {
                    if (chunkLayers[pilar, line, colon] == BlockTypes.Solid)
                    {
                        if (pilar < ChunkData.chunkHeight - 1) // +Y CHECK
                        {
                            if (chunkLayers[pilar + 1, line, colon] == BlockTypes.Air)
                                AddFace(new Vector3(line, pilar, colon), new Vector3[] { 
                                    vertice4, vertice5, vertice6, vertice7
                                });
                        } else AddFace(new Vector3(line, pilar, colon), new Vector3[] { vertice4, vertice5, vertice6, vertice7 });

                        if (pilar > 0) // -Y CHECK
                        {
                            if (chunkLayers[pilar - 1, line, colon] == BlockTypes.Air)
                                AddFace(new Vector3(line, pilar, colon), new Vector3[] {
                                    vertice3, vertice2, vertice1, vertice0
                                });
                        }  else AddFace(new Vector3(line, pilar, colon), new Vector3[] { vertice3, vertice2, vertice1, vertice0 });

                        if (line < ChunkData.chunkSize - 1) // +X CHECK
                        {
                            if (chunkLayers[pilar, line + 1, colon] == BlockTypes.Air)
                                AddFace(new Vector3(line, pilar, colon), new Vector3[] {
                                    vertice2, vertice6, vertice5, vertice1
                                });
                        }
                        else AddFace(new Vector3(line, pilar, colon), new Vector3[] { vertice2, vertice6, vertice5, vertice1 });
                        
                        if (line > 0) // -X CHECK
                        {
                            if (chunkLayers[pilar, line - 1, colon] == BlockTypes.Air)
                                AddFace(new Vector3(line, pilar, colon), new Vector3[] {
                                    vertice7, vertice3, vertice0, vertice4
                                });
                        }
                        else AddFace(new Vector3(line, pilar, colon), new Vector3[] { vertice7, vertice3, vertice0, vertice4 });


                        if (colon < ChunkData.chunkSize - 1) // +Z CHECK
                        {
                            if (chunkLayers[pilar, line, colon + 1] == BlockTypes.Air)
                                AddFace(new Vector3(line, pilar, colon), new Vector3[] {
                                    vertice2, vertice3, vertice7, vertice6
                                });
                        }
                        else AddFace(new Vector3(line, pilar, colon), new Vector3[] { vertice2, vertice3, vertice7, vertice6 });

                        if (colon > 0) // -Z CHECK
                        {
                            if (chunkLayers[pilar, line, colon - 1] == BlockTypes.Air)
                                AddFace(new Vector3(line, pilar, colon), new Vector3[] {
                                    vertice0, vertice1, vertice5, vertice4
                                });
                        }
                        else AddFace(new Vector3(line, pilar, colon), new Vector3[] { vertice0, vertice1, vertice5, vertice4 });

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
    
    /*private void OnDrawGizmos()
    {
        if (vertices == null)
            return;

        for (int i = 0; i < vertices.Count; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }*/
}
