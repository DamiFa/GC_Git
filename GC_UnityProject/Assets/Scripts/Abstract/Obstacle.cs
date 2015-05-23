using UnityEngine;
using System.Collections;

public abstract class Obstacle : MonoBehaviour, IPoolable
{

    // Constants

    public const int LAYER = 9;

    // Inspector variables

    [SerializeField]
    protected int _points;

    public virtual void Initialize()
    {
        gameObject.SetActive(true);
    }

    public virtual void Clear()
    {
        gameObject.SetActive(false);
    }

}
