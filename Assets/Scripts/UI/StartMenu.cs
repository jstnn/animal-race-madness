using MarkLight.Views.UI;
using MonsterLove.StateMachine;
using UnityEngine;

namespace ARM
{
    public class StartMenu : UIView
    {
        public ViewSwitcher ContentViewSwitcher;
		StateMachine<RaceManager.States> gameState;

		void OnEnable()
        {
            EventsManager.SwitchUiContextEvent += SwitchUiContextEvent;
        }

        void OnDisable()
        {
            EventsManager.SwitchUiContextEvent -= SwitchUiContextEvent;
        }
              
		void Start()
        {
            // main = GameObject.Find("RaceManager").GetComponent<Main>();
			gameState = GameObject.Find("RaceManager").GetComponent<RaceManager>().fsm;
        }

        void SwitchUiContextEvent(string context)
        {
            switch (context)
            {
                case "select":
                    ContentViewSwitcher.SwitchTo(1);
					gameState.ChangeState(RaceManager.States.Select);
					break;
                case "start":
					ContentViewSwitcher.SwitchTo(2);
                    // state of play mode is managed in Main
					break;
                case "back":
                    ContentViewSwitcher.Previous();
					gameState.ChangeState(RaceManager.States.Init);
					break;
                case "restart":
                    ContentViewSwitcher.SwitchTo(1);
					gameState.ChangeState(RaceManager.States.Select);
					break;
				case "finish":
					break;
				default:
                    break;
            }
        }

		public void SelectMenu() {
			SwitchUiContextEvent("select");
		}

	}
}

