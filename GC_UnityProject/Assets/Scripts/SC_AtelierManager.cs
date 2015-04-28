using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SC_AtelierManager : MonoBehaviour {

	public GameObject[] a_Ateliers;
	public int i_InstanceAtelier = 3;
	public List<GameObject> a_PoolAtelier;
	public List<GameObject> a_AteliersActifs;
	public float f_OffsetToDeactivate;
	private float f_DepthNextAtelier;
	public int i_NbAteliersActifs;
	private GameObject _Player;
	public List<GameObject> a_DestructiblesManager;
	private bool b_ArrayFilled;
//	private GameObject[] a_DestructiblesTemp;

	// Use this for initialization
	void Start () 
	{
		//L'atelier 0 sera toujours l'atelier de départ
		f_DepthNextAtelier = 0;

		b_ArrayFilled = false;

		//Je remplis la pool d'atelier
		int l = 0;
		int secure = 0;

		while(l < a_Ateliers.Length && secure < 1000)
		{
			for(int i = 0; i < i_InstanceAtelier; i ++)
			{
				if(l == 0)
				{
					i += i_InstanceAtelier-1;
				}

				GameObject clone = Instantiate(a_Ateliers[l], new Vector3(100f, 100f, 0f), Quaternion.identity) as GameObject;
				a_PoolAtelier.Add(clone);
			}

			l++;
			secure ++;
		}

		_Player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!b_ArrayFilled)
		{
			for(int i = 0; i < a_PoolAtelier.Count; i++)
			{
				for(int j = 0; j < a_PoolAtelier[i].GetComponent<SC_Atelier>().a_Destructibles.Count; j++)
				{
					a_DestructiblesManager.Add(a_PoolAtelier[i].GetComponent<SC_Atelier>().a_Destructibles[j]);
				}
			}

			AddAtelier(0);
			b_ArrayFilled = true;
		}

		AteliersHandeler ();
	}

	private void AteliersHandeler ()
	{
		//1. je regarde si l'atelier 1.y + sa longueur + f_OffsetToDeactivate est > que player.y ----> 
		//// je le désactive et je le retire des ateliers actifs et je l'add à la pool d'atelier

		//2. Si le compte d'atelier dans les ateliers actifs est < i_NbAteliersActifs j'add un nouvel atelier
		////je retire un atelier random de la PoolAtelier et je l'ajoute aux ateliers actifs
		/// le random ne doit pas prendre l'atelier 0

		if((a_AteliersActifs[0].transform.position.y - a_AteliersActifs[0].GetComponent<SC_Atelier>().f_Length - f_OffsetToDeactivate) > _Player.transform.position.y)
		{
			RemoveAtelier(0);
		}

		if(a_AteliersActifs.Count < i_NbAteliersActifs)
		{
			AddAtelier(Random.Range(1, a_PoolAtelier.Count));
		}
	}

	private void AddAtelier (int i_IndexPoolAtelier)
	{
		//je le place au bon endroit (les ateliers sont placé par rapport à leur angle superieur gauche)
		//je l'active
		//ajoute sa longueure à la profondeur pour le prochain atelier
		//ajoute un atelier dans le tableau des ateliers actifs
		//je le retire de la pool d'atelier

		a_PoolAtelier[i_IndexPoolAtelier].transform.position = new Vector3(-5f, f_DepthNextAtelier, -0.5f);

		a_PoolAtelier[i_IndexPoolAtelier].GetComponent<SC_Atelier>().Activation();

		f_DepthNextAtelier -= a_PoolAtelier[i_IndexPoolAtelier].GetComponent<SC_Atelier>().f_Length;

		a_AteliersActifs.Add(a_PoolAtelier[i_IndexPoolAtelier]);
		a_PoolAtelier.RemoveAt(i_IndexPoolAtelier);
	}

	private void RemoveAtelier (int i_IndexAtelierActif)
	{
		a_AteliersActifs[i_IndexAtelierActif].GetComponent<SC_Atelier>().Desactivation();

		a_PoolAtelier.Add(a_AteliersActifs[i_IndexAtelierActif]);
		a_AteliersActifs.RemoveAt(i_IndexAtelierActif);
	}
}





















