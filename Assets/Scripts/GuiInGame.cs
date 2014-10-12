using UnityEngine;
using System.Collections;
using System;
using System.Linq;

/// <summary>
/// Component of Level1 scenes GuiScripts empty gameobject.
/// The parent of the GuiScripts object is persistent across scenes.
/// </summary>
public class GuiInGame : MonoBehaviour
{
    public GameObject panelHud;
    public GameObject panelMenu;
    public GameObject MenuButton1;
    public GameObject MenuButton2;
	public AudioClip WinClip;
	public AudioClip LoseClip;
    public UnityEngine.UI.Text ScoreLevel;
    public UnityEngine.UI.Text ScoreTotal;
    public UnityEngine.UI.Slider FuelGauge;
	
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
		setSoundFromToggle();
		game = Globals.Game;
		game.CurrentModeChanged += HandleGameModeChanged;
		game.FuelRemainingChanged += HandleFuelRemainingChanged;
		
		//Fuel
        fuelGaugeMaxWidth = FuelGauge.value;
		resetFuelMeter();
	}

	void HandleGameModeChanged(object sender, EventArgs<Mode> e)
	{
		//Debug.Log("Gamemode is now " + e.Data.ToString() + " == " + game.CurrentMode.ToString());
				
		//show space ship controls HUD
		if (game.CurrentMode == Mode.InGame)
		{
			displayGui(panelHud);
		}
		else if (game.CurrentMode == Mode.MainMenu)
		{
			displayGui(null);
		}
		
		//show menu
		else if (game.CurrentMode != Mode.InGame)
		{
			displayGui(panelMenu);
            //changeButton2(MenuButton2, toggleSoundLabel);
			if (game.CurrentMode == Mode.Paused)
			{
                changeButton(MenuButton1, "Resume Game", OnClickResume);
			}
			else if (game.CurrentMode == Mode.Win)
			{
                changeButton(MenuButton1, "Next Level", OnClickNextLevel);
			}
			else if (game.CurrentMode == Mode.Lose)
			{
                changeButton(MenuButton1, "Retry Level", OnClickRetry);
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

        //TODO: fix keyboard bug of thrust boost caused by thruster being engaged when pause was pressed
	}
	
	public void OnClickNextLevel()
    {
        game.CurrentModeChanged -= HandleGameModeChanged;
        game.FuelRemainingChanged -= HandleFuelRemainingChanged;

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
        game.CurrentModeChanged -= HandleGameModeChanged;
        game.FuelRemainingChanged -= HandleFuelRemainingChanged;

		Time.timeScale = 1;
		resetFuelMeter();
		game.CurrentMode = Mode.InGame;
		Application.LoadLevel(Application.loadedLevel);
	}
	
	public void OnClickToggleSound()
	{
		Globals.IsSoundOn = !Globals.IsSoundOn;
		changeButton(this.MenuButton2, toggleSoundLabel);
		setSoundFromToggle();
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

        if (Application.loadedLevel < 3) //3 is scene "LevelN"
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
	
    /// <summary> shows only the uiToShow. Hiding all other Ui's </summary>
	private void displayGui(GameObject uiToShow)
	{
		if (uiToShow == null)
		{
            panelHud.SetActive(false);
            panelMenu.SetActive(false);
		}
		else if (uiToShow == panelMenu)
		{
            panelHud.SetActive(false);
            panelMenu.SetActive(true);
		}
		else
        {
            panelHud.SetActive(true);
            panelMenu.SetActive(false);
		}
	}

    private void changeButton(GameObject button, string text, UnityEngine.Events.UnityAction action = null)
    {
        var label = button.GetComponentInChildren<UnityEngine.UI.Text>();
        label.text = text;

        if (action != null)
        {
            var buttonScript = button.GetComponent<UnityEngine.UI.Button>();
            buttonScript.onClick.AddListener(action);
        }
    }
	
	private void setSoundFromToggle()
	{
		bool isSoundOn = Globals.IsSoundOn;
				
		//toggle thruster sounds
        // TODO: replace NGUI code below with Unity4.6 code
        //var buttonsWithSound = nguiControls.transform.GetComponentsInChildren<UIButtonSound>();
        //foreach (UIButtonSound buttonSound in buttonsWithSound) {
        //    buttonSound.enabled = isSoundOn;
        //}
		
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
        FuelGauge.value = toPercent;
		
        //Update FuelGauge color
        var fill = FuelGauge.GetComponentsInChildren<UnityEngine.UI.Image>()
            .FirstOrDefault(t => t.name == "Fill");
        if (fill != null)
        {
            fill.color = Color.Lerp(Color.red, Color.green, toPercent);
        }
	}
	
	private void resetFuelMeter()
	{
		game.FuelRemaining = fuelMax;
		UpdateFuelMeter(1); //reset FuelGauge
	}
	#endregion [ Fuel ]
}