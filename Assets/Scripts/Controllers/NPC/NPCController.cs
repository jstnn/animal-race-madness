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
    public float damping = 0.1f;
    public bool loop = false;
    public float speed = 2.0f;
    public bool faceHeading  = true;

    private Vector3 currentHeading, targetHeading;
    private int targetwaypoint;
    private Transform xform;
    private bool useRigidbody;
    private Rigidbody rigidmember;

    Main manager;
    Player player;
    int speedRatio = 50;

    // Use this for initialization
    protected void Start()
    {
        //////// SETTING THISN UP ///////

        manager = GameObject.Find("RaceManager").GetComponent<Main>();
        player = GetComponent<Player>();
        string animationsData = Main.GetStringFromFile("animal-info");
        var data = JSON.Parse(animationsData);
        var animations = data["animals"];
        foreach (KeyValuePair<string, JSONNode> kvp in animations)
        {
            if (player.playerType == kvp.Value["type"].Value)
            {
                speed = kvp.Value["speed"].AsInt / speedRatio;
            }
        }
        waypoints = new List<Transform>();
        faceHeading = false;
        damping = 0.1f;
        loop = true;
        waypointRadius = 7f;
        List<GameObject> waypointObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Waypoint")).OrderBy(go => go.name).ToList();
        foreach (var item in waypointObjects)
        {
            waypoints.Add(item.transform);

        }
        /// //////////////&///////////////
        xform = transform;
        currentHeading = xform.forward;
        if (waypoints.Count <= 0)
        {
            Debug.Log("No waypoints on " + name);
            enabled = false;
        }
        targetwaypoint = 0;
        if (GetComponent<Rigidbody>() != null)
        {
            useRigidbody = true;
            rigidmember = GetComponent<Rigidbody>();
        }
        else
        {
            useRigidbody = false;
        }
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
        if (useRigidbody && manager.fsm.State == Main.States.Play)
            rigidmember.velocity = currentHeading * speed;
        else
            xform.position += currentHeading * Time.deltaTime * speed;
        if (faceHeading)
            xform.LookAt(xform.position + currentHeading);

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


    // draws red line from waypoint to waypoint
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