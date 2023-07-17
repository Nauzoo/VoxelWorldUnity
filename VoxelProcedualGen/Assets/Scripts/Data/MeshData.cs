using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshData
{
    public List<Vector3> vertexes = new List<Vector3>(); // Vertexes that make up the Mesh polygon
    public List<int> triangles = new List<int>(); // Quantity of triangles that will be made using the vertexes
    public List<Vector2> UV = new List<Vector2>(); // UV for texture mapping

    public List<Vector3> colVertexes = new List<Vector3>(); // Vertexes that are part of a collideable area
    public List<int> colTrians = new List<int>(); // Collideable faces quantity

    public MeshData waterMesh;
    private bool isLand = true; // main != water

    public MeshData(bool isLand)
    {
        if (isLand)
        {
            waterMesh = new MeshData(false);
        }
    }

    public void AddVertex(Vector3 position, bool isCollider)
    {
        vertexes.Add(position);
        if (isCollider)
        {
            colVertexes.Add(position);
        }
    }

    public void AddQuads(bool isCollider)
    {
        triangles.Add(vertexes.Count - 4);
        triangles.Add(vertexes.Count - 3);
        triangles.Add(vertexes.Count - 2);

        triangles.Add(vertexes.Count - 4);
        triangles.Add(vertexes.Count - 2);
        triangles.Add(vertexes.Count - 1);

        if (isCollider)
        {
            colTrians.Add(colVertexes.Count - 4);
            colTrians.Add(colVertexes.Count - 3);
            colTrians.Add(colVertexes.Count - 2);

            colTrians.Add(colVertexes.Count - 4);
            colTrians.Add(colVertexes.Count - 2);
            colTrians.Add(colVertexes.Count - 1);
        } 
    }
}
