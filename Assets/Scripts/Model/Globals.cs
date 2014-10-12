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
    /// Make sure this singleton is reset when the game is reset otherwise there
    /// is "object has been destroyed, but still trying to access it." error: http://forum.unity3d.com/threads/197142-Application-LoadLevel()-and-a-quot-Singleton-quot-type-approach
    /// </summary>
    public static State Game
    {
        get
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
