using UnityEngine;
using System.Collections;

public class PlayerKeyboard : MonoBehaviour {
	public ParticleSystem bottomThruster;
	public ParticleSystem leftThruster;
	public ParticleSystem rightThruster;
	public AudioClip SoundExplosion;
	
	public GameObject[] shipExplosions;
	
	public GuiInGame Gui;
	
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
	
	// Use this for initialization
	void Start ()
	{
		//Automatically populate the Gui variable with the Gui script attached to the EmptyObject Gui using its Gui Tag
		Gui = GameObject.FindWithTag("Gui").GetComponent(typeof(GuiInGame)) as GuiInGame;
	}
	
	// Update is called once per frame
	private void Update ()
	{
	    if(Input.GetAxis("Horizontal") > 0) //right
		{
			leftThruster.Emit(1);
			rightThruster.Emit(0);
			rigidbody.AddForce(10,0,0);
		}
	    if(Input.GetAxis("Horizontal") < 0) //left
		{
			rightThruster.Emit(1);
			leftThruster.Emit(0);
			rigidbody.AddForce(-10,0,0);
		}
	    if(Input.GetAxis("Horizontal") == 0)
		{
			leftThruster.Emit(0);
			rightThruster.Emit(0);
		}
		
	    if(Input.GetAxis("Vertical") > 0) //up
		{
			bottomThruster.Emit(1);
			rigidbody.AddForce(0,30,0);
		}
	    if(Input.GetAxis("Vertical") < 0) //down
		{
			rigidbody.AddForce(0,-10,0);
		}
	    if(Input.GetAxis("Vertical") == 0)
		{
			bottomThruster.Emit(0);
		}
		
		HandleAudio();
	}
	
	public void HandleAudio(bool playAudio = false)
	{
		if (Globals.IsSoundOn == false)
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
	
	private void explode()
	{
		if (Globals.IsSoundOn)
			AudioSource.PlayClipAtPoint(SoundExplosion, new Vector3(0,0));
		
		int ndxExplosion = Random.Range(0,shipExplosions.Length);
		Instantiate(shipExplosions[ndxExplosion], transform.position, transform.rotation);
		Destroy(gameObject);
		
		Gui.Lose();
	}
}