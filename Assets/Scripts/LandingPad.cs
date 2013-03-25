using UnityEngine;
using System.Collections;

public class LandingPad : MonoBehaviour {
	
	public Light stationLight;
	public Material onMaterial;
	public Color onColor;
	public GuiInGame Gui;
	
	// Use this for initialization
	void Start ()
	{
		//Automatically populate the Gui variable with the Gui script attached to the EmptyObject Gui using its Gui Tag
		Gui = GameObject.FindWithTag("Gui").GetComponent(typeof(GuiInGame)) as GuiInGame;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Activate()
	{
		if (stationLight.color != onColor)
		{
			print (gameObject.name + " has been activated");
			renderer.material = onMaterial;
			stationLight.color = onColor;
		
			Gui.LandingPadActivated();
		}
	}
}
