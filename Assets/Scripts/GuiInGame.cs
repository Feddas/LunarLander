using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Component of Level1 scenes GuiScripts empty gameobject.
/// The parent of the GuiScripts object is persistent across scenes.
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
	public UISlider FuelGauge;
	
	private State game;
	private float fuelGaugeMaxWidth;
	private const float fuelMax = 400;
	
	private string toggleSoundLabel
	{
		get
		{
			return "Turn Sound " + (Globals.IsSoundOn ? "Off" : "On");
		}
	}
	
	void Start()
	{
		toggleSound();
		game = Globals.Game;
		game.CurrentModeChanged += HandleGameModeChanged;
		game.FuelRemainingChanged += HandleFuelRemainingChanged;
		
		//Fuel
		fuelGaugeMaxWidth = FuelGauge.foreground.localScale.x;
		resetFuelMeter();
	}

	void HandleGameModeChanged(object sender, EventArgs<Mode> e)
	{
		//Debug.Log("Gamemode is now " + e.Data.ToString() + " == " + game.CurrentMode.ToString());
				
		//show space ship controls HUD
		if (game.CurrentMode == Mode.InGame)
		{
			displayGui(nguiControls);
		}
		else if (game.CurrentMode == Mode.MainMenu)
		{
			displayGui(null);
		}
		
		//show menu
		else if (game.CurrentMode != Mode.InGame)
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
	}
	
	void Update()
	{
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
		resetFuelMeter();
		game.CurrentMode = Mode.InGame;
		if (Application.loadedLevel < 3)
			Application.LoadLevel(Application.loadedLevel+1);
		else
			Application.LoadLevel(3); //3 is scene "LevelN"
	}
	
	public void OnClickRetry()
	{
		Time.timeScale = 1;
		resetFuelMeter();
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
		resetFuelMeter();
		game.CurrentMode = Mode.MainMenu;
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
		if (primary == null)
		{
			NGUITools.SetActive(nguiControls,false);
			NGUITools.SetActive(nguiMenu,false);
		}
		else if (primary == nguiMenu)
		{
			NGUITools.SetActive(nguiControls,false);
			NGUITools.SetActive(nguiMenu,true);
		}
		else
		{
			NGUITools.SetActive(nguiControls,true);
			NGUITools.SetActive(nguiMenu,false);
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
	
	#region [ Fuel ]
	void HandleFuelRemainingChanged (object sender, EventArgs<float> e)
	{
		if (e.Data <= 0.01)
		{
			//disable buttons "Left Thrusters" & "Right Thrusters" otherwise they will still make the thruster audio
			foreach (GameObject hudButton in GameObject.FindGameObjectsWithTag("HudButton"))
				hudButton.SetActive(false);
			this.Lose();
		}
		else
		{
			UpdateFuelMeter(e.Data/fuelMax);
		}
	}
	
	public void UpdateFuelMeter(float toPercent)
	{
		//Debug.Log(toPercent + " of " + fuelGaugeMaxWidth + " in " + FuelGauge.foreground.localScale.ToString());
		
		//Update FuelGauge width
		FuelGauge.foreground.localScale = new Vector3(
			fuelGaugeMaxWidth * toPercent,
			FuelGauge.foreground.localScale.y,
			FuelGauge.foreground.localScale.z);
		
		//Update FuelGauge color
		UISprite sliderSprite = FuelGauge.foreground.GetComponent<UISprite>();		
		if (sliderSprite != null)
		{
			sliderSprite.color = Color.Lerp(Color.red, Color.green, toPercent);
		}
	}
	
	private void resetFuelMeter()
	{
		game.FuelRemaining = fuelMax;
		UpdateFuelMeter(1); //reset FuelGauge
	}
	#endregion [ Fuel ]
}