using UnityEngine;
using System.Collections.Generic;
using SimpleJSON;

namespace ARM
{

    public class RaceManager : MonoBehaviour
    {

        int raceWidth = 25;
        int playerCount = 4;
        int yStartPosition = 2;
        //int zStartPosition = 0;

        public static List<string> playerTypeList = new List<string>();
        public List<GameObject> players = new List<GameObject>();
        public Dictionary<int, GameObject> positions = new Dictionary<int, GameObject>();

        void OnEnable()
        {
            EventsManager.ResetPlayersEvent += ResetPlayersEvent;
        }
        void OnDisable()
        {
            EventsManager.ResetPlayersEvent -= ResetPlayersEvent;
        }
   

        void Awake()
        {
            string playersData = Main.GetStringFromFile("animal-info");
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
    }
}