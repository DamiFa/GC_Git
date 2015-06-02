﻿using UnityEngine;
using System.Collections;

public class Grapnel : MonoBehaviour, IPersistent
{

    // States

    private enum States { IDLE, FIRED, HOOKED, REWINDING, HIT_NOTHING }

    // Inspector variables

    [SerializeField]
    private AnimationCurve _launchStrength;
    [SerializeField]
    private AnimationCurve _rewindStrength;
    [SerializeField]
    private float _launchSpeed;
    [SerializeField]
    private float _rewindSpeed;
    [SerializeField]
    private float _weight;
    [SerializeField]
    private LayerMask _layerMask;

    // Private members

    private States _state;
    private Vector3 _targetPosition;
    private Vector3 _initialPosition;
    private float _currentAngle;
    private float _realSpeed;
    private float _duration;
    private Vector2 _boxCastSize;
    private Vector2 _previousPosition;

    private Transform _myTransform;
    private Transform _playerTransform;
    private Character _player;
    private Obstacle _currentlyHookedObject;
    private ApplicationManager _application;
    

    // Messages

    void Awake()
    {
        _myTransform = transform;
        _playerTransform = _myTransform.parent;
        _player = GetComponentInParent<Character>();
        _initialPosition = _myTransform.localPosition;
        _currentlyHookedObject = null;

        Collider2D myCollider = GetComponent<Collider2D>();
        _boxCastSize = new Vector2(myCollider.bounds.max.x - myCollider.bounds.min.x, myCollider.bounds.max.y - myCollider.bounds.min.y);
        _previousPosition = _myTransform.position;
    }

    void Start()
    {
        _application = ApplicationManager.singleton;
    }
	
	void Update()
    {
        if (_application.isPaused)
            return;

        switch (_state)
        {
            case States.IDLE:
                break;
            case States.FIRED:
                MoveTowardsTarget();
                break;
            case States.HOOKED:
                Pull();
                break;
            case States.REWINDING:
                Rewind();
                break;
            case States.HIT_NOTHING:
                break;
            default:
                break;
        }
	}

    void FixedUpdate()
    {
        if (_state == States.FIRED)
        {
            Vector2 direction = (Vector2)_myTransform.position - _previousPosition;
            RaycastHit2D[] hit = Physics2D.BoxCastAll(_previousPosition, _boxCastSize, _currentAngle * Mathf.Rad2Deg + 180.0f, direction, direction.magnitude, _layerMask);
            if (hit.Length == 0)
                return;

            int closestObject = 0;
            float closestDistance = Mathf.Infinity;
            for (int i = 0; i < hit.Length; ++i)
            {
                float distance = Vector2.Distance(_previousPosition, hit[i].transform.position);
                if (distance <= closestDistance)
                {
                    closestDistance = distance;
                    closestObject = i;
                }
            }
            Hook(hit[closestObject].collider.gameObject);
        }

        _previousPosition = _myTransform.position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (_state == States.FIRED)
        {
            //Hook(other.gameObject);
        }
    }

    // Virtual/contract methods

    public void Initialize()
    {
        _state = States.IDLE;
        _currentAngle = 0.0f;
    }

    public void Clear()
    {
        _myTransform.SetParent(_playerTransform);
        _myTransform.localPosition = _initialPosition;
        _myTransform.rotation = Quaternion.identity;
        _currentlyHookedObject = null;
    }

    // Private methods

    private void MoveTowardsTarget()
    {
        if (Vector3.Distance(_myTransform.position, _targetPosition) > 0.1f)
		{
            Vector3 startPosition = _myTransform.TransformPoint(_initialPosition);
            _myTransform.position = Vector3.Lerp(startPosition, _targetPosition, _launchStrength.Evaluate(_duration));
            _duration += _realSpeed * Time.deltaTime;
		}
        else
        {
            _state = States.REWINDING;
        }
    }

    private void Rewind()
    {
        if (Vector3.Distance(_myTransform.localPosition, _initialPosition) > 0.1f)
        {
            _myTransform.localPosition = Vector3.Lerp(_targetPosition, _initialPosition, _rewindStrength.Evaluate(_duration));
            _duration += _realSpeed * Time.deltaTime;
        }
        else
        {
            _state = States.IDLE;
            _myTransform.localPosition = _initialPosition;
            _myTransform.rotation = Quaternion.identity;
        }
    }

    private void Pull()
    {
        Vector2 direction = -new Vector2(_playerTransform.position.x - _myTransform.position.x,
            _playerTransform.position.y - _myTransform.position.y);

        _player.externalForce = direction * _weight * 0.05f;
    }

    private void Hook(GameObject obstacle)
    {
        _state = States.HOOKED;
        _currentlyHookedObject = obstacle.GetComponent<Obstacle>();
        _targetPosition = _currentlyHookedObject.transform.position;
        _myTransform.position = _targetPosition;
        _myTransform.SetParent(obstacle.transform);
    }

    // Public methods

    public void Launch(Vector3 targetPosition)
    {
        if (_state == States.IDLE || _state == States.REWINDING)
        {
            _state = States.FIRED;
            _targetPosition = targetPosition;

            float angle = Mathf.Atan2(targetPosition.y - _myTransform.position.y, targetPosition.x - _myTransform.position.x);
            _myTransform.Rotate(Vector3.forward, angle * Mathf.Rad2Deg + 90.0f - _currentAngle, Space.World);
            _currentAngle = angle;

            _duration = 0.0f;
            float distance = Vector3.Distance(_myTransform.position, targetPosition);
            _realSpeed = (_launchSpeed + Mathf.Abs(_player.fallMovement)) / distance;
        }
    }

    public void Release()
    {
        if (_state == States.FIRED || _state == States.HOOKED)
        {
            _state = States.REWINDING;
            _targetPosition = _playerTransform.InverseTransformPoint(_myTransform.position);

            _duration = 0.0f;
            float distance = Vector3.Distance(_myTransform.position, _playerTransform.TransformPoint(_initialPosition));
            _realSpeed = (_rewindSpeed + Mathf.Abs(_player.fallMovement)) / distance;

            _player.externalForce = Vector2.zero;
            _myTransform.SetParent(_playerTransform);

            if (_currentlyHookedObject != null)
            {
                _currentlyHookedObject.Clear();
                _currentlyHookedObject = null;
            }
        }
    }

}