using UnityEngine;
using System;
using MarkLight;
using MarkLight.Views.UI;

namespace ARM
{
	public class SelectAnimal : UIView
	{
		public Label animalInfo;
		public ObservableList<Animal> Animals;
		RaceManager main;


		public override void Initialize()
        {
            base.Initialize();
			Animals = new ObservableList<Animal>();
        }

		void Start() {
			main = GameObject.Find("RaceManager").GetComponent<RaceManager>();
			foreach (string animalName in RaceManager.playerTypeList.ToArray())
            {
                Animals.Add(new Animal { name = animalName });
            }   
		}

		void AnimalSelected()
		{
			var selectedAnimal = Animals.SelectedItem;
			SetValue(() => animalInfo.Text, "Selected a "+ selectedAnimal.name);
			PlayerPrefs.SetString("playerId", Guid.NewGuid().ToString());
			PlayerPrefs.SetString("playerType", selectedAnimal.name);
			PlayerPrefs.SetString("playerName", "Vicente");

		}
		void StartRace()
		{
			main.fsm.ChangeState(RaceManager.States.Countdown);
			EventsManager.SwitchUiContext("start");
		}
	}
}
