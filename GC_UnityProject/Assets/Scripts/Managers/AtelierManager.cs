using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_WINRT == false
using System.Security.Cryptography;
#endif

public class AtelierManager : MonoBehaviour, IPoolable {

    public static AtelierManager singleton { get; private set; }

    // Inspector variables

    [SerializeField]
    private Atelier[] _ateliers;
    [SerializeField]
    private Atelier _firstAtelier = null;
    [SerializeField]
    private float _offset;

    // Private members

    private int _atelierCount;
    private float _endHeight;

    private List<Atelier> _pooledAteliers;
    private Queue<Atelier> _deployedAteliers;
#if UNITY_WINRT == false
    private RNGCryptoServiceProvider _rng;
    private byte[] _randomNumbers;
#endif

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

        _atelierCount = _firstAtelier != null ? _ateliers.Length + 1 : _ateliers.Length;
        _pooledAteliers = new List<Atelier>();
        _pooledAteliers.Capacity = _atelierCount;
        _deployedAteliers = new Queue<Atelier>();

#if UNITY_WINRT == false
        _rng = new RNGCryptoServiceProvider();
        _randomNumbers = new byte[1];
#endif

        Atelier.ReadyToBePooled += this.Pool;
        Atelier.OnPlayerEntered += this.Append;
    }

    void Start()
    {
        if (_firstAtelier != null)
        {
            _offset = _firstAtelier.transform.position.y + (_firstAtelier.length * 0.5f);
        }
    }

    void Update()
    {

    }

    public void Initialize()
    {
        _pooledAteliers.AddRange(_ateliers);

        if (_firstAtelier != null)
        {
            _firstAtelier.Initialize();
            _endHeight = _offset - _firstAtelier.length;
            _deployedAteliers.Enqueue(_firstAtelier);
            Append();
        }
        else
        {
            _endHeight = _offset;
            Append();
            Append();
        }
    }

    public void Clear()
    {
        _pooledAteliers.Clear();
        for (int i = 0; i < _deployedAteliers.Count; ++i)
        {
            Atelier atelier = _deployedAteliers.Dequeue();
            atelier.Clear();
        }
    }

    // Private methods

    private void Append(Atelier atelier = null)
    {
#if UNITY_WINRT == false
        _rng.GetBytes(_randomNumbers);
        int trueNumber = _randomNumbers[0] % _pooledAteliers.Count;
#else
        int trueNumber = Random.Range(0, _pooledAteliers.Count);
#endif

        Atelier newAtelier = _pooledAteliers[trueNumber];
        newAtelier.transform.position = new Vector3(0.0f, _endHeight - (newAtelier.length * 0.5f), 0.0f);
        newAtelier.Initialize();
        _endHeight -= newAtelier.length;
        _pooledAteliers.RemoveAt(trueNumber);
        _deployedAteliers.Enqueue(newAtelier);
    }
    
    private void Pool(Atelier atelier)
    {
        atelier.Clear();
        _pooledAteliers.Add(atelier);
        _deployedAteliers.Dequeue();
    }

}
