using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Component of LevelN scenes LevelNScripts empty gameobject.
/// Landing pads.
/// </summary>
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class Cave : MonoBehaviour
{
	public GameObject LandingPad;
	
	private IList<Vector2> landingPadLocations = new List<Vector2>();
	private GenHelper helper = new GenHelper();
	
	void Start()
	{
		createLandingPads();
		createCave();
	}
	
	void Update()
	{
	
	}
	
	private void createLandingPads()
	{
		var pad1 = this.helper.PositionNew(this.LandingPad, 
			Random.Range(-10, 0), Random.Range(-10, -2));
		var pad2 = this.helper.PositionNew(this.LandingPad, 
			Random.Range(0, 10), Random.Range(-10, -2));
		
		landingPadLocations.Add(helper.Vector3to2(pad1.transform.position));
		landingPadLocations.Add(helper.Vector3to2(pad2.transform.position));
	}
	
	private void createCave()
	{
		MeshFilter filter = GetComponent<MeshFilter>();
		MeshCollider collider = GetComponent<MeshCollider>();
		if (filter != null && collider != null)
		{
			filter.sharedMesh // make the wall visible
				= collider.sharedMesh // enable physics with the wall
					= caveMesh();
		}
	}
	
	private Mesh caveMesh()
	{
		MeshBuilder meshBuilder = new MeshBuilder();
		
		helper.BuildWall(meshBuilder, new List<Vector2>() {
			new Vector2(-10,-12),
			new Vector2(-12,-6),
			new Vector2(-10,0),
			new Vector2(-12,6),
			new Vector2(-2,12)
		});
		
		//TODO: replace code below with buildWall
//		//left side
//		buildQuad(meshBuilder,
//			offset: new Vector3(-12, -6, -4),
//			widthDir: new Vector3(0, 0, 8),
//			lengthDir: new Vector3(0, 12, 0));
//		
//		//bottom
//		buildQuad(meshBuilder,
//			offset: new Vector3(5, -15, -4),
//			widthDir: new Vector3(0, 0, 8),
//			lengthDir: new Vector3(-10, 0, 0));
		
		Mesh mesh = meshBuilder.CreateMesh();
		mesh.RecalculateNormals();
		
		return mesh;
	}
	
//	private void buildQuad(MeshBuilder meshBuilder,
//		Vector3 offset,
//		Vector3 widthDir,
//		Vector3 lengthDir)
//	{
//		Vector3 normal = Vector3.Cross(lengthDir, widthDir).normalized;
//		
//		meshBuilder.Vertices.Add(offset);
//		meshBuilder.UVs.Add(new Vector2(0.0f, 0.0f));
//		meshBuilder.Normals.Add(normal);
//		
//		meshBuilder.Vertices.Add(offset + lengthDir);
//		meshBuilder.UVs.Add(new Vector2(0.0f, 1.0f));
//		meshBuilder.Normals.Add(normal);
//		
//		meshBuilder.Vertices.Add(offset + lengthDir + widthDir);
//		meshBuilder.UVs.Add(new Vector2(1.0f, 1.0f));
//		meshBuilder.Normals.Add(normal);
//		
//		meshBuilder.Vertices.Add(offset + widthDir);
//		meshBuilder.UVs.Add(new Vector2(1.0f, 0.0f));
//		meshBuilder.Normals.Add(normal);
//		
//		int baseIndex = meshBuilder.Vertices.Count - 4;
//		meshBuilder.AddTriangle(baseIndex, baseIndex + 1, baseIndex + 2);
//		meshBuilder.AddTriangle(baseIndex, baseIndex + 2, baseIndex + 3);
//	}
}
