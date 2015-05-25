using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

    // Properties

    public static GameManager singleton { get; private set; }

    public float currentTime { get; private set; }

    // Private members

    private float _startTime;

    private Character _player;
    private Grapnel _grapnel;

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

        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        _grapnel = _player.GetComponentInChildren<Grapnel>();
        _player.HasDied += this.EndGame;
    }

    void Start()
    {
        StartGame();
    }

	void Update()
    {
        currentTime = Time.time - _startTime;
	}

    // Private methods

    private void StartGame()
    {
        _player.Initialize();
        _grapnel.Initialize();
        AtelierManager.singleton.Initialize();
        _startTime = Time.time;
        ApplicationManager.singleton.Resume();
    }

    private void EndGame()
    {
        ApplicationManager.singleton.Pause();
    }

    // Public methods

    public void Reload()
    {
        AtelierManager.singleton.Clear();
        _player.Clear();
        _grapnel.Clear();
        Camera.main.GetComponent<CameraMotion>().Clear();
        StartGame();
    }

}
