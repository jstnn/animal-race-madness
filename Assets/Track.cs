using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour {

    int lengthTrack = 50;
    bool collided = false;
    GameObject parentObject;

    private void Start()
    {
        parentObject = transform.parent.gameObject;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (!collided && collision.gameObject.tag == "Player")
        {
            collided = true;
            GameObject instance = Instantiate(Resources.Load("Track", typeof(GameObject))) as GameObject;
            instance.transform.position = new Vector3(parentObject.transform.position.x, parentObject.transform.position.y, parentObject.transform.position.z + lengthTrack);

        }
    }
}
