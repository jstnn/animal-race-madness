using System.Collections;
using ARM;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class PlayerAnimation : MonoBehaviour {
    Player player;
    Rigidbody rigidBody;
    Animation anim;
    float thresholdWalkVelocity = 100f;
    float thresholdRunVelocity = 1000f;

	private void Start()
	{
        player = GetComponent<Player>();
        rigidBody = player.currentRb;
		anim = player.GetComponent<Animation>();

		string animationsData = RaceManager.GetStringFromFile("animal-info");
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
        if (player.mainPlayer) {
            // Debug.Log($"V: {rigidBody.velocity.sqrMagnitude}");
        }

        // Animations by velocity
		if (rigidBody.velocity.sqrMagnitude < thresholdWalkVelocity)
        {
            anim.CrossFade(player.idleName);
        }
		if (rigidBody.velocity.sqrMagnitude > thresholdWalkVelocity && rigidBody.velocity.sqrMagnitude <= thresholdRunVelocity)
        {
            anim.CrossFade(player.walkName);
        }
		if (rigidBody.velocity.sqrMagnitude > thresholdRunVelocity)
        {
            anim.CrossFade(player.runName);
        }
    }
}
