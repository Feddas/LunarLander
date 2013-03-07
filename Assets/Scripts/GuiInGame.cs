using UnityEngine;
using System.Collections;
using System;

public class GuiInGame : MonoBehaviour {
	private string guiMode = "InGame";
	private int numActivated, totalLandingPads;
		
	void Update () {
		if (Input.GetKeyDown("escape"))
		{
			Time.timeScale = 0;
			guiMode = "Paused";
		}
	}
	
	void OnGUI ()
	{
		Menu menu = new Menu();
		if (guiMode == "Paused")
		{
			menu.BuildMenu(ref guiMode, "Resume Game");
		}
		if (guiMode == "Win")
		{
			menu.BuildMenu(ref guiMode, "Next Level", () => Application.LoadLevel(Application.loadedLevel+1));
		}
		if (guiMode == "Lose")
		{
			menu.BuildMenu(ref guiMode, "Retry Level", () => Application.LoadLevel(Application.loadedLevel));
		}
	}
	
	public void LandingPadActivated()
	{
		numActivated++;
		if (numActivated == totalLandingPads)
		{
			Win();
		}
		print ("LZ activated");
	}
	
	void Win()
	{
		Time.timeScale = 0;
		guiMode = "Win";
		PlayerPrefs.SetInt("PlayerLevel",Application.loadedLevel+1);
	}
	
	public void Lose()
	{
		Action afterExplosion = () => 
		{
			Time.timeScale = 0;
			guiMode = "Lose";
		};
		StartCoroutine(yieldForExplosion(afterExplosion));
	}
	
	/// <summary> Gives 3 seconds for the explosion animation to play.  </summary>
	private IEnumerator yieldForExplosion(Action afterExplosion)
	{
		yield return new WaitForSeconds(3);
		afterExplosion();
	}
}