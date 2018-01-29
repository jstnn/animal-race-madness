using UnityEngine;
using System.Collections.Generic;
using SimpleJSON;

public class RaceManager : MonoBehaviour {
	public List<GameObject> players = new List<GameObject>();

	public int raceWidth = 25;
    public int playerCount=4;
	public int yStartPosition;
	public int zStartPosition;

	static float colliderFactor=0.5f;

    public static List<string> playerTypeList = new List<string>(); 


    void Start() {
        // get all animal types from json
        string playersData = PlayerController.Read("animations");
        var data = JSON.Parse(playersData);
        var types = data["playerTypes"];
        foreach (KeyValuePair<string, JSONNode> kvp in types)
        {
            playerTypeList.Add(kvp.Value["type"].Value);

        }
    }


	// Use this for initialization
    public void Create () {
        // Instantiate Main Player
        InstantiatePlayer(PlayerPrefs.GetString("playerType"), PlayerPrefs.GetString("playerId"), true, PlayerPrefs.GetString("playerName"));

        // Instantiate NPC Players
        for (int count = 0; count < playerCount; count++) {
            string randomPlayerType = playerTypeList[Random.Range(1, playerTypeList.Count)];
            Debug.Log("@@@@@@@@@@" + randomPlayerType);
            InstantiatePlayer(randomPlayerType, System.Guid.NewGuid().ToString(), false, "NPC "+randomPlayerType);
        }

        PositionPlayers();

	}

	void PositionPlayers() {
		int trackWidth = raceWidth / players.Count;
		int firstPosition = 1;
		int offsetTrack = trackWidth / 2;
		foreach (GameObject player in players) {
			int playerPosition = (firstPosition * trackWidth) - offsetTrack;
			player.transform.position = new Vector3 (playerPosition, player.transform.position.y + yStartPosition, player.transform.position.z + zStartPosition);
			firstPosition++;
		}
	}

	static void FitToChildren(BoxCollider rootGameObject) {
		bool hasBounds = false;
		Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);

		for (int i = 0; i < rootGameObject.transform.childCount; ++i) {
			Renderer childRenderer = rootGameObject.transform.GetChild(i).GetComponent<Renderer>();
			if (childRenderer != null) {
				if (hasBounds) {
					bounds.Encapsulate(childRenderer.bounds);
				}
				else {
					bounds = childRenderer.bounds;
					hasBounds = true;
				}
			}
		}

		BoxCollider collider = (BoxCollider)rootGameObject.GetComponent<Collider>();
		collider.center = bounds.center - rootGameObject.transform.position;
		collider.size = bounds.size * colliderFactor;

	}

    void InstantiatePlayer(string animalType, string animalId, bool isMainPlayer, string animalName) {

        Debug.Log(animalType + animalId + isMainPlayer + animalName);
        string prefabName = "animals/" + animalType + "/FBX FILES/" + animalType;

        GameObject instance = Instantiate(Resources.Load(prefabName, typeof(GameObject))) as GameObject;
        // instance.transform.position = new Vector3(Random.Range(0, 25f), 5, 5);

        players.Add(instance);

        Animal player = instance.AddComponent<Animal>();
        player.playerType = animalType;
        player.uuid = animalId;
        player.mainPlayer = isMainPlayer;
        player.playerName = animalName;


        if (player.mainPlayer)
        {
            // add controls
            instance.AddComponent<PlayerController>();

            // camera follow main player
            CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
            cameraFollow.target = instance.transform;
        }

    }
}
