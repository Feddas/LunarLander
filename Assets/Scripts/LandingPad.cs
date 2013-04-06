using UnityEngine;
using System.Collections;

public class LandingPad : MonoBehaviour {
	public Light stationLight;
	public Material onMaterial;
	public Color onColor;
	public GuiInGame Gui;
	
	void Start ()
	{
		//Automatically populate the Gui variable with the Gui script attached to the EmptyObject Gui using its Gui Tag
		Gui = GameObject.FindWithTag("Gui").GetComponent(typeof(GuiInGame)) as GuiInGame;
	}
	
	void Update ()
	{
	}
	
	public void Activate()
	{
		if (Globals.HasSetting(Setting.IsSoundOn))
			audio.Play();
		
		if (stationLight.color != onColor)
		{
			print (gameObject.name + " has been activated");
			renderer.material = onMaterial;
			stationLight.color = onColor;
		
			Gui.LandingPadActivated();
		}
	}
}
