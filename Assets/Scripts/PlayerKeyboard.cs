using UnityEngine;
using System.Collections;

public class PlayerKeyboard : MonoBehaviour {
	public ParticleSystem bottomThruster;
	public ParticleSystem leftThruster;
	public ParticleSystem rightThruster;
	public AudioClip SoundExplosion;
	public GameObject[] shipExplosions;
	public GuiInGame Gui;
	
	private Thrusters thrusters;
	
	#region [ Overriden functions ]
	/// <summary>
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
		//Globals.ShipTransform = transform;
	}
	
	void Start ()
	{
		thrusters = new Thrusters();
		
		//Automatically populate the Gui variable with the Gui script attached to the EmptyObject Gui using its Gui Tag
		Gui = GameObject.FindWithTag("Gui").GetComponent(typeof(GuiInGame)) as GuiInGame;
	}
	
	private void Update ()
	{
		thrusters.ThrustShipOn(determineDirection());
		
		handleAudio();
	}
	
	private void OnCollisionEnter(Collision hitInfo)
	{
		print(hitInfo.relativeVelocity.magnitude);
		if(hitInfo.relativeVelocity.magnitude > 4)
		{
			explode();
		}
		else if(hitInfo.gameObject.tag == "LandingPad")
		{
			LandingPad landingPad;
			landingPad = hitInfo.gameObject.GetComponent("LandingPad") as LandingPad;
			landingPad.Activate();
		}
		else if(hitInfo.relativeVelocity.magnitude > 2)
		{
			explode();
		}
	}
	#endregion [ Overriden functions ]
	
	#region [ private functions ]
	private PlayerMoveEnum determineDirection()
	{
		PlayerMoveEnum choice;
		
	    if(Input.GetAxis("Horizontal") > 0) //right
		{
			choice = PlayerMoveEnum.LeftThruster;
		}
	    else if(Input.GetAxis("Horizontal") < 0) //left
		{
			choice = PlayerMoveEnum.RightThruster;
		}
	    else
		{
			choice = PlayerMoveEnum.Undetermined;
		}
		
	    if(Input.GetAxis("Vertical") > 0) //up
		{
			switch (choice)
			{
			case PlayerMoveEnum.RightThruster:
				choice = PlayerMoveEnum.RightBottomThruster;
				break;
			case PlayerMoveEnum.LeftThruster:
				choice = PlayerMoveEnum.LeftBottomThruster;
				break;
			default:
				choice = PlayerMoveEnum.BottomThruster;
				break;
			}
		}
		
		return choice;
	}
	
	private void handleAudio(bool playAudio = false)
	{
		if (Globals.HasSetting(Setting.IsSoundOn) == false)
			return;
		
		if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0 || playAudio)
		{
			if(audio.isPlaying == false)
			{
				audio.Play();
			}
		}
		else if(audio.isPlaying)
		{
			audio.Stop();
		}
	}
	
	private void explode()
	{
		if (Globals.HasSetting(Setting.IsSoundOn))
			AudioSource.PlayClipAtPoint(SoundExplosion, new Vector3(0,0));
		
		int ndxExplosion = Random.Range(0,shipExplosions.Length);
		Instantiate(shipExplosions[ndxExplosion], transform.position, transform.rotation);
		Destroy(gameObject);
		
		Gui.Lose();
	}
	#endregion [ private functions ]
}