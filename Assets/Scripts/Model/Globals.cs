using UnityEngine;
using System.Collections;

public class Globals {
	//the 4 values below are initialized in PlayerKeyboard.Start()
	public static ParticleSystem BottomThruster { get; set; }
	public static ParticleSystem LeftThruster { get; set; }
	public static ParticleSystem RightThruster { get; set; }
	public static Rigidbody PlayerShip { get; set; }
	//public static Transform ShipTransform { get; set; }
	public static bool IsSoundOn { get; set; }
}
