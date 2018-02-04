using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using MonsterLove.StateMachine;

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

    public int selGridInt = 0;

	public float health = 1000;
	public float damage = 1;

	float startHealth;

	public StateMachine<States> fsm;
    RaceManager manager;

	void Awake()
	{
		startHealth = health;
       
		//Initialize State Machine Engine		
		fsm = StateMachine<States>.Initialize(this, States.Init);

	}

    private void Start()
    {
        manager = GameObject.Find("RaceManager").GetComponent<RaceManager>();
    }


    public string PositionText() {
        var ordered = manager.positions.OrderBy(x => x.Value.transform.position.z*-1);
        string text = "";
        int i = 1;
        foreach(KeyValuePair<int, GameObject> p in ordered) {
            text += "\n"+ i++ + " -> " + (p.Value.GetComponent<Animal>().mainPlayer ? "*** ": "") + p.Value.name.Replace("(Clone)", "");
                
        }
        return text;
    }

    

    void OnGUI()
	{
		//Example of polling state 
		var state = fsm.State;

		GUILayout.BeginArea(new Rect(30,30,600,400));

		if(state == States.Init && GUILayout.Button("Start"))
		{
            fsm.ChangeState(States.Select);
		}
        if (state == States.Select)
        {
            GUILayout.Label("Select your dude");
            GUILayout.BeginVertical("Box");
            selGridInt = GUILayout.SelectionGrid(selGridInt, RaceManager.playerTypeList.ToArray(), 5);
            if (GUILayout.Button("Start")) {
                string choosenType = RaceManager.playerTypeList.ToArray()[selGridInt];
                PlayerPrefs.SetString("playerId", System.Guid.NewGuid().ToString());
                PlayerPrefs.SetString("playerType", choosenType);
                PlayerPrefs.SetString("playerName", "Vicente");
                Debug.Log("You choose " + choosenType);
                fsm.ChangeState(States.Countdown);
            }
            GUILayout.EndVertical();
        }
        if(state == States.Countdown)
		{
            GUILayout.Label("Look at Console");
		}
		if(state == States.Play)
		{
			if(GUILayout.Button("Force Win"))
			{
				fsm.ChangeState(States.Win);
			}

            GUILayout.Label("Time: " + Mathf.Round(health).ToString() + PositionText());
		}
		if(state == States.Win || state == States.Lose)
		{
			if(GUILayout.Button("Play Again"))
			{
				fsm.ChangeState(States.Countdown);
			}
		}

		GUILayout.EndArea();
	}

	private void Init_Enter()
	{
		Debug.Log("Waiting for start button to be pressed");
	}

    private void Select_Enter() {
       
    }

	//We can return a coroutine, this is useful animations and the like
	private IEnumerator Countdown_Enter()
	{
        GameObject.Find("RaceManager").GetComponent<RaceManager>().Create();
		health = startHealth;

		Debug.Log("Starting in 3...");
		yield return new WaitForSeconds(0.5f);
		Debug.Log("Starting in 2...");
		yield return new WaitForSeconds(0.5f);
		Debug.Log("Starting in 1...");
		yield return new WaitForSeconds(0.5f);

		fsm.ChangeState(States.Play);

	}


	private void Play_Enter()
	{
		Debug.Log("RUN!");
	}

	private void Play_Update()
	{
		health -= damage * Time.deltaTime;

		if(health < 0)
		{
			fsm.ChangeState(States.Lose);
		}
	}

	void Play_Exit()
	{
		Debug.Log("Game Over");
	}

	void Lose_Enter()
	{
		Debug.Log("Lost");
	}

	void Win_Enter()
	{
		Debug.Log("Won");
	}
}
