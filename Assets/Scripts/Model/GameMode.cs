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
}

/// <summary>
/// Tracks the state of the game mode
/// </summary>
public class State
{	
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
	
	public State(Mode startState = Mode.Undefined)
	{
		currentMode = startState;
	}
	
	public event EventHandler<EventArgs<Mode>> CurrentModeChanged;
	private void OnCurrentModeChanged(object sender, EventArgs<Mode> e)
	{
		if (this.CurrentModeChanged != null)
		{
			this.CurrentModeChanged(sender, e);
		}
	}
}