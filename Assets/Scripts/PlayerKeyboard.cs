using UnityEngine;
using System.Collections;

/// <summary>
/// Component of Level1 scenes root "Ship" gameobject
/// Moves the ship in response to the player pressing the keyboards arrow keys
/// </summary>
public class PlayerKeyboard : MonoBehaviour
{
	private ParticleSystem bottomThruster;
	private ParticleSystem leftThruster;
	private ParticleSystem rightThruster;
	
	void Start ()
	{
		bottomThruster = Globals.BottomThruster;
		leftThruster = Globals.LeftThruster;
		rightThruster = Globals.RightThruster;
	}
	
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
}