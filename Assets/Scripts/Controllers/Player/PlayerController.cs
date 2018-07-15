using UnityEngine;
using ARM;

public class PlayerController : MonoBehaviour
{
    Player player;
	RaceManager manager;
	Vector3 target;
    
	void Start() {
		manager = GameObject.Find("RaceManager").GetComponent<RaceManager>();
        player = GetComponent<Player> ();
	}

	private void Update()
	{
		if (player.mainPlayer && manager.fsm.State == RaceManager.States.Play)
        {
            
            if (Input.GetKeyDown("space"))
            {
				player.Jump();
            }
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

				if (Physics.Raycast(ray, out hit, 100) && hit.transform.gameObject.tag == "Floor")
                {
					target = hit.point - transform.position;
					player.MoveToTarget(hit.point);
                }
            }
        }
        // Smoothly rotate player towards the target point.
        if (target != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target), 10f * Time.deltaTime);
	}
}