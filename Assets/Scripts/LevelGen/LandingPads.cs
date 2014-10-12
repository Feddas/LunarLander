using UnityEngine;
using System.Collections;

/// <summary>
/// Component of LevelN scenes LevelNScripts empty gameobject.
/// Landing pads.
/// </summary>
public class LandingPads : MonoBehaviour
{
    public GameObject LandingPad;

    // Use this for initialization
    void Start()
    {
        var pad1 = Instantiate(LandingPad) as GameObject;
        pad1.transform.position = new Vector3(
            Random.Range(-10, 0),
            Random.Range(-10, -2),
            0);

        var pad2 = Instantiate(LandingPad) as GameObject;
        pad2.transform.position = new Vector3(
            Random.Range(0, 10),
            Random.Range(-10, -2),
            0);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
