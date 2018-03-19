using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour {

    int lengthTrackInMeters = 50;
    bool collided = false;
    GameObject parentObject;

    public GameObject prefab;
    float gridX = 1f;
    float gridY = 1f;
    float spacing = 50f;

    void GenerateTrack() {
        collided = true;
        GameObject instance = Instantiate(Resources.Load("Track", typeof(GameObject))) as GameObject;
        instance.transform.position = new Vector3(parentObject.transform.position.x, parentObject.transform.position.y, parentObject.transform.position.z + lengthTrackInMeters);

        //for (int y = 0; y < gridY; y++) {
        //    for (int x = 0; x < gridX; x++) {
        //        Vector3 pos = new Vector3(x, 0, y) * spacing;
        //        Instantiate(instance, pos, Quaternion.identity);
        //    }
        //}
    } 

    void Start()
    {
        parentObject = transform.parent.gameObject;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collided == false && collision.gameObject.tag == "Player")
        {
            GenerateTrack(); 
        }
    }
}
