using UnityEngine;
using System.Collections;
using System;

//[ExecuteInEditMode]
public class GuiMainMenu : MonoBehaviour {
	public void OnClickNewGame()
	{
		PlayerPrefs.SetInt("PlayerLevel", 1);
		Application.LoadLevel(1);
	}
	
	public void OnClickContinue()
	{
		Application.LoadLevel(PlayerPrefs.GetInt("PlayerLevel"));
	}
	
	public void OnClickQuit()
	{
		Application.Quit();
	}
}