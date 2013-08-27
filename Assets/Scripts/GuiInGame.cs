using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Component of Level1 scenes Gui empty gameobject
/// </summary>
public class GuiInGame : MonoBehaviour
{
	public GameObject nguiControls;
	public GameObject nguiMenu;
	public GameObject nguiMenuTopButton;
	public GameObject nguiMenuSoundButton;
	public AudioClip WinClip;
	public AudioClip LoseClip;
	public UILabel ScoreLevel;
	public UILabel ScoreTotal;
	
	private State game = new State(Mode.InGame);
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
		game.CurrentModeChanged += HandleGameModeChanged;
	}

	void HandleGameModeChanged (object sender, EventArgs<Mode> e)
	{
		//Debug.Log("Gamemode is now " + e.Data.ToString() + " == " + game.CurrentMode.ToString());
		
		//show menu
		if (isMenuDisplayed == false && game.CurrentMode != Mode.InGame)
		{
			displayGui(nguiMenu);
			changeButton(nguiMenuSoundButton, toggleSoundLabel);
			if (game.CurrentMode == Mode.Paused)
			{
				changeButton(nguiMenuTopButton, "Resume Game", "OnClickResume");
			}
			else if (game.CurrentMode == Mode.Win)
			{
				changeButton(nguiMenuTopButton, "Next Level", "OnClickNextLevel");
			}
			else if (game.CurrentMode == Mode.Lose)
			{
				changeButton(nguiMenuTopButton, "Retry Level", "OnClickRetry");
			}
		}
		
		//show space ship controls HUD
		else if (game.CurrentMode == Mode.InGame)
		{
			displayGui(nguiControls);
		}
	}
	
	void Update () {
		if (Input.GetKeyDown("escape"))
		{
			Time.timeScale = 0;
			game.CurrentMode = Mode.Paused;
		}
	}
	
	#region [ Button events ]
	public void OnClickResume()
	{
		Time.timeScale = 1;
		game.CurrentMode = Mode.InGame;
	}
	
	public void OnClickNextLevel()
	{
		Time.timeScale = 1;
		game.CurrentMode = Mode.InGame;
		Application.LoadLevel(Application.loadedLevel+1);
	}
	
	public void OnClickRetry()
	{
		Time.timeScale = 1;
		game.CurrentMode = Mode.InGame;
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
		game.CurrentMode = Mode.Win;
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
			game.CurrentMode = Mode.Lose;
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