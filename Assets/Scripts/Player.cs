using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class Player : MonoBehaviour {

	public float velocity = 0f;
	public float experience = 0f;
	public string playerType;
	public string uuid;
	public string idleName = "idle";
	public string walkName = "walk";
	public string runName = "run";
	public bool mainPlayer = false;

	// Use this for initialization
	void Start () {
		
		string animationsData = Read("animations");
		var data = JSON.Parse(animationsData);
		var animations = data ["animations"];
		foreach(System.Collections.Generic.KeyValuePair<string,SimpleJSON.JSONNode> kvp in animations) {
			// Debug.Log(playerType+"+@@@@@@+"+kvp.Value ["type"].Value);
			if ( playerType == kvp.Value ["type"].Value ) {
				idleName = kvp.Value ["idle"].Value;
				walkName = kvp.Value ["walk"].Value;
				runName = kvp.Value ["run"].Value;
			}

		}
		this.transform.localScale = new Vector3 (2, 2, 2);
	}
			




	void Update() {
		this.GetComponent<Animation> ().Play (idleName);
		if (mainPlayer==true) {
			if (Input.GetKeyDown (KeyCode.Space)) {

				this.GetComponent<Animation> ().Play (runName);


				Vector3 position = this.transform.position;
				position.z++;
				this.transform.position = position;


			} else {
				this.GetComponent<Animation> ().Play (idleName);
			}
		}

	}



	public static string Read(string filename) {
		//Load the text file using Reources.Load
		TextAsset theTextFile = Resources.Load<TextAsset>(filename);

		//There's a text file named filename, lets get it's contents and return it
		if (theTextFile != null) {
			return theTextFile.text;
		} else {
			//There's no file, return an empty string.
			return "";
		}
	}

}
