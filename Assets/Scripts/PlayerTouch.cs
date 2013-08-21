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
	
    private void OnGUI()
    {
    }
	
	bool isPressed;
	
	/// <summary>
	/// This method is called by NGUI's UIButton.cs click event handler.
	/// </summary>
	void OnPress(bool isDown)
	{
		isPressed = isDown;
	}
	
	void Update()
	{
		if (isPressed
		    && Globals.BottomThruster != null
			&& Globals.LeftThruster != null
			&& Globals.RightThruster != null
			&& Globals.PlayerShip != null)
		{
			//determine how to move the ship
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

public enum PlayerMoveEnum
{
	Undetermined,
	LeftThruster,
	RightThruster,
	LeftBottomThruster,
	BottomThruster,
	RightBottomThruster,
}