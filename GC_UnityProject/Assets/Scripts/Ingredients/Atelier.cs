using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class Atelier : MonoBehaviour, IPoolable
{

    // Properties

    public float length { get; private set; }
    
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
    }

	void Start()
    {
	    
	}
	
	void Update()
    {
	    
	}

    void OnTriggerEnter2D()
    {
        if (OnPlayerEntered != null)
            OnPlayerEntered(this);
    }

    public void Initialize()
    {
        gameObject.SetActive(true);
        for (int i = 0; i < _ingredients.Length; ++i)
        {
            _ingredients[i].Initialize();
        }

        _countDownB4Pooling = 2;
        OnPlayerEntered += this.CheckIfPoolable;
    }

    public void Clear()
    {
        OnPlayerEntered -= this.CheckIfPoolable;
        gameObject.SetActive(false);
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
