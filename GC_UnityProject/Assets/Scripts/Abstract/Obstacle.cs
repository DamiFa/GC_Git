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

    // Virtual methods

    public virtual void Initialize()
    {
        gameObject.SetActive(true);
    }

    public virtual void Clear()
    {
        gameObject.SetActive(false);

        if (OnDestroyed != null) OnDestroyed(_destructionPoints);
    }

}
