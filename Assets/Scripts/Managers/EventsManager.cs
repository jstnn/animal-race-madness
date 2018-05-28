using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManager : MonoBehaviour {

    // UI TEXT
    public delegate void UpdatePositionTextDelegate(string text);
    public static event UpdatePositionTextDelegate UpdatePositionTextEvent;
    public static void UpdatePositionText(string text)
    {
        if (UpdatePositionTextEvent != null)
            UpdatePositionTextEvent(text);
    }

    public delegate void UpdateCenterTextDelegate(string text);
    public static event UpdateCenterTextDelegate UpdateCenterTextEvent;
    public static void UpdateCenterText(string text)
    {
        if (UpdateCenterTextEvent != null)
            UpdateCenterTextEvent(text);
    }

    public delegate void UpdateTimeTextDelegate(string text);
    public static event UpdateTimeTextDelegate UpdateTimeTextEvent;
    public static void UpdateTimeText(string text)
    {
        if (UpdateTimeTextEvent != null)
            UpdateTimeTextEvent(text);
    }

    public delegate void ResetPlayersDelegate();
    public static event ResetPlayersDelegate ResetPlayersEvent;
    public static void ResetPlayers()
    {
        if (ResetPlayersEvent != null)
            ResetPlayersEvent();
    }


    // CAM MOVEMENTS
    public delegate void SwitchToGeneralCamDelegate();
    public static event SwitchToGeneralCamDelegate SwitchToGeneralCamEvent;
    public static void SwitchToGeneralCam()
    {
        if (SwitchToGeneralCamEvent != null)
            SwitchToGeneralCamEvent();
    }

    public delegate void SwitchToSelectionCamDelegate();
    public static event SwitchToSelectionCamDelegate SwitchToSelectionCamEvent;
    public static void SwitchToSelectionCam()
    {
        if (SwitchToSelectionCamEvent != null)
            SwitchToSelectionCamEvent();
    }

    public delegate void SwitchToRaceCamDelegate(GameObject player);
    public static event SwitchToRaceCamDelegate SwitchToRaceCamEvent;
    public static void SwitchToRaceCam(GameObject player)
    {
        if (SwitchToRaceCamEvent != null)
            SwitchToRaceCamEvent(player);
    }

	public delegate void StartRaceDelegate();
    public static event StartRaceDelegate StartRaceEvent;
    public static void StartRace()
    {
        if (StartRaceEvent != null)
            StartRaceEvent();
    }

	public delegate void SwitchUiContextDelegate(string context);
    public static event SwitchUiContextDelegate SwitchUiContextEvent;
    public static void SwitchUiContext(string context)
    {
        if (SwitchUiContextEvent != null)
            SwitchUiContextEvent(context);
    }


}
