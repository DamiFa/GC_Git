using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SC_Player : MonoBehaviour {
	
	private float _VelocityX;
	private float _VelocityY;
	[HideInInspector]
	public float _Chute;
	public float _FallingSpeed;
	public AnimationCurve _FallingStrenght;
	private float _Tilt;
	public float _TiltSpeed;
	public AnimationCurve _TiltStrenght;
	private Vector3 _DirGrappin;
	public float f_GrappinStrenght;
	private float f_CurrentTime;
	private float f_StartTime;
	private GameObject _Grappin;
	public GameObject _Chaine;
	public float f_DistancePlayerChain;
	private Vector3 _PosChain;
	private Vector3 _DirAncor;
	public List<GameObject> a_Destructibles;
	private GameObject _ATELIER_MANAGER;
	private GameObject _GAME_MANAGER;
	public float f_DistanceToDie;
	public bool b_Invicible = false;

	// Use this for initialization
	void Start () 
	{
		_ATELIER_MANAGER = GameObject.FindGameObjectWithTag("ATELIER_MANAGER");
		_GAME_MANAGER = GameObject.FindGameObjectWithTag("GAME_MANAGER");

		_Chute = 1f;

		_Grappin = GameObject.FindGameObjectWithTag("Grappin");
		f_StartTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () 
	{
		a_Destructibles = _ATELIER_MANAGER.GetComponent<SC_AtelierManager>().a_DestructiblesManager;

		f_CurrentTime = Time.time - f_StartTime;

		_PosChain = new Vector3(transform.position.x,
		                        transform.position.y - 1.1f,
		                        transform.position.z);

		_Chaine.transform.position = _PosChain;

		FallingMovement ();
		TiltMovement ();
		ChainHandeler ();

		if(_Grappin.GetComponent<SC_Grappin>().i_GrappinState == 2)
		{
			GrappinMouvement ();
		}
		else
		{
			_DirGrappin = Vector3.zero;
		}

		//Chute
		_VelocityX = _Tilt + _DirGrappin.x;
		_VelocityY = _Chute + _DirGrappin.y;

		//Clamp Position
		transform.position = new Vector3(Mathf.Clamp(transform.position.x, -4.9f, 4.9f),transform.position.y, transform.position.z);

		//MORT
		if(!b_Invicible)
		{
			for(int i = 0; i < a_Destructibles.Count; i++)
			{
				if(Vector3.Distance(transform.position, a_Destructibles[i].transform.position) < f_DistanceToDie)
				{
					if(!a_Destructibles[i].GetComponent<SC_Destructible>().b_Destroyed)
					{
						if(!_GAME_MANAGER.GetComponent<SC_GameManager>().b_GameEnded)
						{
							StartCoroutine("Mort");
						}
					}
				}
			}
		}
	}
	
	void FixedUpdate()
	{
		GetComponent<Rigidbody>().velocity = new Vector3(_VelocityX, _VelocityY, 0);
	}

	private void TiltMovement ()
	{
		_Tilt = _TiltStrenght.Evaluate(Mathf.Abs(Input.acceleration.x)) * Mathf.Sign(Input.acceleration.x) * _TiltSpeed;
	}

	private void FallingMovement ()
	{
		_Chute = -_FallingStrenght.Evaluate(f_CurrentTime/180f) * _FallingSpeed;
	}

	private void GrappinMouvement ()
	{
		Vector3 dirTemp = -new Vector3(transform.position.x - _Grappin.transform.position.x,
		                               transform.position.y - _Grappin.transform.position.y,
		                               0);

		_DirGrappin = dirTemp * f_GrappinStrenght * 0.05f;
	}

	private void ChainHandeler ()
	{
		f_DistancePlayerChain = Vector3.Distance(_PosChain, _Grappin.transform.position);

		_DirAncor = _Chaine.transform.position - _Grappin.transform.position;

		_Chaine.transform.localScale = new Vector3(_Chaine.transform.localScale.x,
		                                           f_DistancePlayerChain + (0.2f * f_DistancePlayerChain),
		                                           _Chaine.transform.localScale.z);

		if(_DirAncor != Vector3.zero)
		{
			if(_DirAncor.x >= 0)
			{
				_Chaine.transform.eulerAngles = new Vector3( 0, 0, Vector3.Angle(_DirAncor, -Vector3.up));
			}
			else
			{
				_Chaine.transform.eulerAngles = new Vector3( 0, 0, - Vector3.Angle(_DirAncor, -Vector3.up));
			}
		}
	}

	IEnumerator Mort()
	{
		_GAME_MANAGER.GetComponent<SC_GameManager>().EndOfGame();

		yield break;
	}
}
