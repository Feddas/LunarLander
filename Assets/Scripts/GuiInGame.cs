using UnityEngine;
using System.Collections;
using System;

public class GuiInGame : MonoBehaviour {
    public GameObject nguiControls;
    public GameObject nguiMenu;
    public GameObject nguiMenuTopButton;
    private string guiMode = "InGame";
	private int numActivated, totalLandingPads = 2;
	private bool isMenuDisplayed = false;
		
	void Update () {
		if (Input.GetKeyDown("escape"))
		{
			Time.timeScale = 0;
			guiMode = "Paused";
		}
	}
	
	void OnGUI ()
	{
		if (isMenuDisplayed == false)
		{
			if (guiMode == "Paused")
			{
				displayGui(nguiMenu);
				changeTopButton("Resume Game", "OnClickResume");
			}
			else if (guiMode == "Win")
			{
				displayGui(nguiMenu);
				changeTopButton("Next Level", "OnClickNextLevel");
			}
			else if (guiMode == "Lose")
			{
				displayGui(nguiMenu);
				changeTopButton("Retry Level", "OnClickRetry");
			}
		}
		else if (guiMode == "InGame")
		{
			displayGui(nguiControls);
		}
	}
	
	#region [ Button events ]
	public void OnClickResume()
	{
		Time.timeScale = 1;
		guiMode = "InGame";
	}
	
	public void OnClickNextLevel()
	{
		Time.timeScale = 1;
		guiMode = "InGame";
		Application.LoadLevel(Application.loadedLevel+1);
	}
	
	public void OnClickRetry()
	{
		Time.timeScale = 1;
		guiMode = "InGame";
		Application.LoadLevel(Application.loadedLevel);
	}
	
	public void OnClickMainMenu()
	{
		Time.timeScale = 1;
		Application.LoadLevel(0);
	}
	
	public void OnClickQuit()
	{
		Application.Quit();
	}
	#endregion [ Button events ]
	
	public void LandingPadActivated()
	{
		numActivated++;
		if (numActivated == totalLandingPads)
		{
			Win();
		}
		print ("LZ activated");
	}
	
	private void Win()
	{
		Time.timeScale = 0;
		guiMode = "Win";
		PlayerPrefs.SetInt("PlayerLevel",Application.loadedLevel+1);
	}
	
	public void Lose()
	{
		numActivated = 0;
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
	
	private void displayGui(GameObject primary)
	{
		if (primary == nguiMenu)
		{
			NGUITools.SetActive(nguiMenu,true);
			
			//SetActive() line below causes exception "!IsActive () && !m_RunInEditMode"
			//NGUITools.SetActive(nguiControls,false);
			
			isMenuDisplayed = true;
		}
		else
		{
			NGUITools.SetActive(nguiMenu,false);
			
			//deactivated this line due to "!IsActive () && !m_RunInEditMode" error above
			//NGUITools.SetActive(nguiControls,true);
			
			isMenuDisplayed = false;
		}
	}
	
	private void changeTopButton(string text, string actionName)
	{
		UIButtonMessage topButton = nguiMenuTopButton.GetComponent("UIButtonMessage") as UIButtonMessage;
		topButton.functionName = actionName;
		
		UILabel label = nguiMenuTopButton.GetComponentInChildren(typeof(UILabel)) as UILabel;
		label.text = text;
	}
}