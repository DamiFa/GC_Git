using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SC_Grappin : MonoBehaviour {

	private Vector3 _PositionToBe;
	private GameObject _Player;
	private Vector3 _V3_fingerPos;
	private Vector2 _V2_fPos;
	public int i_GrappinState; //0 = Repos, 1 = Lancé, 2 = Accroché, 3 = Reviens, 4 = Accroché à rien
	private int i_NbTouches;
	private int i_PreviousNbTouches;
	public AnimationCurve _GrappinStrenght_ALLER;
	public AnimationCurve _GrappinStrenght_RETOUR;
	public float f_Speed_ALLER;
	public float f_Speed_RETOUR;
	public Camera _Cam;
	public List<GameObject> a_Destructibles;
	public GameObject _CurrentlyLinkedTo;
	private Vector3 _PosRepos;
	public float f_OffSet;
	private Vector3 _DirAncor;
	private GameObject _ATELIER_MANAGER;

	// Use this for initialization
	void Start () 
	{
		_Player = GameObject.FindGameObjectWithTag("Player");
		_ATELIER_MANAGER = GameObject.FindGameObjectWithTag("ATELIER_MANAGER");

		i_NbTouches = Input.touchCount;

		i_GrappinState = 0;
		_PositionToBe = new Vector3(_Player.transform.position.x, _Player.transform.position.y - f_OffSet, _Player.transform.position.z);
	}
	
	// Update is called once per frame
	void Update ()
	{
		a_Destructibles = _ATELIER_MANAGER.GetComponent<SC_AtelierManager>().a_DestructiblesManager;

		i_PreviousNbTouches = i_NbTouches;
		i_NbTouches = Input.touchCount;

		if(i_NbTouches >= 3 && i_PreviousNbTouches == 0)
		{
			Application.LoadLevel(Application.loadedLevel);
		}

		if(i_NbTouches >= 1 && i_PreviousNbTouches == 0)
		{
			_V2_fPos = Input.GetTouch(0).position;
			_V3_fingerPos = _Cam.ScreenToWorldPoint(new Vector3 (_V2_fPos.x, _V2_fPos.y, 10));

			if(i_GrappinState != 1)
			{
				StartCoroutine(LancerGrappin(_V3_fingerPos));
			}
		}

        if (Input.GetMouseButtonDown(0))
        {
            _V2_fPos = Input.mousePosition;
            _V3_fingerPos = _Cam.ScreenToWorldPoint(new Vector3(_V2_fPos.x, _V2_fPos.y, 10));

            if (i_GrappinState != 1)
            {
                StartCoroutine(LancerGrappin(_V3_fingerPos));
            }
        }

		if(i_NbTouches == 0 && i_PreviousNbTouches > 0 || Input.GetMouseButtonUp(0))
		{
			StartCoroutine("RamenerGrappin");
		}

		//Au repos
		if(i_GrappinState != 1)
		{
			if(transform.position.y > _Player.transform.position.y + 4)
			{
				if(i_GrappinState == 2 || i_GrappinState == 4)
				{
					if(i_GrappinState == 2)
					{
						Decrocher ();
					}
					i_GrappinState = 0;
					_PositionToBe = new Vector3(_Player.transform.position.x, _Player.transform.position.y - f_OffSet, _Player.transform.position.z);
				}
			}

			if(Vector3.Distance(transform.position, _Player.transform.position) < f_OffSet + 0.2f && i_GrappinState != 2)
			{	
				i_GrappinState = 0;
				_PositionToBe = new Vector3(_Player.transform.position.x, _Player.transform.position.y - f_OffSet, _Player.transform.position.z);
			}
		}

		//L'accroche
		if(i_GrappinState == 1)
		{
			for(int i = 0; i < a_Destructibles.Count; i++)
			{
				if(Vector3.Distance(transform.position, a_Destructibles[i].transform.position) < a_Destructibles[i].GetComponent<SC_Destructible>().f_DistanceToHook)
				{
					if(!a_Destructibles[i].GetComponent<SC_Destructible>().b_Destroyed)
					{
						Accrocher (a_Destructibles[i]);
						a_Destructibles[i].GetComponent<SC_Destructible>().BeenHooked();
					}
				}
			}

			//Orientation de l'ancre

			_DirAncor = _V3_fingerPos - transform.position;

			if(_DirAncor != Vector3.zero)
			{
				if(_DirAncor.x >= 0)
				{
					transform.eulerAngles = new Vector3( 0, 0, Vector3.Angle(_DirAncor, -Vector3.up));
				}
				else
				{
					transform.eulerAngles = new Vector3( 0, 0, - Vector3.Angle(_DirAncor, -Vector3.up));
				}
			}
		}
		else
		{
			transform.eulerAngles = Vector3.zero;
		}

		//Garder le grappin placé
		if(i_GrappinState == 0 || i_GrappinState == 2)
		{
			if(i_GrappinState == 2)
			{
				_PositionToBe = _CurrentlyLinkedTo.transform.position;
			}

			PlacerGrappin (_PositionToBe);
		}
	}

	IEnumerator LancerGrappin (Vector3 _Destination)
	{
		//avoir la possiblité d'annuler n'importe quand
		i_GrappinState = 1;

		float duration = 0f;
		float distance = Vector3.Distance(transform.position, _Destination);
		float realspeed = (f_Speed_ALLER + Mathf.Abs(_Player.GetComponent<SC_Player>()._Chute))/distance;
		float security = 0f;
		Vector3 startPos = transform.position;

		while(Vector3.Distance(transform.position, _Destination) > 0.1f && security < 200 && i_GrappinState == 1)
		{
			transform.position = Vector3.Lerp(startPos, _Destination, _GrappinStrenght_ALLER.Evaluate(duration));
			duration += realspeed * Time.deltaTime;
			security ++;
			yield return new WaitForEndOfFrame();
		}

		if(i_GrappinState != 2)
		{
			StartCoroutine(RamenerGrappin());
		}

		yield break;
	}

	IEnumerator RamenerGrappin ()
	{
		//Ramene le grappin en dessous du perso
		if(i_GrappinState == 2)
		{
			Decrocher ();
		}

		i_GrappinState = 3;

		float duration = 0f;
		float distance = Vector3.Distance(transform.position, new Vector3(_Player.transform.position.x, _Player.transform.position.y - f_OffSet, _Player.transform.position.z));
		float realspeed = (f_Speed_RETOUR + Mathf.Abs(_Player.GetComponent<SC_Player>()._Chute))/distance;
		float security = 0f;
		Vector3 startPos = transform.position;
		
		while(Vector3.Distance(transform.position, new Vector3(_Player.transform.position.x, _Player.transform.position.y - f_OffSet, _Player.transform.position.z)) > 0.1f && security < 200)
		{
			transform.position = Vector3.Lerp(startPos, 
			                                  new Vector3(_Player.transform.position.x, _Player.transform.position.y - f_OffSet, _Player.transform.position.z), 
			                                  _GrappinStrenght_RETOUR.Evaluate(duration));

			duration += realspeed * Time.deltaTime;
			security ++;
			yield return new WaitForEndOfFrame();
		}
		
		yield break;
	}

	private void PlacerGrappin (Vector3 _Destination)
	{
		transform.position = _Destination;
	}

	private void Accrocher (GameObject _LinkedTo)
	{
		i_GrappinState = 2;
		_CurrentlyLinkedTo = _LinkedTo;
		_PositionToBe = new Vector3(_LinkedTo.transform.position.x,
		                            _LinkedTo.transform.position.y,
		                            transform.position.z);
	}

	public void RamenerGrappinCoroutine ()
	{
		StartCoroutine("RamenerGrappin");
	}

	private void Decrocher ()
	{
		_CurrentlyLinkedTo.GetComponent<SC_Destructible>().DestroyIt();
	}
}
