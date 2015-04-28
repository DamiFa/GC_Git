using UnityEngine;
using System.Collections;

public class SC_MissilesDavid : MonoBehaviour {

	public float f_Speed;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position = new Vector3(transform.position.x, transform.position.y + 1 * f_Speed * 0.01f, transform.position.z);

		if(transform.position.y > 20f) 
		{
			Destroy(this.gameObject); 
		}
	}
}
