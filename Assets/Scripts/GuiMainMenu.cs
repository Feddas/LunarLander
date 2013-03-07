using UnityEngine;
using System.Collections;
using System;

[ExecuteInEditMode]
public class GuiMainMenu : MonoBehaviour {
	void OnGUI ()
	{
		Action firstButtonAction = () =>
		{
			PlayerPrefs.SetInt("PlayerLevel", 1);
			Application.LoadLevel(1);
		};
		Action secondButtonAction = () =>
		{
			Application.LoadLevel(PlayerPrefs.GetInt("PlayerLevel"));
		};
		
		(new Menu()).BuildMainMenu(
			"New Game", firstButtonAction,
			"Continue Game", secondButtonAction);
	}
}