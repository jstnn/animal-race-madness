using UnityEngine;
using System;
using System.Text.RegularExpressions;

public class Waypoint : MonoBehaviour {
	public int order;

	private void Start()
	{
		order = Int32.Parse(Regex.Replace(name, "[^0-9]", ""));
	}
}
