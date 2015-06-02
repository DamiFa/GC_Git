using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_WINRT == false
using System.Security.Cryptography;
#endif

public class AtelierManager : MonoBehaviour, IPersistent
{

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
#if UNITY_WINRT == false
    private RNGCryptoServiceProvider _rng;
    private byte[] _randomNumbers;
#endif

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

        _atelierCount = _firstAtelier != null ? _ateliers.Length + 1 : _ateliers.Length;
        _pooledAteliers = new List<Atelier>();
        _pooledAteliers.Capacity = _atelierCount;

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

    // Virtual/contract methods

    public void Initialize()
    {
        _pooledAteliers.AddRange(_ateliers);

        if (_firstAtelier != null)
        {
            _firstAtelier.Initialize();
            _endHeight = _offset - _firstAtelier.length;
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
        if (_firstAtelier != null && _firstAtelier.isDeployed)
        {
            _firstAtelier.Clear();
            _firstAtelier.transform.position = new Vector3(0.0f, _offset - _firstAtelier.length * 0.5f, 0.0f);
        }

        for (int i = 0; i < _ateliers.Length; ++i)
        {
            if (_ateliers[i].isDeployed)
            {
                _ateliers[i].Clear();
            }
            _ateliers[i].transform.position = new Vector3(100.0f, 0.0f, 0.0f);
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
    }
    
    private void Pool(Atelier atelier)
    {
        atelier.Clear();
        _pooledAteliers.Add(atelier);
    }

}
