using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Component of MainMenu scenes Gui empty gameobject
/// </summary>
//[ExecuteInEditMode]
public class GuiMainMenu : MonoBehaviour
{
	public void OnClickNewGame()
	{
		PlayerPrefs.DeleteAll();
		PlayerPrefs.SetInt(PlayerPrefKey.Level, 1);
		Globals.Game.CurrentMode = Mode.InGame;
		Application.LoadLevel(1);
	}
	
	public void OnClickContinue()
	{
		Application.LoadLevel(PlayerPrefs.GetInt(PlayerPrefKey.Level));
	}
	
	public void OnClickQuit()
	{
		Application.Quit();
	}
}