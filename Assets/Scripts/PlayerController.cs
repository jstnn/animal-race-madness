﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class PlayerController : MonoBehaviour
{
	public string idleName = "idle";
	public string walkName = "walk";
	public string runName = "run";

	public Rigidbody rb;
	private Animal player;

	int patrolSpeed = 30;

	int force = 3;
	float acceleration = 3.0f;

	void Start() {
		rb = GetComponent<Rigidbody>();
		player = this.GetComponent<Animal> ();

		string animationsData = Read("animations");
		var data = JSON.Parse(animationsData);
		var animations = data ["playerTypes"];
		foreach(KeyValuePair<string,SimpleJSON.JSONNode> kvp in animations) {
			if ( player.playerType == kvp.Value ["type"].Value ) {
				idleName = kvp.Value ["idle"].Value;
				walkName = kvp.Value ["walk"].Value;
				runName = kvp.Value ["run"].Value;
				rb.mass = kvp.Value ["mass"].AsInt;
			}
		}
		this.transform.localScale = new Vector3 (2, 2, 2);
		if (player.mainPlayer) {
			GameObject mainCamera = GameObject.Find ("Main Camera");
			CameraFollow cameraFollow = mainCamera.AddComponent<CameraFollow> ();
			cameraFollow.target = this.gameObject.transform;
			cameraFollow.offset = new Vector3 (20, 20, -10);
			cameraFollow.smoothSpeed = 1;
		}
	}
	void FixedUpdate ()
	{
		Animation anim = player.GetComponent<Animation> ();

		if (player.mainPlayer == true) {
			if (Input.GetKeyDown (KeyCode.A)) {
				rb.velocity += new Vector3(0,force,0) + (acceleration * gameObject.transform.forward);
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