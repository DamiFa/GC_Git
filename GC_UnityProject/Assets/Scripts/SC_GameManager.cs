using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SC_GameManager : MonoBehaviour {

	//S'occupe du timer, de la fin de jeu ...

	private float f_StartTime;
	private float f_CurrentTime;
	private float f_Timer;
	public float f_TimeToEnd;
	public GUIStyle _TimerGUIStyle;
	public bool b_GameEnded;

	// Use this for initialization
	void Start () 
	{
		Time.timeScale = 1f;

		f_StartTime = Time.time;
		b_GameEnded = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		f_CurrentTime = Time.time - f_StartTime;

		TimerHandeler ();
	}

	private void TimerHandeler ()
	{
		f_Timer = f_TimeToEnd - f_CurrentTime;

		if(f_Timer < 0.1f)
		{
			if(!b_GameEnded)
			{
				EndOfGame ();
			}
		}
	}

	public void AddTimeToTimer (float f_TimeToAdd)
	{
		f_TimeToEnd += f_TimeToAdd;
	}

	public void EndOfGame ()
	{
		b_GameEnded = true;

		print("MORT");

		Time.timeScale = 0f;
	}

	void OnGUI ()
	{
		string minutes = Mathf.Floor(f_Timer / 60).ToString("00");
		string seconds = (f_Timer % 60).ToString("00");

		GUI.Label(new Rect(20f, 10f, 80f, 30f), minutes + ":" + seconds, _TimerGUIStyle);
	}
}
