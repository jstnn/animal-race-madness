using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class PlayerController : MonoBehaviour
{
	public string idleName = "idle";
	public string walkName = "walk";
	public string runName = "run";

	Rigidbody rb;
	Animal player;
    int patrolSpeed = 20;
	int force = 2;
	float acceleration = 2.0f;

	void Start() {
		player = GetComponent<Animal> ();
        rb = player.currentRb;

		string animationsData = Read("animations");
		var data = JSON.Parse(animationsData);
		var animations = data ["playerTypes"];
		foreach(KeyValuePair<string,JSONNode> kvp in animations) {
			if ( player.playerType == kvp.Value ["type"].Value ) {
				idleName = kvp.Value ["idle"].Value;
				walkName = kvp.Value ["walk"].Value;
				runName = kvp.Value ["run"].Value;
				player.mass = kvp.Value ["mass"].AsInt;
			}
		}
		transform.localScale = new Vector3 (2, 2, 2);

	}
	void FixedUpdate ()
	{
		Animation anim = player.GetComponent<Animation> ();

		if (player.mainPlayer == true) {
			if (Input.GetKeyDown (KeyCode.A)) {
				rb.velocity += new Vector3(0,force*3,0) + (acceleration * gameObject.transform.forward);
			}
			if (Input.GetKeyDown (KeyCode.Space)) {
				rb.velocity += new Vector3(0,0,force) + (acceleration * gameObject.transform.forward);
			}
		}
			
		// Animations by velocity
		if (rb.velocity.z == 0) {
			anim.CrossFade(idleName);
		}
		else if (rb.velocity.z > 0 && rb.velocity.z <= patrolSpeed) {
			anim.CrossFade(walkName);
		}
		else if (rb.velocity.z > patrolSpeed) {
			anim.CrossFade(runName);
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