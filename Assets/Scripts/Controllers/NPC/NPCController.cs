// SeekSteer.cs
// Written by Matthew Hughes
// 19 April 2009
// Uploaded to Unify Community Wiki on 19 April 2009
// URL: http://www.unifycommunity.com/wiki/index.php?title=SeekSteer
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ARM;
using MonsterLove.StateMachine;
using SimpleJSON;

public class NPCController : MonoBehaviour
{

    public List<Transform> waypoints;
    public float waypointRadius = 1.5f;
    public float damping = 1f;
    public bool loop = false;
    private Vector3 currentHeading, targetHeading;
    private int targetwaypoint;
    private Transform xform;

	RaceManager manager;
    Player player;
    
    // Use this for initialization
    protected void Start()
    {
		//////// SETTING THIS UP ///////

        manager = GameObject.Find("RaceManager").GetComponent<RaceManager>();
        player = GetComponent<Player>();
        waypoints = new List<Transform>();
        damping = .1f;
        loop = true;
        waypointRadius = 20f;
        List<GameObject> waypointObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Checkpoint")).OrderBy(go => go.name).ToList();
        foreach (var item in waypointObjects)
        {
			Transform checkpoint = item.transform;
			checkpoint.position = new Vector3(checkpoint.position.x+Random.Range(-1,1), 0, checkpoint.position.z);
			waypoints.Add(checkpoint);
        }
        /////////////////&///////////////
        xform = transform;
        currentHeading = xform.forward;
        if (waypoints.Count <= 0)
        {
            Debug.Log("No waypoints on " + name);
            enabled = false;
        }
        targetwaypoint = 0;      
    }

    // calculates a new heading
    protected void FixedUpdate()
    {
        targetHeading = waypoints[targetwaypoint].position - xform.position;
		currentHeading = Vector3.Lerp(currentHeading, targetHeading, Time.deltaTime); 
    }

    // moves us along current heading
    protected void Update()
    {
		if (manager.fsm.State == RaceManager.States.Play) {
			float probability = .5f;
			if (Random.value <= probability) {
				player.MoveToTarget(xform.position + currentHeading);
				xform.LookAt(xform.position + currentHeading);
			}
        } 

        if (Vector3.Distance(xform.position, waypoints[targetwaypoint].position) <= waypointRadius)
        {
            targetwaypoint++;
            if (targetwaypoint >= waypoints.Count)
            {
                targetwaypoint = 0;
                if (!loop)
                    enabled = false;
            }
        }
    }


    //// draws red line from waypoint to waypoint
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (waypoints == null)
            return;
        for (int i = 0; i < waypoints.Count; i++)
        {
            Vector3 pos = waypoints[i].position;
            if (i > 0)
            {
                Vector3 prev = waypoints[i - 1].position;
                Gizmos.DrawLine(prev, pos);
            }
        }
    }

}