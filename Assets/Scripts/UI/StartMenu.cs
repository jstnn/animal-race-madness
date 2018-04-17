using MarkLight.Views.UI;
using MarkLight;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace ARM
{
    public class StartMenu : UIView
    {
        public ViewSwitcher ContentViewSwitcher;
        public ObservableList<Animal> Animals;
        public List AnimalsList;
        public Label PositionText;
        public Label CenterText;
        public Label TimeText;
        public Button RestartButton;
        Main main;

        public override void Initialize()
        {
            base.Initialize();
            Animals = new ObservableList<Animal>();
            RestartButton.enabled = false;
        }

        void OnEnable()
        {
            EventsManager.UpdatePositionTextEvent += UpdatePositionTextEvent;
            EventsManager.UpdateCenterTextEvent += UpdateCenterTextEvent;
            EventsManager.UpdateTimeTextEvent += UpdateTimeTextEvent;
            EventsManager.EndGameUiEvent += EndGameUiEvent;
        }
        void OnDisable()
        {
            EventsManager.UpdatePositionTextEvent -= UpdatePositionTextEvent;
            EventsManager.UpdateCenterTextEvent -= UpdateCenterTextEvent;
            EventsManager.UpdateTimeTextEvent -= UpdateTimeTextEvent;
            EventsManager.EndGameUiEvent -= EndGameUiEvent;
        }


        void EndGameUiEvent()
        {
            PositionText.enabled = false;
            RestartButton.enabled = true;
        }

        void UpdatePositionTextEvent(string text)
        {
            if (PositionText != null)
            {
                SetValue(() => PositionText.Text, text);
            }
        }

        void UpdateCenterTextEvent(string text)
        {
            if (CenterText != null)
            {
                SetValue(() => CenterText.Text, text);
            }
        }

        void UpdateTimeTextEvent(string text)
        {
            if (TimeText != null)
            {
                SetValue(() => TimeText.Text, text);
            }
        }

        void Start()
        {
            string[] playerListType = RaceManager.playerTypeList.ToArray();
            main = GameObject.Find("RaceManager").GetComponent<Main>();
            foreach (string animalName in RaceManager.playerTypeList.ToArray())
            {
                Animals.Add(new Animal { name = animalName });
            }   
        }

        void AnimalSelected()
        {
            var selectedAnimal = Animals.SelectedItem;
            Debug.Log(selectedAnimal);
            PlayerPrefs.SetString("playerId", System.Guid.NewGuid().ToString());
            PlayerPrefs.SetString("playerType", selectedAnimal.name);
            PlayerPrefs.SetString("playerName", "Vicente");
            Debug.Log("You choose " + selectedAnimal.name);
            main.fsm.ChangeState(Main.States.Countdown);
            OnlinePlay();
        }

        public void Play()
        {
            ContentViewSwitcher.SwitchTo(1);
            main.fsm.ChangeState(Main.States.Select);
        }

        void SelectMenu()
        {
            ContentViewSwitcher.SwitchTo(1);
            main.fsm.ChangeState(Main.States.Select);
        }

        public void OnlinePlay()
        {
            ContentViewSwitcher.SwitchTo(2);
            RestartButton.enabled = false;
        }

        public void Back()
        {
            ContentViewSwitcher.Previous();
            main.fsm.ChangeState(Main.States.Init);
        }

        public void Restart() {
            ContentViewSwitcher.SwitchTo(1);
            main.fsm.ChangeState(Main.States.Select);
        }


	}
}

