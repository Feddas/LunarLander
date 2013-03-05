using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public class GuiLabels : MonoBehaviour {
	private bool firstRun = true;
	private GUIStyle myStyl;
	private int lblWidth, lblHeight, lblLeft, lblTop;
	
    private void OnGUI()
    {
		if (firstRun)
		{
			myStyl = new GUIStyle();
			myStyl.normal.textColor = GUI.skin.label.normal.textColor;
			myStyl.fontSize = (Screen.height+Screen.width)/40;
			myStyl.alignment = TextAnchor.MiddleCenter;
			
			lblWidth = Screen.width / 2;
			lblHeight = Screen.height / 2;
			lblLeft = Screen.width / 2 - lblWidth / 2;
			lblTop = Screen.height / 2 - lblHeight / 2;
			firstRun = false;
		}
		
        GUILayout.BeginArea(new Rect(lblLeft, lblTop, lblWidth, lblHeight));
        GUILayout.Label("Lunar Lander", myStyl);
        GUILayout.EndArea();
    }
}
