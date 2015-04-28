using UnityEngine;
using System.Collections;

public class SC_PlayerDavid : MonoBehaviour {

	public float f_Speed;
	public GameObject _GO_Missile;

	// Use this for initialization
	void Start () 
	{
		transform.position = new Vector3 (0, 1, 0);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKey(KeyCode.LeftArrow))
		{
			if(transform.position.x > -8.5f)
				transform.position = new Vector3(transform.position.x - 1 * f_Speed * 0.01f, transform.position.y, transform.position.z);
		}

		if(Input.GetKey(KeyCode.RightArrow))
		{
			if(transform.position.x < 8.5f)
				transform.position = new Vector3(transform.position.x + 1 * f_Speed * 0.01f, transform.position.y, transform.position.z);
		}

		if(Input.GetKeyDown(KeyCode.Space))
		{
			Instantiate(_GO_Missile, this.transform.position, Quaternion.identity);
		}
	}
}
