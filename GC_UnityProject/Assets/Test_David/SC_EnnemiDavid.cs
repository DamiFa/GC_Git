using UnityEngine;
using System.Collections;

public class SC_EnnemiDavid : MonoBehaviour {

	public float f_Speed;
	public int direction;
	public int vie;
	private bool b_Mort;

	// Use this for initialization
	void Start ()
	{
		direction = 1;
		transform.position = new Vector3 (0, 9, 0);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!b_Mort)
		{
			transform.position = new Vector3(transform.position.x + direction * f_Speed * 0.01f, transform.position.y, transform.position.z);

			if(transform.position.x < -7.5f)
				direction = 1;

			if(transform.position.x > 7.5f)
				direction = -1;
		}

		if(vie <= 0)
			b_Mort = true;
	}
	
	void OnTriggerEnter (Collider col)
	{
		Debug.Log("ouch");
		vie -= 1;
	}

	void OnGUI ()
	{
		if(b_Mort)
		{
			GUI.Box(new Rect(Screen.width * 0.5f - Screen.width * 0.5f * 0.5f, Screen.height * 0.45f, Screen.width * 0.5f, Screen.height * 0.45f), "Bravo, tu es très fort !");
		}
	}
}
