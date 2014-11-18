using UnityEngine;
using System.Collections;

public class FinderScript : MonoBehaviour
{
		private HostData[] hostData;
		public GameObject serverButton;

		// Use this for initialization
		void Start ()
		{
				int i = 0;
				MasterServer.ClearHostList ();
				MasterServer.RequestHostList ("KingOfTheDungeon");
				hostData = MasterServer.PollHostList ();
				Debug.Log (hostData.Length.ToString () + " Servers Found");
				while (i < hostData.Length) {
						Debug.Log ("Game Name: " + hostData [i].gameName);
						i++;
				}
		}
	
		public void RefreshServerList ()
		{
				int i = 0;
				MasterServer.ClearHostList ();
				MasterServer.RequestHostList ("KingOfTheDungeon");
				hostData = MasterServer.PollHostList ();
				Debug.Log (hostData.Length.ToString () + " Servers Found");
				while (i < hostData.Length) {
						Debug.Log ("Game Name: " + hostData [i].gameName);
						i++;
				}
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
}
