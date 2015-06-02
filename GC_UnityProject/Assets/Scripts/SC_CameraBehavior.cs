using UnityEngine;
using System.Collections;

public class SC_CameraBehavior : MonoBehaviour {
	
	private Vector3 _StartPos;
	
	private Vector3 v_tiltDirection;
	
	private float f_timer;
	public bool b_IsTilting;
	
	private float f_startCamSize;
	private float f_camSizeDirection;

	private GameObject _Player;
	public float f_Speed;
	
	// Use this for initialization
	void Start () 
	{
		_Player = GameObject.FindGameObjectWithTag("Player");

		_StartPos = transform.position;
		
		f_startCamSize = this.GetComponent<Camera>().orthographicSize;
		
		b_IsTilting = false;
		
		f_camSizeDirection = f_startCamSize;
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, _Player.transform.position.y - 5.5f, -10f), f_Speed * Time.deltaTime);
	}
	
	private void tilt()
	{
		transform.position = Vector3.Lerp(transform.position, 
		                                  new Vector3 (transform.position.x + v_tiltDirection.x,
		             transform.position.y + v_tiltDirection.y, 
		             transform.position.z + v_tiltDirection.z), 
		                                  20f*Time.deltaTime);
		
		GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, f_camSizeDirection, 2f*Time.deltaTime);
	}
}
