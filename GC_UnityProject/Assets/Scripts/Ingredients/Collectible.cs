using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour, IPersistent
{
    // Constants

    public const int LAYER = 12;

    // Events

    public static event System.Action<int> OnDestroyed;

    // Inspector variables

    [SerializeField]
    protected int _points;

    public void Initialize()
    {
        gameObject.SetActive(true);
    }

    public void Clear()
    {
        gameObject.SetActive(false);

        if (OnDestroyed != null) OnDestroyed(_points);
    }

}
