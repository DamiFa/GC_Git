using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SC_Atelier : MonoBehaviour {

	public List<GameObject> a_Destructibles;
	public float f_Length;

	// Use this for initialization
	void Start () 
	{
		a_Destructibles = new List<GameObject>();

		for(int i = 0; i < transform.childCount; i++)
		{
			if(transform.GetChild(i).CompareTag("Destructible"))
			{
				a_Destructibles.Add(transform.GetChild(i).gameObject);
			}
		}
	}

	public void Activation ()
	{
		for(int i = 0; i < a_Destructibles.Count; i++)
		{
			a_Destructibles[i].GetComponent<SC_Destructible>().TrueStart();
		}
	}

	public void Desactivation ()
	{
		for(int i = 0; i < a_Destructibles.Count; i++)
		{
			a_Destructibles[i].GetComponent<SC_Destructible>().Reinitialise();
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
