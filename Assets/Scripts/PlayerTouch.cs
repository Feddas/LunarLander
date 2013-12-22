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
	
	private bool isPressed;
	
	/// <summary>
	/// This method is called by NGUI's UIButton.cs click event handler.
	/// </summary>
	void OnPress(bool isDown)
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
		}
	}
}