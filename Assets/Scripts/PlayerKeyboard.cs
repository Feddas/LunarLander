using UnityEngine;
using System.Collections;

public class PlayerKeyboard : MonoBehaviour {
	public ParticleSystem bottomThruster;
	public ParticleSystem leftThruster;
	public ParticleSystem rightThruster;
	
	public GameObject[] shipExplosions;
	
	public GuiInGame Gui;
	
	// Use this for initialization
	void Start ()
	{
		Globals.BottomThruster = bottomThruster;
		Globals.LeftThruster = leftThruster;
		Globals.RightThruster = rightThruster;
		Globals.PlayerShip = rigidbody;
		
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
		int ndxExplosion = Random.Range(0,shipExplosions.Length);
		Instantiate(shipExplosions[ndxExplosion], transform.position, transform.rotation);
		Destroy(gameObject);
		
		Gui.Lose();
	}
}