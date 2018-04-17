using UnityEngine;

public class CameraManager : MonoBehaviour {

    public Camera generalCamera;
    public Camera selectionCamera;
    public Camera raceCamera;

    void OnEnable()
    {
        EventsManager.SwitchToGeneralCamEvent += SwitchToGeneralCamEvent;
        EventsManager.SwitchToSelectionCamEvent += SwitchToSelectionCamEvent;
        EventsManager.SwitchToRaceCamEvent += SwitchToRaceCamEvent;
    }
    void OnDisable()
    {
        EventsManager.SwitchToGeneralCamEvent -= SwitchToGeneralCamEvent;
        EventsManager.SwitchToSelectionCamEvent -= SwitchToSelectionCamEvent;
        EventsManager.SwitchToRaceCamEvent -= SwitchToRaceCamEvent;
    }
 
    void Start()
    {
        SwitchToGeneralCamEvent();
    }

    public void SwitchToGeneralCamEvent() {
        generalCamera.enabled = true;
        selectionCamera.enabled = false;
        raceCamera.enabled = false;
    }

    public void SwitchToSelectionCamEvent()
    {
        generalCamera.enabled = false;
        selectionCamera.enabled = true;
        raceCamera.enabled = false;
    }

    public void SwitchToRaceCamEvent(GameObject player)
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
