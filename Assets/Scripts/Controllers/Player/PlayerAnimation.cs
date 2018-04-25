using System.Collections;
using ARM;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class PlayerAnimation : MonoBehaviour {
    Player player;
    Rigidbody rigidBody;
    Animation anim;
    int thresholdWalkVelocity = 0;
    int thresholdRunVelocity = 10;

	private void Start()
	{
        player = GetComponent<Player>();
        rigidBody = player.currentRb;
		anim = player.GetComponent<Animation>();

        string animationsData = Main.GetStringFromFile("animal-info");
        var data = JSON.Parse(animationsData);
        var animations = data["animals"];
        foreach (KeyValuePair<string, JSONNode> kvp in animations)
        {
            if (player.playerType == kvp.Value["type"].Value)
            {
                player.idleName = kvp.Value["idle"].Value;
                player.walkName = kvp.Value["walk"].Value;
                player.runName = kvp.Value["run"].Value;
            }
        }
	}

	void FixedUpdate()
    {
        // Animations by velocity
        if (rigidBody.velocity.magnitude < thresholdWalkVelocity)
        {
            anim.CrossFade(player.idleName);
        }
        if (rigidBody.velocity.magnitude > thresholdWalkVelocity && rigidBody.velocity.magnitude <= thresholdRunVelocity)
        {
            anim.CrossFade(player.walkName);
        }
        if (rigidBody.velocity.magnitude > thresholdRunVelocity)
        {
            anim.CrossFade(player.runName);
        }
    }
}
