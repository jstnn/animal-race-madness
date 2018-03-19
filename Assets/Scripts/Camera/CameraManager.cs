using UnityEngine;

public class CameraManager : MonoBehaviour {

    public Camera generalCamera;
    public Camera selectionCamera;
    public Camera raceCamera;
 
    void Start()
    {
        SwitchToGeneral();
    }

    public void SwitchToGeneral() {
        generalCamera.enabled = true;
        selectionCamera.enabled = false;
        raceCamera.enabled = false;
    }

    public void SwitchToSelection()
    {
        generalCamera.enabled = false;
        selectionCamera.enabled = true;
        raceCamera.enabled = false;
    }

    public void SwitchToRace(GameObject player)
    {
        generalCamera.enabled = false;
        selectionCamera.enabled = false;
        raceCamera.enabled = true;

        if (player) {
            CameraFollow cameraFollow = raceCamera.GetComponent<CameraFollow>();
            cameraFollow.target = player.transform;
        }
    }

}
