using UnityEngine;
using ARM;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class PlayerController : MonoBehaviour
{
    Player player;
    Rigidbody rigidBody;
    Main manager;

	void Start() {
        manager = GameObject.Find("RaceManager").GetComponent<Main>();
        player = GetComponent<Player> ();
        rigidBody = player.currentRb;

        string animationsData = Main.GetStringFromFile("animal-info");
		var data = JSON.Parse(animationsData);
		var animations = data ["animals"];
		foreach(KeyValuePair<string,JSONNode> kvp in animations) {
			if ( player.playerType == kvp.Value ["type"].Value ) {
				player.mass = kvp.Value ["mass"].AsInt;
                player.force = kvp.Value["force"].AsInt;
                player.speed = kvp.Value["speed"].AsInt;
			}
		}
        // fix models scale to world scale
		transform.localScale = new Vector3 (2, 2, 2);

	}
    Vector3 target;
	private void Update()
	{
        if (player.mainPlayer && manager.fsm.State == Main.States.Play)
        {
            if (Input.GetKeyDown("space"))
            {
                Vector3 up = transform.TransformDirection(Vector3.up);
                rigidBody.AddForce(up * 3, ForceMode.Impulse);
            }
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 1000) && !hit.rigidbody)
                {
                    target = (hit.point - transform.position).normalized;
                    rigidBody.AddForce(target * 3, ForceMode.Impulse);
                }
            }
        }
        // Smoothly rotate player towards the target point.
        if (target != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target), 5f * Time.deltaTime);
	}
}