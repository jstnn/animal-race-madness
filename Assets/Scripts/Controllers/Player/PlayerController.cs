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
				player.mass = kvp.Value ["mass"].AsFloat;
				rigidBody.mass= kvp.Value["mass"].AsFloat;
				player.force = kvp.Value["force"].AsFloat;
				player.speed = kvp.Value["speed"].AsFloat;
				player.acceleration = kvp.Value["acceleration"].AsFloat;
			}
		}
        // fix models scale to world scale
		transform.localScale = new Vector3 (3, 3, 3);

	}
    Vector3 target;
	private void Update()
	{
        if (player.mainPlayer && manager.fsm.State == Main.States.Play)
        {
            
            if (Input.GetKeyDown("space"))
            {
                Vector3 up = transform.TransformDirection(Vector3.up);
                rigidBody.AddForce(up * 10f, ForceMode.Impulse);
            }
            if (Input.GetMouseButton(0))
            {
				Debug.Log("HI");
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

				if (Physics.Raycast(ray, out hit, 100) && hit.transform.gameObject.tag == "Floor")
                {
					Debug.Log(hit.transform.gameObject.name);
                    target = (hit.point - transform.position).normalized;
                    GameObject point = (GameObject)Instantiate(Resources.Load("Point"), hit.point + Vector3.up, Quaternion.identity);
                    // rigidBody.velocity = Vector3.zero;
					rigidBody.AddForce(target * player.acceleration * player.mass, ForceMode.Impulse);
					Debug.Log(player.playerType + ": " + rigidBody.velocity.magnitude);
                    Destroy(point, .1f);
                }
            }
        }
        // Smoothly rotate player towards the target point.
        if (target != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target), 5f * Time.deltaTime);
	}
}