using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class for holding all the data needed for a mesh in progress.
/// copied from http://jayelinda.com/wp/modelling-by-numbers-part-1a/
/// </summary>
public class MeshBuilder
{
    /// <summary>
    /// The vertex positions of the mesh.
    /// </summary>
    public List<Vector3> Vertices { get { return m_Vertices; } }
    private List<Vector3> m_Vertices = new List<Vector3>();

    /// <summary>
    /// The vertex normals of the mesh.
    /// </summary>
    public List<Vector3> Normals { get { return m_Normals; } }
    private List<Vector3> m_Normals = new List<Vector3>();

    /// <summary>
    /// The UV coordinates of the mesh.
    /// </summary>
    public List<Vector2> UVs { get { return m_UVs; } }
    private List<Vector2> m_UVs = new List<Vector2>();

    //indices for the triangles:
    private List<int> m_Indices = new List<int>();

    /// <summary>
    /// Adds a triangle to the mesh.
    /// </summary>
    /// <param name="index0">The vertex index at corner 0 of the triangle.</param>
    /// <param name="index1">The vertex index at corner 1 of the triangle.</param>
    /// <param name="index2">The vertex index at corner 2 of the triangle.</param>
    public void AddTriangle(int index0, int index1, int index2)
    {
        m_Indices.Add(index0);
        m_Indices.Add(index1);
        m_Indices.Add(index2);
    }

    /// <summary>
    /// Initialises an instance of the Unity Mesh class, based on the stored values.
    /// </summary>
    /// <returns>The completed mesh.</returns>
    public Mesh CreateMesh()
    {
        //Create an instance of the Unity Mesh class:
        Mesh mesh = new Mesh();

        //add our vertex and triangle values to the new mesh:
        mesh.vertices = m_Vertices.ToArray();
        mesh.triangles = m_Indices.ToArray();

        //Normals are optional. Only use them if we have the correct amount:
        if (m_Normals.Count == m_Vertices.Count)
            mesh.normals = m_Normals.ToArray();

        //UVs are optional. Only use them if we have the correct amount:
        if (m_UVs.Count == m_Vertices.Count)
            mesh.uv = m_UVs.ToArray();

        //have the mesh recalculate its bounding box (required for proper rendering):
        mesh.RecalculateBounds();

        return mesh;
    }
}