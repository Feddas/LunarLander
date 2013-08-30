using UnityEngine;
using System.Collections;

/// <summary>
/// Component of Level1 scenes root "Ship" gameobject
/// Moves the ship in response to the player pressing the keyboards arrow keys
/// </summary>
public class PlayerKeyboard : MonoBehaviour
{
	private void Start ()
	{
	}
	
	public static bool IsKeyboardThrustersOn
	{
		get 
		{
			return Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
		}
	}
	
	private void Update ()
	{
		bool isPressed = PlayerKeyboard.IsKeyboardThrustersOn;
		if (isPressed && Thrusters.OfShipInitialized)
		{
			//determine how to move the ship
			PlayerMoveEnum choice = PlayerMoveEnum.Undetermined;
			if (Input.GetAxis("Horizontal") > 0) //right
			{
				choice = PlayerMoveEnum.LeftThruster;
			}
			else if (Input.GetAxis("Horizontal") < 0) //left
			{
				choice = PlayerMoveEnum.RightThruster;
			}
			if (Input.GetAxis("Vertical") > 0) //up
			{
				//retain horizontal movement in choice
				choice = (PlayerMoveEnum)((int)PlayerMoveEnum.BottomThruster + (int)choice);
			}
			
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
		}
	}
}