using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class Animal : MonoBehaviour {

	public float velocity = 0f;
	public float experience = 0f;
	public string playerType;
	public string playerName;
	public string uuid;
	public bool mainPlayer = false;


	// Use this for initialization
	void Start () {
		this.transform.localScale = new Vector3 (2, 2, 2);
	}

}
