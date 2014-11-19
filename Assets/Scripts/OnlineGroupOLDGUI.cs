using UnityEngine;
using System.Collections;

public class OnlineGroupOLDGUI : MonoBehaviour
{
		private int lastLevelPrefix = 0;
		public Rpc RpcLink;
		float count = 0;
		public string levelToLoad = "1";
		public GUISkin mySkin;
		public bool onFinder = true;
		public bool onSettings = false;
		private Rect windowRect1 = new Rect (25, 25, 600, 100);
		private Rect windowRect2 = new Rect (25, 25, 600, 100);
		public string disconnectedLevel = "MainMenu";
		private string gameName = "KoTD Tresure Ruse";
		private bool refreshing = false;
		private HostData[] hostData;
		private string password = "nimda";
		private string level = "1";
		private string username = "Your Player Name";
		private string ip = "if you see this then there is a problem";
		private string port = "27050";
		private string playerCount = "4";
		private int players;
		private int portint;
		public MainMenuEvents mainMenuLink;


		// Use this for initialization
		void Start ()
		{
				onFinder = true;
				onSettings = false;
				ip = Network.player.ipAddress;
				HostData[] hostdata = MasterServer.PollHostList ();
				DontDestroyOnLoad (this);
				networkView.group = 1;
				Network.SetLevelPrefix (lastLevelPrefix);
		
				if (Network.HavePublicAddress ()) {
						Debug.Log ("This machine has a public IP address");
				} else {
						Debug.Log ("This machine has a private IP address");
				}
		}

		void  Connect (string ip, int port)
		{
				Network.Connect (ip, port, password);
		
		}

		void OnConnectedToServer ()
		{
				Network.RemoveRPCsInGroup (0);
				Network.RemoveRPCsInGroup (1);
				RpcLink.networkView.RPC ("LoadLevel", RPCMode.AllBuffered, level, lastLevelPrefix + 1);
				int tmpLevel;
				int.TryParse (level, out tmpLevel);
				Application.LoadLevel (tmpLevel.ToString ());
				Debug.Log ("Connected to server");
		}

		void OnFailedToConnect (NetworkConnectionError error)
		{
				mainMenuLink.SwitchToMainGroup ();
				mainMenuLink.onlineGroupActive = false;
		}

		void startServer ()
		{
		
				int.TryParse (playerCount, out players);
				int.TryParse (port, out portint);
		
				Network.incomingPassword = password;
				bool useNat = !Network.HavePublicAddress ();
				Network.InitializeServer (players, portint, useNat);
				MasterServer.RegisterHost (gameName, "ShArksGiving", level);
				Debug.Log ("started server");
		}

		void refreshHostList ()
		{
				MasterServer.RequestHostList (gameName);
				refreshing = true;
		}

		void DoMyWindow1 (int windowID)
		{
				GUILayout.BeginVertical ();
				GUILayout.Label ("Multiplayer Setup");
				GUILayout.Label ("Username");
				username = GUILayout.TextField (username, 32);
				if (GUILayout.Button ("start a server")) {
						onFinder = false;
						onSettings = true;
				}
				if (GUILayout.Button ("Refresh Server list")) {
						Debug.Log ("Refresh Hosts");
						refreshHostList ();
				}
				if (GUILayout.Button ("Back To Main")) {
						mainMenuLink.SwitchToMainGroup ();
						mainMenuLink.onlineGroupActive = false;
				}
				GUILayout.Label ("password (defult is nimda)");
				password = GUILayout.PasswordField (password, "*" [0], 32);
				GUILayout.Label ("Multiplayer Servers (may need refresh)");
				int i = 0;
				if (MasterServer.PollHostList ().Length > i) { 
						foreach (HostData element in hostData) {
								if (GUILayout.Button (element.connectedPlayers + " of " + element.playerLimit + " on level " + element.comment + "[" + element.ip [0] + "]")) { 
										string tmpIp = "";
										tmpIp = element.ip [i] + "";
										Connect (tmpIp, element.port);
								}
								i++;
						}
				}
				GUILayout.EndVertical ();
		}

		void DoMyWindow2 (int windowID)
		{
				GUILayout.BeginVertical ();
				GUILayout.Label ("Server Setup");
				GUILayout.Label ("ip address (should be prefilled out)");
				ip = GUILayout.TextField (ip, 32);
		
				GUILayout.Label ("port");
				port = GUILayout.TextField (port, 32);
		
				GUILayout.Label ("level (1) or level (1)");
				level = GUILayout.TextField (level, 1);
		
				GUILayout.Label ("Your game's unique name");
				gameName = GUILayout.TextField (gameName, 32);
		
				GUILayout.Label ("Max Players (recommended 4 or less)");
				playerCount = GUILayout.TextField (playerCount, 2);
		
				GUILayout.Label ("password (defult is nimda)");
				password = GUILayout.PasswordField (password, "*" [0], 32);

		
				if (GUILayout.Button ("start server")) {
						startServer ();
				}
				if (GUILayout.Button ("Close Server Setup")) {
						onFinder = true;
						onSettings = false;
				}
				GUILayout.EndVertical ();
		}

		void  Update ()
		{
				count -= Time.deltaTime;
				if (count < 0) {
						refreshHostList ();
						count = 3;
				}
				if (refreshing) {
						if (MasterServer.PollHostList ().Length > 0) {  
								refreshing = false;
								Debug.Log (MasterServer.PollHostList ().Length);
								hostData = MasterServer.PollHostList ();
						}
				}
		}
		// Update is called once per frame
		void OnGUI ()
		{
				if (mainMenuLink.onlineGroupActive) {
						if (onFinder) {
								windowRect1 = GUILayout.Window (0, windowRect1, DoMyWindow1, "");
						} else if (onSettings) {
								windowRect2 = GUILayout.Window (0, windowRect2, DoMyWindow2, "");
						}
				}
				GUI.skin = mySkin;
				if (Network.isServer) {
						bool first = true;
						if (first) {
								int tmpLevel;
								int.TryParse (level, out tmpLevel);
								Application.LoadLevel (tmpLevel);
								first = false;
						}
				}
		}
}