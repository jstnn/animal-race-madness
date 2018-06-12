using UnityEngine;
using System;
using System.Text.RegularExpressions;

public class Checkpoint : MonoBehaviour {
	public int position;

	void Awake()
	{
		this.gameObject.name = transform.parent.name;
		position = Int32.Parse(Regex.Replace(transform.parent.name, "[^0-9]", ""));

	}
}
