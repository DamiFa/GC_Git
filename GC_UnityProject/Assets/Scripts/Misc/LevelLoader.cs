using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour
{

    // Events

    public event System.Action<AsyncOperation> OnStartedLoadingLevel;

	// Inspector variables

    [SerializeField]
    private string _levelName;

    // Public methods

    public void LoadLevel()
    {
        var asOp = Application.LoadLevelAsync(_levelName);
        if (OnStartedLoadingLevel != null) OnStartedLoadingLevel(asOp);
    }

}
