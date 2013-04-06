using UnityEngine;
using System.Collections;
using System;

public class Thrusters {
	public ParticleSystem ThrusterBottom { get; set; }
	public ParticleSystem ThrusterLeft { get; set; }
	public ParticleSystem ThrusterRight { get; set; }
	
	public Thrusters()
	{
		if (Globals.BottomThruster != null
			&& Globals.LeftThruster != null
			&& Globals.RightThruster != null
			&& Globals.PlayerShip != null)
		{
			ThrusterBottom = Globals.BottomThruster;
			ThrusterLeft = Globals.LeftThruster;
			ThrusterRight = Globals.RightThruster;
		}
		else
		{
			throw new Exception("All Globals.Thrusters need to be set before Thrusters can be instantiated");
		}
	}
	
	public void ThrustShipOn(PlayerMoveEnum activeThrusters)
	{
		if (Globals.PlayerShip != null)
		{
			Vector3 shipForce = getThrust(activeThrusters);
			Globals.PlayerShip.AddForce(shipForce);
		}
		else
		{
			throw new Exception("Globals.PlayerShip need to be set before ThrustShipOn() can be used");
		}
	}
	
	private Vector3 getThrust(PlayerMoveEnum direction)
	{
		float sideThrust = 600 * Time.deltaTime;
		float bottomThrust = 2000 * Time.deltaTime;
		switch (direction)
		{
			case PlayerMoveEnum.LeftThruster:
				ThrusterBottom.Emit(0);
				ThrusterLeft.Emit(1);
				ThrusterRight.Emit(0);
				return new Vector3(sideThrust,0,0);
			case PlayerMoveEnum.RightThruster:
				ThrusterBottom.Emit(0);
				ThrusterLeft.Emit(0);
				ThrusterRight.Emit(1);
				return new Vector3(-sideThrust,0,0);
			case PlayerMoveEnum.LeftBottomThruster:
				ThrusterBottom.Emit(1);
				ThrusterLeft.Emit(1);
				ThrusterRight.Emit(0);
				return new Vector3(sideThrust,bottomThrust,0);
			case PlayerMoveEnum.BottomThruster:
				ThrusterBottom.Emit(1);
				ThrusterLeft.Emit(0);
				ThrusterRight.Emit(0);
				return new Vector3(0,30,0);
			case PlayerMoveEnum.RightBottomThruster:
				ThrusterBottom.Emit(1);
				ThrusterLeft.Emit(0);
				ThrusterRight.Emit(1);
				return new Vector3(-sideThrust,bottomThrust,0);
			case PlayerMoveEnum.Undetermined:
			default:
				ThrusterBottom.Emit(0);
				ThrusterLeft.Emit(0);
				ThrusterRight.Emit(0);
				return new Vector3(0,0,0);
		}
	}
}
