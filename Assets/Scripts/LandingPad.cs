using UnityEngine;
using System.Collections;

/// <summary>
/// Component of the prefabs LandingPad gameobject
/// </summary>
public class LandingPad : MonoBehaviour
{
    public Light stationLight;
    public Material onMaterial;
    public Color onColor;
    public PlayerShip shipScriptInstance;

    // Use this for initialization
    void Start()
    {
        if (Globals.PlayerShip != null)
        {
            shipScriptInstance = Globals.PlayerShip.GetComponent(typeof(PlayerShip)) as PlayerShip;
        }
        else
        {
            //populate the shipScriptInstance variable with the PlayerShip script attached to the Ship using its "Player" Tag
            shipScriptInstance = GameObject.FindWithTag("Player").GetComponent(typeof(PlayerShip)) as PlayerShip;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Activate()
    {
        if (Globals.IsSoundOn)
            GetComponent<AudioSource>().Play();

        if (stationLight.color != onColor)
        {
            Debug.Log(gameObject.name + " has been activated");
            GetComponent<Renderer>().material = onMaterial;
            stationLight.color = onColor;

            shipScriptInstance.LandingPadActivated();
        }
    }
}