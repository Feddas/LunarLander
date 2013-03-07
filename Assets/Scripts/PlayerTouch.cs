using UnityEngine;
using System.Collections;

public class PlayerTouch : MonoBehaviour {	
    //file:///C:/Apps/Unity/Editor/Data/Documentation/Documentation/Components/gui-Layout.html
    /// <summary> default to the bottom third of the screen </summary>
    public Rect button = new Rect(0, 0, 100, 100);
    public PlayerMoveEnum choice = PlayerMoveEnum.Undetermined;
	
    private void OnGUI()
    {
        GUI.skin.button.fontSize = (Screen.height+Screen.width)/40;
        if (button == new Rect(0, 0, 100, 100))
            button = createButton(choice);
		
        if (GUI.RepeatButton(button, choice.ToString()))
		{
			if (Globals.BottomThruster != null
				&& Globals.LeftThruster != null
				&& Globals.RightThruster != null
				&& Globals.PlayerShip != null)
			{
				Thrusters thrusters = new Thrusters()
				{
					ThrusterBottom = Globals.BottomThruster,
					ThrusterLeft = Globals.LeftThruster,
					ThrusterRight = Globals.RightThruster,
				};
				Globals.PlayerShip.AddForce(thrusters.DoThrust(choice));
	            print(choice.ToString() + " fired");
			}
		}
    }

    private Rect createButton(PlayerMoveEnum choice)
    {
        float margin = 1;
        float left;
        float top = margin;
        float width = Screen.width / 2 - (2 * margin);
        float height = Screen.height - (2 * margin);
        switch (choice)
        {
			//2 buttons, one on the left the other on the right
            case PlayerMoveEnum.LeftBottomThruster:
                left = 0 + margin;
                return new Rect(left, top, width, height);
            case PlayerMoveEnum.RightBottomThruster:
                left = Screen.width / 2 + margin;
                return new Rect(left, top, width, height);
			
			//All other types of movement are not handled
            case PlayerMoveEnum.LeftThruster:
            case PlayerMoveEnum.RightThruster:
            case PlayerMoveEnum.BottomThruster:
            case PlayerMoveEnum.Undetermined:
            default:
                return button;
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