using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

    // Properties

    public static GameManager singleton { get; private set; }

    public float currentTime { get; private set; }
    public int score { get; private set; }
    public int combo { get; private set; }

    // Events

    public event System.Action OnPaused;

    // Inspector variables

    [SerializeField]
    private float _comboTimeWindow = 3.0f;

    // Private members

    private float _startTime;
    private float _comboTimerStart;

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
        _player.HasDied             += this.EndGame;
        _grapnel.Pulled4Distance    += AddScore;
        Obstacle.OnDestroyed        += AddScore;
        Obstacle.OnDestroyed        += IncrementCombo;
        Collectible.OnDestroyed     += AddScore;
    }

    void Start()
    {
        StartGame();
    }

	void Update()
    {
        if (ApplicationManager.isPaused) return;

        currentTime = Time.time - _startTime;

        if (Time.time - _comboTimerStart > _comboTimeWindow)
        {
            combo = 1;
        }
	}

    void OnDestroy()
    {
        Obstacle.OnDestroyed -= IncrementCombo;
        Obstacle.OnDestroyed -= AddScore;
        Collectible.OnDestroyed -= AddScore;
    }

    // Private methods

    private void StartGame()
    {
        _player.Initialize();
        _grapnel.Initialize();
        AtelierManager.singleton.Initialize();
        BackgroundManager.singleton.Initialize();
        _startTime = Time.time;
        ApplicationManager.singleton.Resume();
        combo = 1;
        score = 0;
    }

    private void EndGame()
    {
        ApplicationManager.singleton.Pause();
    }

    private void IncrementCombo(int score)
    {
        ++combo;
        _comboTimerStart = Time.time;
    }

    private void AddScore(int newScore)
    {
        score += newScore * combo;
    }

    // Public methods

    public void Reload()
    {
        AtelierManager.singleton.Clear();
        BackgroundManager.singleton.Clear();
        _player.Clear();
        _grapnel.Clear();
        Camera.main.GetComponent<CameraMotion>().Clear();
        StartGame();
    }

    public void TogglePause()
    {
        if (ApplicationManager.isPaused)
        {
            ApplicationManager.singleton.Resume();
        }
        else
        {
            ApplicationManager.singleton.Pause();
            if (OnPaused != null) OnPaused();
        }
    }

}
