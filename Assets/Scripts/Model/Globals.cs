using UnityEngine;
using System.Collections;

public class Globals {
	//the 4 values below are initialized in PlayerKeyboard.Start()
	public static ParticleSystem BottomThruster { get; set; }
	public static ParticleSystem LeftThruster { get; set; }
	public static ParticleSystem RightThruster { get; set; }
	public static Rigidbody PlayerShip { get; set; }
	//public static Transform ShipTransform { get; set; }
	
	#region [ GameSettings and Settings helper functions ]
	public static Setting GameSettings { get; set; }
	
	public static bool HasSetting(Setting gameSetting)
	{
		return (Globals.GameSettings & gameSetting) == gameSetting;
	}
	
	public static Setting ToggleSetting(Setting toBeToggled)
	{
		bool hasKey = Globals.HasSetting(toBeToggled); //(Globals.GameSettings & toBeToggled) == toBeToggled;
		if (hasKey) //toggle it off
			Globals.GameSettings &= ~toBeToggled;
		else //toggle it on
			Globals.GameSettings |= toBeToggled;
		
		return Globals.GameSettings;
	}
	#endregion [ GameSettings and Settings helper functions ]
}
