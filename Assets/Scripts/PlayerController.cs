using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	public Rigidbody rb;
	private Player player;

	int intensity = 160;

	void Start() {
		rb = GetComponent<Rigidbody>();
		player = this.GetComponent<Player> ();

		rb.mass = 100;
	}
	void FixedUpdate ()
	{
		if (player.mainPlayer == true) {
			if (Input.GetKeyDown (KeyCode.A)) {
				rb.MovePosition (transform.position + transform.up * Time.deltaTime * intensity);
				Debug.Log ((transform.position + transform.up * Time.deltaTime * intensity)+"move up");
			}
			if (Input.GetKeyDown (KeyCode.Space)) {
				rb.MovePosition (transform.position + transform.forward * Time.deltaTime * intensity);
				Debug.Log ((transform.position + transform.forward * Time.deltaTime * intensity)+"move forward");
			}
		}
	}
}