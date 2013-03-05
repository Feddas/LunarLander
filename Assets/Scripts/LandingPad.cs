using UnityEngine;
using System.Collections;

public class LandingPad : MonoBehaviour {
	
	public Light stationLight;
	public Material onMaterial;
	public Color onColor;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Activate()
	{
		print (gameObject.name + " has been activated");
		renderer.material = onMaterial;
		stationLight.color = onColor;
	}
}
