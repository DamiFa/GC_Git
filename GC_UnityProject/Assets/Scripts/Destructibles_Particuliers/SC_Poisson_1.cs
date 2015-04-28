using UnityEngine;
using System.Collections;

public class SC_Poisson_1 : MonoBehaviour {

	private int i_State = 0; //0 = repos, 1 = hooked, 2 = détruit
	private int i_LvlSC = 0;
	private float f_speed;
	private Transform _A;
	private Transform _B;
	private bool _TrueStartLaunched = false;
	public float f_LimitFromLeft;
	public float f_LimitFromRight;
	private float f_Dir;
	private GameObject _Player;
	private Vector3 _StartPos;

	// Use this for initialization
	void Start () 
	{
		_A = GameObject.Find("_A").transform;
		_B = GameObject.Find("_B").transform;
	}

	public void TrueStartSousScript ()
	{
		_StartPos = transform.position;
		f_Dir = 1;
		i_State = 0;
		i_LvlSC = this.GetComponent<SC_Destructible>().i_Lvl;
		f_speed = this.GetComponent<SC_Destructible>()._LvlCurves[i_LvlSC].Evaluate(0);
		f_LimitFromLeft = this.GetComponent<SC_Destructible>()._LvlCurves[i_LvlSC].Evaluate(1);
		f_LimitFromRight = this.GetComponent<SC_Destructible>()._LvlCurves[i_LvlSC].Evaluate(2);
		_TrueStartLaunched = true;
	}

	// Update is called once per frame
	void Update () 
	{
		if(_TrueStartLaunched)
		{
			switch (i_State)
			{
			case 0 :
				transform.position = new Vector3(transform.position.x + (f_speed * f_Dir * Time.deltaTime), transform.position.y, transform.position.z);

				if(this.transform.position.x > _B.position.x - f_LimitFromRight)
				{
					f_Dir = -1;
				}

				if(this.transform.position.x < _A.position.x + f_LimitFromLeft)
				{
					f_Dir = 1;
				}

				break;

			case 1 :

				if(i_LvlSC == 1)
				{
					transform.position = new Vector3(transform.position.x, transform.position.y + (f_speed * -1f * Time.deltaTime), transform.position.z);
				}
				else if(i_LvlSC == 2)
				{
					_Player = GameObject.FindGameObjectWithTag("Player");
					transform.position = Vector3.MoveTowards(transform.position, _Player.transform.position, f_speed * Time.deltaTime);
				}

				break;

			case 2 :

				break;

			default :
				break;
			}
		}
	}

	public void BeenHookedSousScript ()
	{
		i_State = 1;
	}

	public void DestroyItSousScript ()
	{
		i_State = 2;
	}

	public void ReinitialiseSousScript ()
	{
		transform.position = _StartPos;
		i_State = 0;
		_TrueStartLaunched = false;
	}
}
