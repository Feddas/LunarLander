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
        //there will be a nullref if the landingPad is generated ontop of the players ship
        var pad1 = this.helper.PositionNew(this.LandingPad,
            Random.Range(-10, -4), Random.Range(-10, 4));
        var pad2 = this.helper.PositionNew(this.LandingPad,
            Random.Range(4, 10), Random.Range(-10, 4));

        pad1.transform.Rotate(0, -90, 0);
        pad2.transform.Rotate(0, -90, 0);

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
        int pads = landingPadLocations.Count;
        float degreesPerPad = 360 / pads;
        IList<Vector2> wallCoords;//new List<Vector2>();

        if (pads == 2)
        {
            Vector2 normal = 2 * landingPadLocations[0] - landingPadLocations[1];
            wallCoords = wallForPad(landingPadLocations[0], normal, degreesPerPad);
            normal = 2 * landingPadLocations[1] - landingPadLocations[0];
            foreach (var coord in wallForPad(landingPadLocations[1], normal, degreesPerPad))
            {
                wallCoords.Add(coord);
            }
            //wallForPad(new Vector2(0,0), new Vector2(1,0), 180);

            //close off the cave
            wallCoords.Add(wallCoords[0]);
        }
        else
        {
            Debug.Log("Unsupported number of landing pads");
            return null;
        }

        helper.BuildWall(meshBuilder, wallCoords);
        //		new List<Vector2>() {
        //			new Vector2(-10,-12),
        //			new Vector2(-12,-6),
        //			new Vector2(-10,0),
        //			new Vector2(-12,6),
        //			new Vector2(-2,12)
        //		});

        Mesh mesh = meshBuilder.CreateMesh();
        mesh.RecalculateNormals();

        return mesh;
    }

    private IList<Vector2> wallForPad(Vector2 padLocation, Vector2 padNormal, float degreeOfWall)
    {
        IList<Vector2> wall = new List<Vector2>();
        degreeOfWall /= 2; // focus on halves, below the padNormal and above it.

        float increment = -1 * degreeOfWall;
        Vector2 newPoint;
        while (increment <= degreeOfWall)
        {
            newPoint = rotatePadFromNormal(padLocation, padNormal, increment);
            wall.Add(newPoint);
            increment += 30;
        }
        return wall;
    }

    private Vector2 rotatePadFromNormal(Vector2 padLocation, Vector2 padNormal, float rotation)
    {
        rotation *= Mathf.Deg2Rad;
        padNormal *= Random.Range(.9f, 1.1f); //add random variation to the magnitude to give the cave wall a more jagged look

        Vector2 newPoint;
        //how to rotate: x_new = ( (x - cx) * cos(theta) + (y - cy) * sin(theta)) + cx http://stackoverflow.com/questions/14842090/rotate-line-around-center-point-given-two-vertices
        newPoint.x = ((padNormal.x - padLocation.x) * Mathf.Cos(rotation) + (padNormal.y - padLocation.y) * Mathf.Sin(rotation)) + padLocation.x;
        newPoint.y = (-1 * (padNormal.x - padLocation.x) * Mathf.Sin(rotation) + (padNormal.y - padLocation.y) * Mathf.Cos(rotation)) + padLocation.y;

        return newPoint;
    }
}