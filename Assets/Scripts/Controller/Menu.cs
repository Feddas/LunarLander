using UnityEngine;
using System.Collections;
using System;

public class Menu {
	public Menu()
	{
	}
	
	public void BuildMenu(ref string guiMode,
		string firstButton,
		Action firstButtonAction = null)
	{
		Action firstButtonWithTimeScale = () =>
		{
			Time.timeScale = 1;
			if (firstButtonAction != null)
				firstButtonAction();
		};
		Action secondButtonAction = () =>
		{
			Time.timeScale = 1;
			Application.LoadLevel(0);
		};
		
		buildMenu(ref guiMode,
			firstButton, firstButtonWithTimeScale,
			"Quit to Main Menu", secondButtonAction);
	}
	
	public void BuildMainMenu(string firstButton,
		Action firstButtonAction,
		string secondButton,
		Action secondButtonAction)
	{
		string dummy = "main menu doesn't do anything with guiMode";
		
		buildMenu(ref dummy,
			firstButton, firstButtonAction,
			secondButton, secondButtonAction,
			showSecondButton: PlayerPrefs.HasKey("PlayerLevel"));
	}
	
	private void buildMenu(ref string guiMode,
		string firstButton, Action firstButtonAction,
		string secondButton, Action secondButtonAction,
		bool showSecondButton = true)
	{
        float width = 200;
        float height = 60;
        float left = Screen.width / 2 - width / 2;
        float top = Screen.height / 2 - height / 2;
		if(GUI.Button(new Rect(left, top, width, height), firstButton))
		{
			guiMode = "InGame";
			if (firstButtonAction != null)
				firstButtonAction();
		}
		
		if (showSecondButton)
		{
			top += 2*height;
			if(GUI.Button(new Rect(left, top, width, height), secondButton))
			{
				if (secondButtonAction != null)
					secondButtonAction();
			}
		}
	}
}
