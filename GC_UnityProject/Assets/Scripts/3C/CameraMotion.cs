using UnityEngine;
using System.Collections;

public class CameraMotion : MonoBehaviour, IPersistent
{

    // Inspector variables

    [SerializeField]
    private Transform _player;

    // Private members

    private float _offsetY;

    private Transform _myTransform;

    void Awake()
    {
        _myTransform = transform;
        _offsetY = _myTransform.position.y - _player.position.y;
    }
	
	void Update()
    {
        _myTransform.position = new Vector3(_myTransform.position.x, _player.position.y + _offsetY, _myTransform.position.z);
	}

    // Virtual/contract methods

    public void Initialize()
    {
        
    }

    public void Clear()
    {
        _myTransform.position = new Vector3(_myTransform.position.x, _player.position.y + _offsetY, _myTransform.position.z);
    }
}
