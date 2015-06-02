using UnityEngine;
using System.Collections;

public class SC_Score : MonoBehaviour {

	public float f_Score;
	public GUIStyle _ScoreGUIStyle;
	private GameObject _Player;
	private float f_PosY;
	private float f_PreviousPosY;
	private float f_DeltaPosY;
	public float f_MultiplicatoDepth;
	public float f_TimerForCombo;

	// Use this for initialization
	void Start () 
	{
		_Player = GameObject.FindGameObjectWithTag("Player");
		f_Score = 0f;

		f_PosY = _Player.transform.position.y;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//GainParPofondeur
		f_PreviousPosY = f_PosY;
		f_PosY = _Player.transform.position.y;

		f_DeltaPosY = f_PreviousPosY - f_PosY;
		AddScore(f_DeltaPosY * f_MultiplicatoDepth);
	}

	public void AddScore (float f_ScoreToAdd, bool b_ByDestroy = false)
	{
		f_Score += f_ScoreToAdd;
	}

	void OnGUI ()
	{
		float f_ScoreToDisplay = Mathf.Round(f_Score);

		GUI.Label(new Rect(250f, 10f, 80f, 30f), f_ScoreToDisplay.ToString(), _ScoreGUIStyle);
	}
}
