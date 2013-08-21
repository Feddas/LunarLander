using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Component of Level1 scenes Gui empty gameobject
/// </summary>
public class GuiInGame : MonoBehaviour {
    public GameObject nguiControls;
    public GameObject nguiMenu;
    public GameObject nguiMenuTopButton;
    public GameObject nguiMenuSoundButton;
	public AudioClip WinClip;
	public AudioClip LoseClip;
	public UILabel ScoreLevel;
	public UILabel ScoreTotal;
	
    private string guiMode = "InGame";
	private bool isMenuDisplayed = false;
	
	private string toggleSoundLabel
	{
		get
		{
			return "Turn Sound " + (Globals.IsSoundOn ? "Off" : "On");
		}
	}
	
	void Start ()
	{
		toggleSound();
	}
	
	void Update () {
		if (Input.GetKeyDown("escape"))
		{
			Time.timeScale = 0;
			guiMode = "Paused";
		}
	}
	
	void OnGUI ()
	{
		if (isMenuDisplayed == false && guiMode != "InGame")
		{
			displayGui(nguiMenu);
			changeButton(nguiMenuSoundButton, toggleSoundLabel);
			if (guiMode == "Paused")
			{
				changeButton(nguiMenuTopButton, "Resume Game", "OnClickResume");
			}
			else if (guiMode == "Win")
			{
				changeButton(nguiMenuTopButton, "Next Level", "OnClickNextLevel");
			}
			else if (guiMode == "Lose")
			{
				changeButton(nguiMenuTopButton, "Retry Level", "OnClickRetry");
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
	
	public void OnClickToggleSound()
	{
		Globals.IsSoundOn = !Globals.IsSoundOn;
		changeButton(nguiMenuSoundButton, toggleSoundLabel);
		toggleSound();
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
	
	public void UpdateScore(int levelScore)
	{
		ScoreLevel.text = "Level Score:" + levelScore;
	}
	
	public void Win(int levelScore)
	{
		if (Globals.IsSoundOn)
		{
			audio.clip = WinClip;
			audio.Play();
		}
		Time.timeScale = 0;
		guiMode = "Win";
		PlayerPrefs.SetInt(PlayerPrefKey.Level, Application.loadedLevel+1);
		int totalScore = levelScore;
		if (PlayerPrefs.HasKey(PlayerPrefKey.TotalScore))
		{
			totalScore = PlayerPrefs.GetInt(PlayerPrefKey.TotalScore) + levelScore;
			PlayerPrefs.SetInt(PlayerPrefKey.TotalScore, totalScore);
		}
		else
		{
			PlayerPrefs.SetInt(PlayerPrefKey.TotalScore, totalScore);
		}
		PlayerPrefs.Save();
		
		ScoreTotal.text = "Total Score:" + totalScore;
	}
	
	public void Lose()
	{
		Action afterExplosion = () => 
		{
			if (Globals.IsSoundOn)
			{
				audio.clip = LoseClip;
				audio.Play();
			}
			Time.timeScale = 0;
			guiMode = "Lose";
		};
		
		//Refactor: try to move this call into PlayerShip.cs, tried couldn't figure out why Coroutine wouldn't work
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
	
	private void toggleSound()
	{
		bool isSoundOn = Globals.IsSoundOn;
				
		//toggle thruster sounds
		var buttonsWithSound = nguiControls.transform.GetComponentsInChildren<UIButtonSound>();
		foreach (UIButtonSound buttonSound in buttonsWithSound) {
			buttonSound.enabled = isSoundOn;
		}
		
		//toggle explosion sounds
//		var explosions = Globals.PlayerShip.GetComponent<PlayerKeyboard>().shipExplosions;
//		foreach (GameObject explosion in explosions) {
//			var sound = explosion.GetComponent<DetonatorSound>();
//			sound.on = isSoundOn;
//			sound.maxVolume = 0; //isSoundOn ? 1 : 
//			sound.enabled = isSoundOn;
//		}
	}
}