using UnityEngine;
using System.Collections;
using System.Text;
using System;

public class MessageWindowTestInterface : MonoBehaviour {

	public GUISkin consoleSkin;
	MessageWindow consoleWindow;
	public Vector2 consoleSize = new Vector2(500, 300);
	Vector2 consolePosition = new Vector2(0,0);
	Rect consoleRect;

	// Use this for initialization
	void Start () {
		consolePosition.y = Screen.height - consoleSize.y;

		consoleRect = new Rect(consolePosition.x, consolePosition.y, consoleSize.x, consoleSize.y);
		consoleWindow = new MessageWindow(consoleSize, consoleSkin);

		consoleWindow.AddMessage(
			new MessageWindow.StringMessageItem(20, 
		                         "This is an extra long message. This will show the messgaes will" + 
		                         " simply wrap around and the message below will be placed right after," + 
		                         " despite the wrapping"));

		consoleWindow.AddMessage(
			new MessageWindow.StringMessageItem(20,
		                         "We can also force a line break with \\n" +
		                         " anywhere we \n want \n the \n breaks to be."));

		consoleWindow.AddMessage(
			new MessageWindow.StringMessageItem(20,
		                                    "Press Space to add more messages!"));
	}
	
	// Update is called once per frame
	void Update () {
		//Only count down timers when we're not moused over the console window
		if(!GUIUtils.MouseOverRect(consoleRect)) {
			consoleWindow.pauseAutoScroll = false;
			consoleWindow.CountDownTimers();
		} else {
			consoleWindow.pauseAutoScroll = true;
		}


		//Mock up some triggers or events happening in game that would display messages
		if(Input.GetKeyDown(KeyCode.Space)) {
			consoleWindow.AddMessage(30, GUIUtils.MakeRandomString(UnityEngine.Random.Range(3, 15)));
		}
		if(Input.GetKeyDown(KeyCode.P)) {
			Vector3 mousePosition = Input.mousePosition;
			mousePosition.z = 15; //depth of objects into scene
			consoleWindow.AddMessage(
				new MessageWindow.LocationMessageItem(30,
			                                      "Location event!",
			                                      Camera.main.ScreenToWorldPoint(mousePosition),
			                                      OnLocationClicked));
		}
	}

	void OnLocationClicked(MessageWindow.MessageItem item) {
		Vector3 messageLocation = ((MessageWindow.LocationMessageItem)item).GetLocation();
		Camera.main.transform.position = new Vector3(messageLocation.x,
		                                             Camera.main.transform.position.y,
		                                             messageLocation.z);
	}

	void OnGUI() {
		consoleWindow.Draw(consolePosition);
	}
	
}
