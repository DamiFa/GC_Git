using UnityEngine;
using System.Collections;

public class Asteroid : Obstacle
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void Initialize()
    {
        base.Initialize();

        Debug.Log("Astero depth: " + _depth);
    }

    public override void Clear()
    {
        base.Clear();
    }

}
