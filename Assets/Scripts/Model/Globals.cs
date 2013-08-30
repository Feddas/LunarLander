using UnityEngine;
using System.Collections;

public class Globals
{
	//the 4 values below are initialized in PlayerShip.Awake()
	public static ParticleSystem BottomThruster { get; set; }
	public static ParticleSystem LeftThruster { get; set; }
	public static ParticleSystem RightThruster { get; set; }
	public static Rigidbody PlayerShip { get; set; }
	
	public static bool IsSoundOn { get; set; }
	
	private static State game;
	/// <summary>
	/// Uses the State class as a singleton to reference a single state for the life of the game.
	/// </summary>
	public static State Game
	{ get
		{
			if (game == null)
				game = new State(Mode.InGame);
			return game;
		}
		set
		{
			game = value;
		}
	}
}
