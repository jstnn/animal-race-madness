using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using MonsterLove.StateMachine;
namespace ARM
{
    public class Main : MonoBehaviour
    {
        //Declare which states we'd like use
        public enum States
        {
            Init,
            Select,
            Countdown,
            Play,
            Win,
            Lose
        }
        public StateMachine<States> fsm;
        float currentTime = 0;
        float maxTime = 30;
        RaceManager raceManager;

        void Awake()
        {
            //Initialize State Machine Engine		
            fsm = StateMachine<States>.Initialize(this, States.Init);

        }

        void Start()
        {
            raceManager = GetComponent<RaceManager>();
        }


        public string PositionText()
        {
            var ordered = raceManager.positions.OrderBy(x => x.Value.transform.position.z * -1);
            string text = "";
            int i = 1;
            foreach (KeyValuePair<int, GameObject> p in ordered)
            {
                text += "\n" + i++ + " -> " + (p.Value.GetComponent<Player>().mainPlayer ? "*** " : "") + p.Value.name.Replace("(Clone)", "");

            }
            return text;
        }

        void Init_Enter()
        {
            Debug.Log("Waiting for start button to be pressed");
        }

        void Select_Enter()
        {
            EventsManager.SwitchToSelectionCam();
            EventsManager.UpdateTimeText("");
            EventsManager.UpdateCenterText("");
            EventsManager.ResetPlayers();
        }

        //We can return a coroutine, this is useful animations and the like
        IEnumerator Countdown_Enter()
        {
            GameObject.Find("RaceManager").GetComponent<RaceManager>().Create();
            EventsManager.UpdateTimeText("");
            EventsManager.UpdateCenterText("Starting in 3...");
            yield return new WaitForSeconds(0.5f);
            EventsManager.UpdateCenterText("Starting in 2...");
            yield return new WaitForSeconds(0.5f);
            EventsManager.UpdateCenterText("Starting in 1...");
            yield return new WaitForSeconds(0.5f);

            fsm.ChangeState(States.Play);

            EventsManager.UpdateCenterText("Run");
            yield return new WaitForSeconds(0.1f);

        }


        void Play_Enter()
        {
            EventsManager.UpdateCenterText("");
            currentTime = maxTime;
        }

        void Play_Update()
        {
            EventsManager.UpdatePositionText(PositionText());
            currentTime -= 1 * Time.deltaTime;
            EventsManager.UpdateTimeText("Time left: " + Mathf.Round(currentTime).ToString());

            if (currentTime < 0)
            {
                fsm.ChangeState(States.Lose);
            }
        }

        void Play_Exit()
        {
            EventsManager.UpdateCenterText("Game Over");
            EventsManager.SwitchToGeneralCam();
        }

        void Lose_Enter()
        {
            EventsManager.UpdateCenterText("You suck");
            EventsManager.EndGameUi();

        }

        void Win_Enter()
        {
            EventsManager.UpdateCenterText("You won bitch");
        }
    }
}