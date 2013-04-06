using UnityEngine;
using System.Collections;

public class PlayerTouch : MonoBehaviour {
    /// <summary> default to the bottom third of the screen </summary>
    public PlayerMoveEnum choice = PlayerMoveEnum.Undetermined;
	
	private Thrusters thrusters;
	
	void Start ()
	{
		thrusters = new Thrusters();
	}
	
    private void OnGUI()
    {
    }
	
	bool isPressed;
	//int count = 0;
	void OnPress(bool isDown)
	{
		isPressed = isDown;
		
//		if (isPressed == false)
//			count = 0;
	}
	
	void Update()
	{
		if (isPressed)
		{
			thrusters.ThrustShipOn(choice);
            //print(choice.ToString() + " fire #" + count++);
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