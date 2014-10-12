using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Game modes the can occur during play.
/// </summary>
public enum Mode
{
    Undefined,
    InGame,
    Paused,
    Win,
    Lose,
    MainMenu,
}

/// <summary>
/// Tracks the state of the game mode
/// </summary>
public class State
{
    #region [ Mode ]
    private Mode currentMode;
    public Mode CurrentMode
    {
        get
        {
            return this.currentMode;
        }
        set
        {
            if (this.currentMode == value)
                return;

            this.currentMode = value;
            EventArgs<Mode> eventArgs = new EventArgs<Mode>(this.currentMode);
            this.OnCurrentModeChanged(this, eventArgs);
        }
    }

    public event EventHandler<EventArgs<Mode>> CurrentModeChanged;
    private void OnCurrentModeChanged(object sender, EventArgs<Mode> e)
    {
        if (this.CurrentModeChanged != null)
        {
            this.CurrentModeChanged(sender, e);
        }
    }
    #endregion [ Mode ]

    #region [ IsThrustersOn ]
    public const float FuelMinimum = 0.01f;

    private float fuelRemaining;
    public float FuelRemaining
    {
        get
        {
            return this.fuelRemaining;
        }
        set
        {
            if (this.fuelRemaining == value)
                return;
            else if (this.fuelRemaining <= 0) //on initilzation, don't raise the event
            {
                this.fuelRemaining = value;
                return;
            }

            this.fuelRemaining = value;
            EventArgs<float> eventArgs = new EventArgs<float>(this.fuelRemaining);
            this.OnFuelRemainingChanged(this, eventArgs);
        }
    }

    public event EventHandler<EventArgs<float>> FuelRemainingChanged;
    private void OnFuelRemainingChanged(object sender, EventArgs<float> e)
    {
        if (this.FuelRemainingChanged != null)
        {
            this.FuelRemainingChanged(sender, e);
        }
    }
    #endregion [ IsThrustersOn ]

    public State(Mode startState = Mode.Undefined)
    {
        currentMode = startState;
    }
}