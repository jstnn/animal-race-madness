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

    float colliderSize = 0.3f;

    public Rigidbody currentRb;

    void Awake()
    {
        transform.localScale = new Vector3(2, 2, 2);
        currentRb = gameObject.AddComponent<Rigidbody>();
        currentRb.detectCollisions = true;
        currentRb.freezeRotation = true;
        // currentRb.isKinematic = true;
        foreach (Transform child in GetComponentsInChildren<Transform>(true)) //include inactive
        {
            if (child.gameObject.name.Contains("_"))
            {
                Debug.Log(child.gameObject.name);
                BoxCollider joint = child.gameObject.AddComponent<BoxCollider>();
                joint.size = new Vector3(colliderSize, colliderSize, colliderSize);
            }

        }
    }

}
