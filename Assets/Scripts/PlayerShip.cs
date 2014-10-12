using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Component of Level1 scenes root "Ship" gameobject
/// - initializes ship and thruster globals
/// - determines successful landing or crash
/// - plays thruster and explosion audio
/// </summary>
public class PlayerShip : MonoBehaviour
{
    public ParticleSystem bottomThruster;
    public ParticleSystem leftThruster;
    public ParticleSystem rightThruster;
    public AudioClip SoundExplosion;

    public GameObject[] shipExplosions;

    /// <summary> Provides hooks into the GuiInGame Lose() method. </summary>
    public GuiInGame Gui;

    /// <summary>
    /// Use this ship to initialize global variables.
    /// Awake() runs before Start() allowing the Globals to be referenced in Start()
    /// which happens in GuiInGame.start().toggleSound()
    /// http://www.richardfine.co.uk/junk/unity%20lifetime.png
    /// </summary>
    void Awake()
    {
        Globals.BottomThruster = bottomThruster;
        Globals.LeftThruster = leftThruster;
        Globals.RightThruster = rightThruster;
        Globals.PlayerShip = rigidbody;
    }

    // Use this for initialization
    void Start()
    {
        //Automatically populate the Gui variable with the Gui script attached to the EmptyObject Gui using its Gui Tag
        Gui = GameObject.FindWithTag("Gui").GetComponent(typeof(GuiInGame)) as GuiInGame;
        totalLandingPads = (GameObject.FindGameObjectsWithTag("LandingPad") as GameObject[]).Length;
    }

    // Update is called once per frame
    private void Update()
    {
        HandleAudio();
    }

    public void HandleAudio()
    {
        if (Globals.IsSoundOn == false)
            return;

        bool keyboardThrustersOn = PlayerKeyboard.IsKeyboardThrustersOn;
        if (keyboardThrustersOn && Thrusters.HaveFuel)
        {
            if (audio.isPlaying == false)
            {
                audio.Play();
            }
        }
        else if (audio.isPlaying)
        {
            audio.Stop();
        }
    }

    private void OnCollisionEnter(Collision hitInfo)
    {
        if (hitInfo.relativeVelocity.magnitude > 4)
        {
            explode(hitInfo.relativeVelocity.magnitude);
        }
        else if (hitInfo.gameObject.tag == "LandingPad")
        {
            LandingPad landingPad;
            landingPad = hitInfo.gameObject.GetComponent("LandingPad") as LandingPad;
            landingPad.Activate();
        }
        else if (hitInfo.relativeVelocity.magnitude > 2)
        {
            explode(hitInfo.relativeVelocity.magnitude);
        }
    }

    #region [ Lose ]
    private void explode(float magnitude)
    {
        Debug.Log(magnitude);
        if (Globals.IsSoundOn)
            AudioSource.PlayClipAtPoint(SoundExplosion, new Vector3(0, 0));

        int ndxExplosion = UnityEngine.Random.Range(0, shipExplosions.Length);
        Instantiate(shipExplosions[ndxExplosion], transform.position, transform.rotation);
        Destroy(gameObject);

        numActivated = 0;
        Gui.Lose();
    }
    #endregion [ Lose ]

    #region [ Win ]
    private int numActivated, totalLandingPads;
    public void LandingPadActivated()
    {
        numActivated++;
        Gui.UpdateScore(numActivated);
        if (numActivated == totalLandingPads)
        {
            Gui.Win(numActivated);
        }
    }
    #endregion [ Win ]
}