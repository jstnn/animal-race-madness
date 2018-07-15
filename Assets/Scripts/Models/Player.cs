using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System;

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
		public bool lockedLap;
		public Rigidbody currentRb;
        float colliderSize = .5f;
		int scaleFactor = 3;

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
            isInGround = true;
            idleName = "idle";
            walkName = "walk";
            runName = "run";
			currentLap = 0;
			lockedLap = true;
        }

        void Start()
        {
			transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            currentRb = gameObject.AddComponent<Rigidbody>();
            currentRb.detectCollisions = true;
            currentRb.freezeRotation = true;
            currentRb.mass = mass;
            currentRb.drag = 1f;
            currentRb.collisionDetectionMode = CollisionDetectionMode.Continuous;

            foreach (Transform child in GetComponentsInChildren<Transform>(true)) //include inactive
            {
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


			string animationsData = RaceManager.GetStringFromFile("animal-info");
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
        }
        
		public void Jump()
        {
			if (isInGround) {
				currentRb.AddForce(new Vector3(0, acceleration * mass * 2 , 0), ForceMode.Impulse);
			}
        }


		public void MoveToTarget(Vector3 target)
		{
			if (isInGround)
			{
				Vector3 impulseTarget = (target - transform.position).normalized;
				GameObject point = (GameObject)Instantiate(Resources.Load("Point"), target, Quaternion.identity);
				currentRb.AddForce((impulseTarget * acceleration * mass) / scaleFactor, ForceMode.Impulse);
				Destroy(point, .1f);
			}
		}

        void OnCollisionEnter(Collision collision)
        {
			if (collision.gameObject.tag == "Player")
            {
				collision.rigidbody.velocity *= -1;
            }     
            if (collision.gameObject.tag == "Floor")
            {
                isInGround = true;
                currentRb.freezeRotation = true;
			} else {
				isInGround = false;
                currentRb.freezeRotation = false;
			}        
        }
        

		private void OnTriggerEnter(Collider collision)
		{
			
			if (collision.gameObject.tag == "Checkpoint")
            {
				currentCheckpoint = collision.gameObject.GetComponent<Checkpoint>().position;            

				if (collision.gameObject.GetComponent<Checkpoint>().lastCheckpoint && lockedLap == true)
                {
					if (RaceManager.maxLaps == currentLap++) {
						RaceManager manager = GameObject.Find("RaceManager").GetComponent<RaceManager>();
						manager.fsm.ChangeState(RaceManager.States.Win);
					}
					lockedLap = false;
                    currentLap++;
					EventsManager.AddTime(10f);
                }
				// this is a very bad solution
                if ((currentCheckpoint + currentLap * 1000) > (100 + currentLap * 1000))
                {
                    lockedLap = true;
                }
            }
            
            
		}
      
	}

}

