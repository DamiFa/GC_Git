using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InGameUIManager : MonoBehaviour, IPersistent
{

    public static InGameUIManager singleton { get; private set; }

    // Inspector variables

    [SerializeField]
    private Text _score;
    [SerializeField]
    private Text _combo;
    [SerializeField]
    private GameObject _endScreen;
    [SerializeField]
    private Text _endScreenScore;
    [SerializeField]
    private GameObject _pauseMenu;

    // Private members


    // Messages

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

        GameObject.FindGameObjectWithTag("Player").GetComponent<Character>().HasDied += ShowEndScreen;
    }

    void Start()
    {
        ApplicationManager.singleton.OnPaused += ShowPauseScreen;
    }

    void Update()
    {
        _score.text = GameManager.singleton.score.ToString();
        _combo.text = string.Format("x{0}", GameManager.singleton.combo.ToString());
    }

    void OnDestroy()
    {
        ApplicationManager.singleton.OnPaused -= ShowPauseScreen;
    }

    // Virtual/contract methods

    public void Initialize()
    {
        
    }

    public void Clear()
    {
        _endScreen.SetActive(false);
    }

    // Private methods

    private void ShowEndScreen()
    {
        _endScreenScore.text = string.Format("Score\n{0}", GameManager.singleton.score.ToString());
        _endScreen.SetActive(true);
    }

    private void ShowPauseScreen()
    {
        _pauseMenu.SetActive(true);
    }

}
