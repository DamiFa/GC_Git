using UnityEngine;
using System.Collections;

public class ApplicationManager : MonoBehaviour {

    public static ApplicationManager singleton { get; private set; }

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
    }

	void Start()
    {
        
	}
	
	void Update()
    {
	
	}

    void OnGUI()
    {
        if (GUILayout.Button("Reload"))
        {
            Application.LoadLevel(0); // TEMP
        }
    }

}
