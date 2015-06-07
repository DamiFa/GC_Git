using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class Atelier : MonoBehaviour, IPersistent
{

    // Properties

    public float length { get; private set; }
    public bool isDeployed { get; private set; }
    public Obstacle[] ingredients
    {
        get { return _ingredients; }
        set
        {
            if (Application.isPlaying)
                Debug.LogWarning("You tried to change the ingredients list during play.", this);
            else
                _ingredients = value;
        }
    }
    
    // Events

    public static event System.Action<Atelier> OnPlayerEntered;
    public static event System.Action<Atelier> ReadyToBePooled;

    // Inspector variables

    [SerializeField]
    private Obstacle[] _ingredients;

    // Private members

    private int _countDownB4Pooling;

    // Messages

    void Awake()
    {
        Bounds bounds = GetComponent<Collider2D>().bounds;
        length = bounds.max.y - bounds.min.y;
        isDeployed = false;
    }

    void OnTriggerEnter2D()
    {
        if (OnPlayerEntered != null)
        {
            OnPlayerEntered(this);
        }
    }

    void OnDestroy()
    {
        if (isDeployed)
        {
            Clear();
        }
    }

    // Virtual/contract methods

    public void Initialize()
    {
        gameObject.SetActive(true);
        for (int i = 0; i < _ingredients.Length; ++i)
        {
            _ingredients[i].Initialize();
        }

        _countDownB4Pooling = 2;
        OnPlayerEntered += this.CheckIfPoolable;
        isDeployed = true;
    }

    public void Clear()
    {
        OnPlayerEntered -= this.CheckIfPoolable;
        gameObject.SetActive(false);
        isDeployed = false;
    }

    // Private methods

    private void CheckIfPoolable(Atelier atelier)
    {
        if (atelier != this)
        {
            --_countDownB4Pooling;
            if (_countDownB4Pooling == 0 && ReadyToBePooled != null)
                ReadyToBePooled(this);
        }
    }

}
