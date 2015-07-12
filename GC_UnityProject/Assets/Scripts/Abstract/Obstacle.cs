using UnityEngine;
using System.Collections;

public abstract class Obstacle : MonoBehaviour, IPersistent
{

    // Constants

    public const int LAYER = 9;

    // Events

    public static event System.Action<int> OnDestroyed;

     // Properties

    public int hookedPoints { get { return _hookedPoints; } }

    // Inspector variables

    [SerializeField]
    protected int _hookedPoints;
    [SerializeField]
    protected int _destructionPoints;
    [SerializeField]
    protected float[] _depthLevels;

    // Protected members

    protected uint _depth = 1;

    // Virtual methods

    public virtual void Initialize()
    {
        float playerHeight = Character.player.transform.position.y * -1.0f;
        int i;
        for (i = 0; i < _depthLevels.Length; ++i)
        {
            if (playerHeight < _depthLevels[i]) break;
        }

        _depth = i != 0 ? (uint)i : 1;

        gameObject.SetActive(true);
    }

    public virtual void Clear()
    {
        gameObject.SetActive(false);

        if (OnDestroyed != null) OnDestroyed(_destructionPoints);
    }

}
