using UnityEngine;
using System.Collections;
using System;

public class GuiInGame : MonoBehaviour {
    public GameObject nguiControls;
    public GameObject nguiMenu;
    public GameObject nguiMenuTopButton;
    public GameObject nguiMenuSoundButton;
	public AudioClip WinClip;
	public AudioClip LoseClip;
	
    private GameModeEnum guiMode = GameModeEnum.InGame;
	private int numActivated, totalLandingPads;
	private bool isMenuDisplayed = false;
	
	private string toggleSoundLabel
	{
		get
		{
			return "Turn Sound " + (Globals.HasSetting(Setting.IsSoundOn) ? "Off" : "On");
		}
	}
	
	#region [ Overriden functions ]
	void Start ()
	{
		totalLandingPads = (GameObject.FindGameObjectsWithTag("LandingPad") as GameObject[]).Length;
		Globals.GameSettings = loadSettings();
		updateSound();
	}
	
	void Update ()
	{
		if (Input.GetKeyDown("escape"))
		{
			Time.timeScale = 0;
			guiMode = GameModeEnum.Paused;
		}
	}
	
	void OnGUI ()
	{
		if (isMenuDisplayed == false && guiMode != GameModeEnum.InGame)
		{
			displayGui(nguiMenu);
			changeButton(nguiMenuSoundButton, toggleSoundLabel);
			if (guiMode == GameModeEnum.Paused)
			{
				changeButton(nguiMenuTopButton, "Resume Game", "OnClickResume");
			}
			else if (guiMode == GameModeEnum.Win)
			{
				changeButton(nguiMenuTopButton, "Next Level", "OnClickNextLevel");
			}
			else if (guiMode == GameModeEnum.Lose)
			{
				changeButton(nguiMenuTopButton, "Retry Level", "OnClickRetry");
			}
		}
		else if (guiMode == GameModeEnum.InGame)
		{
			displayGui(nguiControls);
		}
	}
	#endregion [ Overriden functions ]
	
	#region [ Button events ]
	public void OnClickResume()
	{
		Time.timeScale = 1;
		guiMode = GameModeEnum.InGame;
	}
	
	public void OnClickNextLevel()
	{
		Time.timeScale = 1;
		guiMode = GameModeEnum.InGame;
		Application.LoadLevel(Application.loadedLevel+1);
	}
	
	public void OnClickRetry()
	{
		Time.timeScale = 1;
		guiMode = GameModeEnum.InGame;
		Application.LoadLevel(Application.loadedLevel);
	}
	
	public void OnClickToggleSound()
	{
		Globals.ToggleSetting(Setting.IsSoundOn);
		saveSettings();
		
		changeButton(nguiMenuSoundButton, toggleSoundLabel);
		updateSound();
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
	
	#region [ Public functions ]
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
		if (Globals.HasSetting(Setting.IsSoundOn))
		{
			audio.clip = WinClip;
			audio.Play();
		}
		Time.timeScale = 0;
		guiMode = GameModeEnum.Win;
		PlayerPrefs.SetInt("PlayerLevel",Application.loadedLevel+1);
	}
	
	public void Lose()
	{
		numActivated = 0;
		Action afterExplosion = () => 
		{
			if (Globals.HasSetting(Setting.IsSoundOn))
			{
				audio.clip = LoseClip;
				audio.Play();
			}
			Time.timeScale = 0;
			guiMode = GameModeEnum.Lose;
		};
		StartCoroutine(yieldForExplosion(afterExplosion));
	}
	#endregion [ Public functions ]
	
	#region [ private functions ]
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
	
	private void changeButton(GameObject button, string text, string actionName = null)
	{
		UILabel label = button.GetComponentInChildren(typeof(UILabel)) as UILabel;
		label.text = text;
		
		if (actionName != null)
		{
			UIButtonMessage buttonMessage = button.GetComponent("UIButtonMessage") as UIButtonMessage;
			buttonMessage.functionName = actionName;
		}
	}
	
	private void updateSound()
	{
		bool isSoundOn = Globals.HasSetting(Setting.IsSoundOn);
				
		//toggle thruster sounds on NGUI touch buttons
		var buttonsWithSound = nguiControls.transform.GetComponentsInChildren<UIButtonSound>();
		foreach (UIButtonSound buttonSound in buttonsWithSound) {
			buttonSound.enabled = isSoundOn;
		}
	}
	
	private Setting loadSettings()
	{
		if (PlayerPrefs.HasKey("GameSettings"))
		{
			return (Setting)PlayerPrefs.GetInt("GameSettings");
		}
		else
		{
			return Setting.None;
		}
	}
	
	private void saveSettings()
	{
		PlayerPrefs.SetInt("GameSettings", (int)Globals.GameSettings);
	}
	#endregion [ private functions ]
}