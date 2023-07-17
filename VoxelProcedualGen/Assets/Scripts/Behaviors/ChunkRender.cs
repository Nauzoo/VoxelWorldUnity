using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class ChunkRender : MonoBehaviour
{
    MeshFilter meshFilter;
    MeshRenderer meshRender;
    MeshCollider meshCollider;
    Mesh mesh;
    public bool showGizmos = false;

    public ChunkData chunkData { get; private set; }

    public bool playerMod
    {
        get
        {
            return chunkData.playerMod;
        }
        set
        {
            chunkData.playerMod = value;
        }
    }

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        mesh = meshFilter.mesh;
    }

    public void awakeChunk(ChunkData data)
    {
        this.chunkData = data;
    }

    private void RenderMesh(MeshData data)
    {
        mesh.Clear();

        mesh.subMeshCount = 2;
        mesh.vertices = data.vertexes.Concat(data.waterMesh.vertexes).ToArray();

        mesh.SetTriangles(data.triangles.ToArray(), 0);
        mesh.SetTriangles(data.waterMesh.triangles.Select(i => i + data.triangles.Count).ToArray(), 0);

        mesh.uv = data.UV.Concat(data.waterMesh.UV).ToArray();
        mesh.RecalculateNormals();

        meshCollider.sharedMesh = null;
        
        Mesh collisionMesh = new Mesh();
        collisionMesh.vertices = data.colVertexes.ToArray();
        collisionMesh.triangles = data.colTrians.ToArray();
        collisionMesh.RecalculateNormals();

        meshCollider.sharedMesh = collisionMesh;
    }

    public void UpdateChunk()
    {
        RenderMesh(Chunk.GetChunkMesh(chunkData));
    }

    public void UpdateChunk(MeshData data)
    {
        RenderMesh(data);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (showGizmos)
        {
            if(Application.isPlaying && chunkData != null)
            {
                if (Selection.activeObject == gameObject)
                    Gizmos.color = new Color(0f, 1f, 0.2f, 0.4f);
                
                else
                    Gizmos.color = new Color(1f, 0f, 0.2f, 0.4f);
                
                Gizmos.DrawCube(transform.position + new Vector3(chunkData.size / 2f, chunkData.height / 2f, chunkData.size / 2f),
                        new Vector3(chunkData.size, chunkData.height, chunkData.size));
            }
        }
    }

#endif
}
