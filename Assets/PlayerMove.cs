using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {
	
    //file:///C:/Apps/Unity/Editor/Data/Documentation/Documentation/Components/gui-Layout.html
    /// <summary> default to the bottom third of the screen </summary>
    public Rect button = new Rect(0, 0, 100, 100);
    public PlayerMoveEnum choice = PlayerMoveEnum.Undetermined;
	
    private void OnGUI()
    {
		GUI.skin.button.fontSize = (Screen.height+Screen.width)/40;
        if (button == new Rect(0, 0, 100, 100))
            button = createButton(choice);
		
        if (GUI.Button(button, choice.ToString()))
            print("success");//ExecuteChoice.Instance.DropInChoice(choice);
    }

    private Rect createButton(PlayerMoveEnum choice)
    {
        float margin = 1;
        float left;
        float top = Screen.height / 2 + margin;
        float width = Screen.width / 3 - (2 * margin);
        float height = Screen.height / 2 - (2 * margin);
        switch (choice)
        {
			//top row of buttons
            case PlayerMoveEnum.LeftThruster:
                left = 0 + margin;
                return new Rect(left, margin, width, height);
            case PlayerMoveEnum.RightThruster:
                left = (Screen.width * 2) / 3 + margin;
                return new Rect(left, margin, width, height);
			
			//bottom row of buttons
            case PlayerMoveEnum.LeftBottomThruster:
                left = 0 + margin;
                return new Rect(left, top, width, height);
            case PlayerMoveEnum.BottomThruster:
                left = Screen.width / 3 + margin;
                return new Rect(left, top, width, height);
            case PlayerMoveEnum.RightBottomThruster:
                left = (Screen.width * 2) / 3 + margin;
                return new Rect(left, top, width, height);
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