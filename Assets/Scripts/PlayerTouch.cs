using UnityEngine;
using System.Collections;

/// <summary>
/// Component of Level1 scenes "Touch for Left/Right Thrusters" buttons
/// </summary>
public class PlayerTouch : MonoBehaviour
{
    /// <summary>
    /// Players choice in ships movement direction.
    /// </summary>
    public PlayerMoveEnum choice = PlayerMoveEnum.Undetermined;
    public AudioClip clipWhilePressed;

    private bool isPressed;

    /// <summary>
    /// This method is called by NGUI's UIButton.cs click event handler.
    /// </summary>
    public void OnPress(bool isDown)
    {
        isPressed = isDown;
    }

    void HandleGameModeChanged(object sender, EventArgs<Mode> e)
    {
        //ensure thrusters are released when the game mode changes
        if (Globals.Game.CurrentMode != Mode.InGame && isPressed)
        {
            isPressed = false;

            //this game object is persisted even after a win, so do not dereference the handler as commented out below.
            //Globals.Game.CurrentModeChanged -= HandleGameModeChanged;
        }
    }

    void Start()
    {
        Globals.Game.CurrentModeChanged += HandleGameModeChanged;
    }

    void Update()
    {
        if (isPressed && Thrusters.OfShipInitialized)
        {
            //determine how much to move in choice direction
            Thrusters thrusters = new Thrusters()
            {
                ThrusterBottom = Globals.BottomThruster,
                ThrusterLeft = Globals.LeftThruster,
                ThrusterRight = Globals.RightThruster,
            };
            Vector3 shipForce = thrusters.ThrustOn(choice);

            //move the ship
            Globals.PlayerShip.AddForce(shipForce);

            if (clipWhilePressed != null && clipWhilePressed.isReadyToPlay)
                PlaySound(clipWhilePressed);
        }
        else
        {
            //TODO: turn off thruster audio
        }
    }


    private static AudioListener mListener { get; set; }
    /// <summary>
    /// Play the specified audio clip with the specified volume and pitch.
    /// </summary>

    /// PlaySound was copied from NGUI
    static public AudioSource PlaySound(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        volume *= 1f;

        if (clip != null && volume > 0.01f)
        {
            if (mListener == null)
            {
                mListener = GameObject.FindObjectOfType(typeof(AudioListener)) as AudioListener;

                if (mListener == null)
                {
                    Camera cam = Camera.main;
                    if (cam == null) cam = GameObject.FindObjectOfType(typeof(Camera)) as Camera;
                    if (cam != null) mListener = cam.gameObject.AddComponent<AudioListener>();
                }
            }

            if (mListener != null && mListener.enabled && mListener.gameObject.activeInHierarchy)
            {
                AudioSource source = mListener.audio;
                if (source == null) source = mListener.gameObject.AddComponent<AudioSource>();
                source.pitch = pitch;
                source.PlayOneShot(clip, volume);
                return source;
            }
        }
        return null;
    }
}