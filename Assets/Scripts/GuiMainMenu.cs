using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Component of MainMenu scenes Gui empty gameobject
/// Menu options:
/// 1. NewGame (inGameChangesTo)=> Resume/Retry/NextLevel
/// 2. ToggleSound
/// 3. ContinueGame (inGameChangesTo)=> QuitToMainMenu
/// 4. Quit
/// </summary>
//[ExecuteInEditMode]
public class GuiMainMenu : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = 20;
    }

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