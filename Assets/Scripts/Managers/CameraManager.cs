using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour {

    public CinemachineVirtualCamera generalCamera;
    public CinemachineVirtualCamera selectionCamera;
    public CinemachineVirtualCamera raceCamera;

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
        generalCamera.MoveToTopOfPrioritySubqueue();
    }

    public void SwitchToSelectionCamEvent()
    {
        selectionCamera.MoveToTopOfPrioritySubqueue();
    }

    public void SwitchToRaceCamEvent(GameObject player)
    {
        raceCamera.MoveToTopOfPrioritySubqueue();
        raceCamera.Follow = player.transform;
        raceCamera.LookAt = player.transform;
    }

}
