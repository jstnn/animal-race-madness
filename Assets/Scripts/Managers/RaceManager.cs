using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using MonsterLove.StateMachine;
using SimpleJSON;
using System.Collections;

namespace ARM
{

    public class RaceManager : MonoBehaviour
    {
		public int totalWaypoints = 127;
        int raceWidth = 25;
        int playerCount = 4;
        int yStartPosition = 2;
        //int zStartPosition = 0;

        public static List<string> playerTypeList = new List<string>();
        public List<GameObject> players = new List<GameObject>();
        public Dictionary<int, GameObject> positions = new Dictionary<int, GameObject>();

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
        float maxTime = 20;
        public static int maxLaps = 3;

        void OnEnable()
        {
            EventsManager.ResetPlayersEvent += ResetPlayersEvent;
			EventsManager.AddTimeEvent += AddTimeEvent;
        }
        void OnDisable()
        {
            EventsManager.ResetPlayersEvent -= ResetPlayersEvent;
			EventsManager.AddTimeEvent -= AddTimeEvent;
        }
   

        void Awake()
        {
			//Initialize State Machine Engine       
            fsm = StateMachine<States>.Initialize(this, States.Init);

            //animal data
			string playersData = RaceManager.GetStringFromFile("animal-info");
            var data = JSON.Parse(playersData);
            var types = data["animals"];
            foreach (KeyValuePair<string, JSONNode> kvp in types)
            {
                playerTypeList.Add(kvp.Value["type"].Value);

            }

        }


        public void Create()
        {
            ResetPlayersEvent();

            // Instantiate Main Player
            InstantiatePlayer(PlayerPrefs.GetString("playerType"), PlayerPrefs.GetString("playerId"), true, PlayerPrefs.GetString("playerName"));

            // Instantiate NPC Players
            for (int count = 0; count < playerCount; count++)
            {
                string randomPlayerType = playerTypeList[Random.Range(1, playerTypeList.Count)];
                InstantiatePlayer(randomPlayerType, System.Guid.NewGuid().ToString(), false, "NPC " + randomPlayerType);
            }

            PositionPlayers();

            // set last checkpoint
			GameObject.Find($"Waypoint {totalWaypoints}").GetComponent<Checkpoint>().lastCheckpoint = true;

        }

        public void ResetPlayersEvent()
        {
            foreach (GameObject player in players)
            {
                Destroy(player);
            }
            players.Clear();
            positions.Clear();
        }

        void PositionPlayers()
        {
			Debug.Log(players);
			players.Sort((a, b) => 1 - 2 * Random.Range(0, 1));
			Debug.Log("-----");
			Debug.Log(players);
            int trackWidth = raceWidth / players.Count;
            int firstPosition = 1;
            int offsetTrack = trackWidth / 2;
			Vector3 startPosition = GameObject.Find("Start").transform.position;

            foreach (GameObject player in players)
            {
                int playerPosition = (firstPosition * trackWidth) - offsetTrack;
				player.transform.position = new Vector3(playerPosition, player.transform.position.y + yStartPosition, startPosition.z);
                firstPosition++;
            }
        }

        void InstantiatePlayer(string animalType, string animalId, bool isMainPlayer, string animalName)
        {

			string prefabPath = $"animals/{animalType}";

            GameObject instance = Instantiate(Resources.Load(prefabPath, typeof(GameObject))) as GameObject;
            instance.tag = "Player";
            players.Add(instance);
            positions.Add(positions.Count + 1, instance);

            Player player = instance.AddComponent<Player>();
            player.playerType = animalType;
            player.uuid = animalId;
            player.mainPlayer = isMainPlayer;
            player.playerName = animalName;

            if (player.mainPlayer) {
                EventsManager.SwitchToRaceCam(instance);
                instance.AddComponent<PlayerController>();
            } else {
                instance.AddComponent<NPCController>();
            }

            instance.AddComponent<PlayerAnimation>();

        }




        //// from Main.cs
		void AddTimeEvent(float time)
        {
            currentTime += time;
        }

        public string PositionText()
        {
            var ordered = positions.OrderBy(x => x.Value.GetComponent<Player>().currentLap * 1000 + x.Value.GetComponent<Player>().currentCheckpoint).Reverse();
            string text = "";
            int i = 1;
            foreach (KeyValuePair<int, GameObject> p in ordered)
            {
                Player player = p.Value.GetComponent<Player>();
                string mainPlayerMark = player.mainPlayer ? "***" : "";
                string playerText = $"\n{i}->{ mainPlayerMark } { player.playerType } ({ player.currentLap * 1000 + player.currentCheckpoint })";
                text += playerText;
                i++;
            }
            return text;
        }

        void Init_Enter()
        {
            EventsManager.SwitchToGeneralCam();
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
            yield return new WaitForSeconds(5f);
            EventsManager.StartRace();
            yield return new WaitForSeconds(0.1f);
            EventsManager.UpdateCenterText("Starting in 3...");
            yield return new WaitForSeconds(0.5f);
            EventsManager.UpdateCenterText("Starting in 2...");
            yield return new WaitForSeconds(0.5f);
            EventsManager.UpdateCenterText("Starting in 1...");
            yield return new WaitForSeconds(0.5f);
            EventsManager.UpdateCenterText("Run");
            fsm.ChangeState(States.Play);
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
            EventsManager.SwitchUiContext("finish");

        }

        void Win_Enter()
        {
            EventsManager.UpdateCenterText("You won bitch");
        }

        public static string GetStringFromFile(string filename)
        {
            //Load the text file using Reources.Load
            TextAsset theTextFile = Resources.Load<TextAsset>(filename);

            //There's a text file named filename, lets get it's contents and return it
            if (theTextFile != null)
            {
                return theTextFile.text;
            }
            else
            {
                //There's no file, return an empty string.
                return "";
            }
        }
        
    }
}