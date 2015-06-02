using UnityEngine;
using System.Collections;

public class ApplicationManager : MonoBehaviour
{

    // Properties

    public static ApplicationManager singleton { get; private set; }

    public bool isPaused { get; private set; }

    void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            enabled = false;
            return;
        }

        isPaused = false;
    }

	void Start()
    {
        
	}
	
	void Update()
    {
	    if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadLevel();
        }
	}

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0.0f;
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1.0f;
    }

    public void ReloadLevel()
    {
        GameManager.singleton.Reload();
    }

}
