using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class PlayerController : MonoBehaviour
{
	Rigidbody rb;
	Animal player;
    Animation anim;
    Main manager;
    int walkMinSpeed = 1;
    int runMinSpeed = 20;
	float acceleration = 4.0f;

	void Start() {
        manager = GameObject.Find("RaceManager").GetComponent<Main>();
		player = GetComponent<Animal> ();
        rb = player.currentRb;
        anim = player.GetComponent<Animation>();

		string animationsData = Read("animations");
		var data = JSON.Parse(animationsData);
		var animations = data ["playerTypes"];
		foreach(KeyValuePair<string,JSONNode> kvp in animations) {
			if ( player.playerType == kvp.Value ["type"].Value ) {
				player.idleName = kvp.Value ["idle"].Value;
				player.walkName = kvp.Value ["walk"].Value;
				player.runName = kvp.Value ["run"].Value;
				player.mass = kvp.Value ["mass"].AsInt;
                player.force = kvp.Value["force"].AsInt;
			}
		}
		transform.localScale = new Vector3 (2, 2, 2);

	}
	void FixedUpdate ()
	{
        // MAIN PLAYER
        if (player.mainPlayer && manager.fsm.State == Main.States.Play)
        {
            // JUMP
            if (Input.GetKeyDown(KeyCode.A) && player.isInGround)
            {
                rb.velocity += new Vector3(0, player.force*acceleration, player.force) + (acceleration * gameObject.transform.forward);
            }
            // ADVANCE FORWARD
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity += new Vector3(0, 0, player.force * acceleration) + (acceleration * gameObject.transform.forward);
            }
        }
        // NPC
        if (!player.mainPlayer && manager.fsm.State == Main.States.Play)
        {
            var r = Random.Range(0, 100);
            if (r < 5)
            {
                rb.velocity += new Vector3(0, 0, player.force) + (acceleration * gameObject.transform.forward);
            }
        }
	
			
		// Animations by velocity
        if (rb.velocity.z < walkMinSpeed) {
			anim.CrossFade(player.idleName);
		}
        else if (rb.velocity.z > walkMinSpeed && rb.velocity.z <= runMinSpeed) {
			anim.CrossFade(player.walkName);
		}
        else if (rb.velocity.z > runMinSpeed) {
			anim.CrossFade(player.runName);
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