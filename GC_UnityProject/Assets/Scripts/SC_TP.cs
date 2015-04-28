using UnityEngine;
using System.Collections;

public class SC_TP : MonoBehaviour 
{
//	private Vector2 _Destination;
//	private float f_Speed;
//	private float f_Delay;
//	private bool b_OnTp;
//	
//	// Use this for initialization
//	void Start () 
//	{
//		b_OnTp = false;
//	}
//	
//	// Update is called once per frame
//	void Update () 
//	{
//		if(!b_OnTp)
//		{
//		 	if(true)
//			{
//				StartCoroutine("TP");
//			}
//		}
//	}
//
//	IEnumerator TP ()
//	{
//		b_OnTp = true;
//		camera.GetComponent<SC_Camera>().GoToDestination(_Destination, f_Delay);
//
//		//Start TP
//		renderer.enabled = false;
//		collider.enabled = false;
//
//		yield return new WaitForSeconds(f_Delay);
//
//		transform.position = _Destination;
//		renderer.enabled = true;
//		collider.enabled = true;
//
//		b_OnTp = false;
//		yield break;
//	}
}
