using UnityEngine;
using System.Collections;
using System.Net;
using System.Text.RegularExpressions;

[RequireComponent (typeof (ServerHostAndJoin))]
public class ManualConnection : MonoBehaviour {

	string ip = "";
	string port = "";
	string password = "";
	public Vector2 position = new Vector2(0,0);
	public Vector2 connectionInfoPosition = new Vector2(0,0);

	//Regex to match an IP address or partial valid IP address
	string ipPattern = @"^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)|\.){0,3}$";
	Regex ipRegEx;

	//Regex to match a port or partial valid port
	string portPattern = @"^([0-9]{1,4}|[1-5][0-9]{4}|6[0-4][0-9]{3}|65[0-4][0-9]{2}|655[0-2][0-9]|6553[0-5])$";
	Regex portRegEx;

	public delegate void ManualSettingsInput(string ip, int port, string password);
	public event ManualSettingsInput OnManualInput;

	public bool connecting = false;
	public bool showConnectionInfo = false;

	void Start() {
		ipRegEx = new Regex(ipPattern);
		portRegEx = new Regex(portPattern);
	}

	void Update() {

	}

	string ValidateIP(string input) {
		//Allow full erase of string
		if(input.Length < 1)
			return input;

		//See if we're inline with a IP address
		if(ipRegEx.IsMatch(input)) {
			return input;
		} else {
			return ip;
		}
	}

	string ValidatePort(string input) {
		//Allow full erase of string
		if(input.Length < 1)
			return input;
		
		//See if we're inline with a IP address
		if(portRegEx.IsMatch(input)) {
			return input;
		} else {
			return port;
		}
	}

	void TryConnect(string ip, string port, string password) {
		IPAddress tempIP;
		//One final check on the IP
		if(!string.IsNullOrEmpty(ip) && IPAddress.TryParse(ip, out tempIP)) {
			OnManualInput(ip, int.Parse(port), password);
		} else {
			//Be mean, clear the IP if they get it wrong.
			this.ip = "";
		}
	}

	void ShowConnectionInfo(string IP, int port) {
		if(IP.Equals ("UNASSIGNED_SYSTEM_ADDRESS")) {
			Vector2 errorLabelPosition = connectionInfoPosition;
			GUIContent errorLabel = new GUIContent("Start a server to see external IP and port");
			Vector2 errorLabelSize = GUI.skin.label.CalcSize(errorLabel);
			GUI.Label(new Rect(errorLabelPosition.x, errorLabelPosition.y,
			                   errorLabelSize.x, errorLabelSize.y), errorLabel);
		} else {
			Vector2 ipLabelPosition = connectionInfoPosition;
			GUIContent ipLabel = new GUIContent("Server IP: " + IP);
			Vector2 ipLabelSize = GUI.skin.label.CalcSize(ipLabel);
			GUI.Label(new Rect(ipLabelPosition.x, ipLabelPosition.y, ipLabelSize.x, ipLabelSize.y), ipLabel);
			
			Vector2 portLabelPosition = new Vector2(ipLabelPosition.x, ipLabelPosition.y + ipLabelSize.y);
			GUIContent portLabel = new GUIContent("Server Port: " + port);
			Vector2 portLabelSize = GUI.skin.label.CalcSize(portLabel);
			GUI.Label(new Rect(portLabelPosition.x, portLabelPosition.y,
			                   portLabelSize.x, portLabelSize.y), portLabel);
		}
	}

	void OnGUI() {
		if(showConnectionInfo) {
			ShowConnectionInfo(Network.player.externalIP, Network.player.externalPort);
		}
		if(connecting) {
			Vector2 ipLabelPosition = position;
			GUIContent ipLabel = new GUIContent("Server IP: ");
			Vector2 ipLabelSize = GUI.skin.label.CalcSize(ipLabel);
			GUI.Label(new Rect(ipLabelPosition.x, ipLabelPosition.y, ipLabelSize.x, ipLabelSize.y), ipLabel);

			Vector2 portLabelPosition = new Vector2(ipLabelPosition.x, ipLabelPosition.y + ipLabelSize.y);
			GUIContent portLabel = new GUIContent("Server Port: ");
			Vector2 portLabelSize = GUI.skin.label.CalcSize(portLabel);
			GUI.Label(new Rect(portLabelPosition.x, portLabelPosition.y, portLabelSize.x, portLabelSize.y),
			          portLabel);

			Vector2 passwordLabelPosition = new Vector2(portLabelPosition.x,
			                                            portLabelPosition.y + portLabelSize.y);
			GUIContent passwordLabel = new GUIContent("Server Password: ");
			Vector2 passwordLabelSize = GUI.skin.label.CalcSize(passwordLabel);
			GUI.Label(new Rect(passwordLabelPosition.x, passwordLabelPosition.y,
			                   passwordLabelSize.x, passwordLabelSize.y), passwordLabel);

			float maxX = Mathf.Max(ipLabelPosition.x + ipLabelSize.x,
			                       portLabelPosition.x + portLabelSize.x,
			                       passwordLabelPosition.x + passwordLabelSize.x);

			//Wrap the text field in validation, only valid character are allowed.
			ip = ValidateIP(GUI.TextField(new Rect(maxX, ipLabelPosition.y, 150, ipLabelSize.y), ip));

			//Do the same for port
			port = ValidatePort(GUI.TextField(new Rect(maxX, portLabelPosition.y, 150, portLabelSize.y), port));

			//Password... anything goes
			password = GUI.TextField(new Rect(maxX, passwordLabelPosition.y, 150,
			                                  passwordLabelSize.y), password);

			GUIContent buttonConnect = new GUIContent("Connect");
			if(GUI.Button(new Rect(position.x, passwordLabelPosition.y + passwordLabelSize.y,
			                       (maxX - position.x) + 150, GUI.skin.button.
			                       CalcHeight(buttonConnect, (maxX - position.x) + 150)),buttonConnect)) {
				TryConnect(ip, port, password);
			}	
		}
	}
}
