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

            if (Globals.IsSoundOn && clipWhilePressed != null && clipWhilePressed.isReadyToPlay)
                PlaySound(clipWhilePressed);
        }
        else
        {
            //TODO: turn off thruster audio
        }
    }

    /// <summary> first component found that is able to play sounds </summary>
    private static AudioListener mListener;
    private static System.DateTime timeLastClipEnds;

    /// <summary>
    /// Play the specified audio clip with the specified volume and pitch.
    /// PlaySound was copied from NGUI
    /// </summary>
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

            if (System.DateTime.Now > PlayerTouch.timeLastClipEnds // wait for last clip to play through
                && mListener != null && mListener.enabled && mListener.gameObject.activeInHierarchy)
            {
                PlayerTouch.timeLastClipEnds = System.DateTime.Now.AddSeconds(clip.length);
                AudioSource source = mListener.GetComponent<AudioSource>();
                if (source == null) source = mListener.gameObject.AddComponent<AudioSource>();
                source.pitch = pitch;
                source.PlayOneShot(clip, volume);
                return source;
            }
        }
        return null;
    }
}