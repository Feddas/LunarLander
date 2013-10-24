using UnityEngine;
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
			vector2 = vertices[index];
			meshBuilder.Vertices.Add(Vector2to3(vector2, -4));
			meshBuilder.Vertices.Add(Vector2to3(vector2, 4));
			
			Vector2 uvFront = new Vector2(0,0),
				uvBack = new Vector2(0,1);
			
			if (index > 0)
			{
				uvFront.x = uvBack.x = (1.0f / segments) * index;
				
				int baseIndex = meshBuilder.Vertices.Count - 1;
				
				int index0 = baseIndex;
				int index1 = baseIndex - 1;
				int index2 = baseIndex - 2;
				int index3 = baseIndex - 3;
				
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
}