using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class RaceManager : MonoBehaviour {
	public List<GameObject> players = new List<GameObject>();
	private static string url = "https://api.myjson.com/bins/y73cf";

	public int raceWidth = 25;


	// Use this for initialization
	void Start () {
		WWW www = new WWW(url);
		StartCoroutine(WaitForRequest(www));
	}


	IEnumerator WaitForRequest(WWW www)
	{
		yield return www;

		// check for errors
		if (www.error == null)
		{
			var data = JSON.Parse(www.text);
			var players = data ["players"];
			foreach(System.Collections.Generic.KeyValuePair<string,SimpleJSON.JSONNode> kvp in players) {
				CreatePlayer (kvp.Value ["type"].Value, kvp.Value ["current_player"].AsBool);
			}
			PositionPlayers ();
		} else {
			Debug.Log("WWW Error: "+ www.error);
		}    
	}

	private void PositionPlayers() {
		int trackWidth = raceWidth / players.Count;
		Debug.Log (trackWidth);
		int firstPosition = 1;
		int offsetTrack = trackWidth / 2;
		foreach (GameObject player in players) {
			int playerPosition = (firstPosition * trackWidth) - offsetTrack;
			player.transform.position = new Vector3 (playerPosition, player.transform.position.y, player.transform.position.z);
			firstPosition++;
		}
	}

	private void CreatePlayer(string type, bool mainPlayer) {
		string prefabName = "animals/" + type + "/FBX FILES/" + type;
		GameObject instance = Instantiate(Resources.Load(prefabName, typeof(GameObject))) as GameObject;
		instance.AddComponent<Player> ();
		instance.GetComponent<Player> ().playerType = type;
		instance.GetComponent<Player> ().mainPlayer = mainPlayer;
		players.Add (instance);
	}
}
