using UnityEngine;
using System.Collections;

public class SC_Barrel_1 : MonoBehaviour {

	public GameObject _Explosion;
	public GameObject _Bulles;

	// Use this for initialization
	void Start () 
	{
		if(_Explosion == null)
		{
			_Explosion = transform.FindChild("Explosion_1").gameObject;
		}
		if(_Bulles == null)
		{
			_Bulles = transform.FindChild("Bulles").gameObject;
		}
	}

	public void TrueStartSousScript ()
	{
		if(_Explosion == null)
		{
			_Explosion = transform.FindChild("Explosion_1").gameObject;
		}

//		_Explosion.transform.position = transform.position;
		_Explosion.SetActive(false);

		if(_Bulles == null)
		{
			_Bulles = transform.FindChild("Bulles").gameObject;
		}
		_Bulles.SetActive(true);
	}

	public void DestroyItSousScript ()
	{
		_Bulles.SetActive(false);
//		_Explosion.transform.position = transform.position;
		_Explosion.SetActive(true);
	}

	public void ReinitialiseSousScript ()
	{
		_Explosion.SetActive(false);
		_Bulles.SetActive(false);
	}

	public void BeenHookedSousScript ()
	{

	}

	// Update is called once per frame
	void Update () 
	{
	
	}
}
