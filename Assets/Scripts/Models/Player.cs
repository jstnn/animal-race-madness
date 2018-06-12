﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

namespace ARM
{
    public class Animal
    {
        public string name;
		public int position;
    }

    public class Player : MonoBehaviour
    {

        public float velocity;
        public float experience;
        public string playerType;
        public string playerName;
        public string uuid;
        public float mass;
        public float force;
        public float speed;
		public float acceleration;
        public bool mainPlayer;
        public bool isInGround;
        public string idleName;
        public string walkName;
        public string runName;
        public Animal animal;
		public int currentCheckpoint;
		public int currentLap;
		public Rigidbody currentRb;
        float colliderSize = 0.18f;

        private void Awake()
        {
            // initialize animal
            uuid = "19849731-323d-486d-a548-8fd2f8e29811";
            playerType = "PIG";
            playerName = "Unknown";
            experience = 1;
            mass = 1f;
            force = 1f;
            speed = 10f;
			acceleration = 1f;
            mainPlayer = false;
            isInGround = false;
            idleName = "idle";
            walkName = "walk";
            runName = "run";
			currentLap = 1;
        }

        void Start()
        {
            // transforms
            transform.localScale = new Vector3(2, 2, 2);
            currentRb = gameObject.AddComponent<Rigidbody>();
            currentRb.detectCollisions = true;
            currentRb.freezeRotation = true;
            currentRb.mass = mass;
            currentRb.drag = 5f;
            currentRb.collisionDetectionMode = CollisionDetectionMode.Continuous;

            foreach (Transform child in GetComponentsInChildren<Transform>(true)) //include inactive
            {
				// if (child.gameObject.name.Contains("Foot") || child.gameObject.name.Contains("Hand"))
				if (child.gameObject.name.Contains("Head"))
                {
                    child.gameObject.tag = "Player";
                    BoxCollider joint = child.gameObject.AddComponent<BoxCollider>();
                    joint.size = new Vector3(colliderSize, colliderSize, colliderSize);
                }
                //if (child.gameObject.name.Contains("Spine"))
                //{
                //    BoxCollider joint = child.gameObject.GetComponent<BoxCollider>();
                //    joint.size = new Vector3(1.2f, 0.5f, 0.5f);
                //}
            }


			string animationsData = Main.GetStringFromFile("animal-info");
            var data = JSON.Parse(animationsData);
            var animations = data["animals"];
            foreach (KeyValuePair<string, JSONNode> kvp in animations)
            {
                if (playerType == kvp.Value["type"].Value)
                {
                    mass = kvp.Value["mass"].AsFloat;
					currentRb.mass = kvp.Value["mass"].AsFloat;
                    force = kvp.Value["force"].AsFloat;
                    speed = kvp.Value["speed"].AsFloat;
                    acceleration = kvp.Value["acceleration"].AsFloat;
                }
            }
            // fix models scale to world scale
            //transform.localScale = new Vector3 (3, 3, 3);
        }
        
		public void MoveToTarget(Vector3 target) {
			Vector3 impulseTarget = (target - transform.position).normalized;
			GameObject point = (GameObject)Instantiate(Resources.Load("Point"), target, Quaternion.identity);
			currentRb.AddForce(impulseTarget * acceleration * mass, ForceMode.Impulse);
            // Debug.Log(player.playerType + ": " + rigidBody.velocity.magnitude);
            Destroy(point, .1f);
		}

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Floor")
            {
                isInGround = true;
                // currentRb.freezeRotation = true;
            }         
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.tag == "Floor")
            {
                isInGround = false;
                // currentRb.freezeRotation = false;
            }
        }

		private void OnTriggerEnter(Collider collision)
		{
			if (collision.gameObject.tag == "Checkpoint")
            {
				currentCheckpoint = collision.gameObject.GetComponent<Checkpoint>().position;
            }
            
		}
      
	}

}

