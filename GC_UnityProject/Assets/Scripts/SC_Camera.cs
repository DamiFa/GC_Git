using UnityEngine;
using System.Collections;

public class SC_Camera : MonoBehaviour {

	private GameObject _Player;
	public float f_Speed;

	// Use this for initialization
	void Start () 
	{
		_Player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, _Player.transform.position.y - 5.5f, -10f), f_Speed * Time.deltaTime);
	}
}
