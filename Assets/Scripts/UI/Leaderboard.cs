using MarkLight;
using MarkLight.Examples.Data;
using MarkLight.Views.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ARM
{
    public class Leaderboard : UIView
    {
		public ObservableList<Animal> players;
        public DataGrid DataGrid;

        public override void Initialize()
        {
            base.Initialize();

			players = new ObservableList<Animal>();
                     
        }

		void Start()
		{
			RaceManager raceManager = GameObject.Find("RaceManager").GetComponent<RaceManager>();
            foreach (GameObject player in raceManager.players)
            {
                players.Add(new Animal { name = player.name, position = 1 });
            }  
		}

        public void ItemSelected()
        {
            // var selectedItem = Highscores.SelectedItem;
        }

    }
}

