using UnityEngine;
using System.Collections;

public class Thrusters {
	public ParticleSystem ThrusterBottom { get; set; }
	public ParticleSystem ThrusterLeft { get; set; }
	public ParticleSystem ThrusterRight { get; set; }
	
	//public Rigidbody ObjToBeThrust { get; set; }
	
	public Thrusters()
	{
	}
	
	public Vector3 DoThrust(PlayerMoveEnum direction)
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
