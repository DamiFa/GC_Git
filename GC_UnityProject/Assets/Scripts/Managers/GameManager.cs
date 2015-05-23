using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    // Properties

    public static GameManager singleton { get; private set; }

    public float currentTime { get; private set; }

    // Private members

    private float _startTime;

    void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else
        {
            enabled = false;
            return;
        }
    }

    void Start()
    {
        StartGame();
    }

	void Update()
    {
        currentTime = Time.time - _startTime;
	}

    void StartGame()
    {
        AtelierManager.singleton.Initialize();
        _startTime = Time.time;
    }
}
