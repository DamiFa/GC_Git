using UnityEngine;
using System.Collections;

public class SC_Destructible : MonoBehaviour {
	
	public float f_Score;
	public float f_Depth;
	public AnimationCurve[] _LvlCurves;
	public AnimationCurve _LvlDpdDepth;
	public int i_State; //0 = repos, 1 = accroché, 2 = Détruit
	public int i_Lvl;
	public GameObject _Visuel;
	public bool b_SousScript;
	public bool b_Destroyed;
	private GameObject _GAME_MANAGER;
	public float f_DistanceToHook = 1f;
	private GameObject _Cam;

	// Use this for initialization
	void Start ()
	{
		_Cam = GameObject.FindGameObjectWithTag("MainCamera");
		_GAME_MANAGER = GameObject.FindGameObjectWithTag("GAME_MANAGER");
	}

	public void TrueStart ()
	{
		i_State = 0;

		f_Depth = Mathf.Abs(transform.position.y);
		b_Destroyed = false;

		i_Lvl = Mathf.FloorToInt(_LvlDpdDepth.Evaluate(f_Depth));

		f_DistanceToHook = _LvlCurves[i_Lvl].Evaluate(5);

		//Je parcours tous les enfant, s'ils sont des visuels je n'affiche que celui qui correspond
		//Je stock le bon dans la variable visuel pour lancer les animations
		for(int i = 0; i < transform.childCount; i ++)
		{
			if(transform.GetChild(i).CompareTag("Visuel"))
			{
				if(transform.GetChild(i).name == i_Lvl.ToString())
				{
					_Visuel = transform.GetChild(i).gameObject;
					_Visuel.SetActive(true);
				}
				else
				{
					transform.GetChild(i).gameObject.SetActive(false);
				}
			}
		}

		if(b_SousScript)
		{
			this.gameObject.SendMessage("TrueStartSousScript");
		}
	}

	// Update is called once per frame
	void Update () 
	{
	
	}

	//Quand l'atelier est désactivé
	public void Reinitialise ()
	{
		if(b_SousScript)
		{
			this.gameObject.SendMessage("ReinitialiseSousScript");
		}

		for(int i = 0; i < transform.childCount; i ++)
		{
			if(transform.GetChild(i).CompareTag("Visuel"))
			{
				transform.GetChild(i).gameObject.SetActive(false);
			}
		}
	}

	public void BeenHooked ()
	{
		if(b_SousScript)
		{
			this.gameObject.SendMessage("BeenHookedSousScript");
		}
	}

	public void DestroyIt ()
	{
		i_State = 2;

		_Cam.GetComponent<SC_CameraBehavior>().b_IsTilting = true;

		_GAME_MANAGER.GetComponent<SC_Score>().AddScore(f_Score, true);
		b_Destroyed = true;
		_Visuel.SetActive(false);

		if(b_SousScript)
		{
			this.gameObject.SendMessage("DestroyItSousScript");
		}
	}
}
