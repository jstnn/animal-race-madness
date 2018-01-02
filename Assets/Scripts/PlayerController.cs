using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	public Rigidbody rb;
	private Player player;
	int intensity=60;
	void Start() {
		rb = GetComponent<Rigidbody>();
		player = this.GetComponent<Player> ();
	}
	void FixedUpdate ()
	{
		if (player.mainPlayer == true) {
			if (Input.GetKeyDown (KeyCode.Space)) {
				Debug.Log ("move");
				rb.MovePosition (transform.position + transform.forward*intensity * Time.deltaTime);
				Debug.Log ((transform.position + transform.forward*intensity * Time.deltaTime)+"move");
			}
		}
	}
}