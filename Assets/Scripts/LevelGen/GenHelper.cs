﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// common methods for game level generation.
/// </summary>
public class GenHelper
{
	public GameObject PositionNew(GameObject obj, float x, float y)
	{
		GameObject instance = MonoBehaviour.Instantiate(obj) as GameObject;
		instance.transform.position = new Vector3(x, y, 0);
		
		return instance;
	}
	
	/// <summary>
	/// When this is -1 the previous index was new, not pre-existing
	/// </summary>
	int existingVertexIndex = -1;
	
	/// <summary>
	/// builds a wall with the normal pointing inwards if vertices are given clockwise
	/// based off terrian of: http://jayelinda.com/wp/modelling-by-numbers-part-1a/
	/// </summary>
	public void BuildWall(MeshBuilder meshBuilder, IList<Vector2> vertices)
	{
		if (vertices == null || vertices.Count < 1)
			return;
		
		Vector2 vector2;
		int segments = vertices.Count - 1;
		for (int index = 0; index < vertices.Count; index++)
		{
			// Add vertex
			vector2 = vertices[index];
			bool isDistinctVertex = (vertices.IndexOf(vector2) == index);
			if (isDistinctVertex) // add new vertex
			{
				meshBuilder.Vertices.Add(Vector2to3(vector2, -4));
				meshBuilder.Vertices.Add(Vector2to3(vector2, 4));
			}
			else // make use of existing vertex.
			{
				if (existingVertexIndex != -1)
					Debug.Log("BuildWall is reusing too many vertices");
				existingVertexIndex = vertices.IndexOf(vector2)
					* 2 // because there are 2 vertices added per vector2 (z of 4 & -4)
						+ 1; // use the z of 4 vertex
			}
			
			//init UV map with values for index == 0
			Vector2 uvFront = new Vector2(0,0),
				uvBack = new Vector2(0,1);
			
			//connect with previous vertice
			if (index > 0)
			{
				uvFront.x = uvBack.x = (1.0f / segments) * index;
				
				int[] indices = quadrilateralIndices(isDistinctVertex, meshBuilder.Vertices.Count);
				if (indices == null)
					continue; //skip adding triangles for this vertex iteration
				
				int index0 = indices[0],
					index1 = indices[1],
					index2 = indices[2],
					index3 = indices[3];
				
				meshBuilder.AddTriangle(index0, index2, index1);
				meshBuilder.AddTriangle(index2, index3, index1);
			}
			
			//Debug.Log("added " + uvFront.x + ", " + uvFront.y);
			
			meshBuilder.UVs.Add(uvFront);
			meshBuilder.UVs.Add(uvBack);
		}
	}
	
	public Vector3 Vector2to3(Vector2 vector, float z)
	{
		return new Vector3(vector.x, vector.y, z);
	}
	
	public Vector2 Vector3to2(Vector3 vector)
	{
		return new Vector2(vector.x, vector.y);
	}
	
	private int[] quadrilateralIndices(bool isNewVertex, int vertexCount)
	{
		int[] indices = new int[4];
		int baseIndex = vertexCount - 1;
		
		// handle new vertex where the previous vertex was also new
		if (isNewVertex //can use Vertices.count
			&& existingVertexIndex == -1) //previous vertex was not preexisting
		{
			indices[0] = baseIndex;
			indices[1] = baseIndex - 1;
			indices[2] = baseIndex - 2;
			indices[3] = baseIndex - 3;
		}
		
		// handle new vertex where the previous vertex was old
		else if (isNewVertex
			&& existingVertexIndex != -1)
		{
			indices[0] = baseIndex;
			indices[1] = baseIndex - 1;
			indices[2] = existingVertexIndex;
			indices[3] = existingVertexIndex - 1;
			
			existingVertexIndex = -1; // previous is now this newly added vertex
		}
				
		// handle old vertex where the previous vertex was new
		else if (isNewVertex == false)
		{
			indices[0] = existingVertexIndex;
			indices[1] = existingVertexIndex - 1;
			indices[2] = baseIndex;
			indices[3] = baseIndex - 1;
		}
		
		//note can't handle two old indices because
		// isNewVertex == false && existingVertexIndex == -1 // never happens because isNewVertex == false causes a set of existingVertexIndex
		// return null; //flag that we don't want to add triangles
		
		return indices;
	}
}