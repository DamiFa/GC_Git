using UnityEngine;
using System.Collections;

public class Grapnel : MonoBehaviour, IPersistent
{

    // States

    public enum States { IDLE, FIRED, HOOKED, REWINDING, HIT_NOTHING }

    // Events

    public event System.Action<States> OnStateChanged;
    public event System.Action<int> Pulled4Distance;

    // Properties

    public States state
    {
        get { return _state; }
        private set
        {
            _state = value;
            if (OnStateChanged != null) OnStateChanged(value);
        }
    }
    public Vector3 targetPosition
    {
        get { return _targetPosition; }
    }

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
    [SerializeField]
    private float _pointCollectingDistance = 1.0f;

    // Private members

    private States _state;
    private Vector3 _targetPosition;
    private Vector3 _initialPosition;
    private float _currentAngle;
    private float _realSpeed;
    private float _duration;
    private Vector2 _boxCastSize;
    private Vector2 _previousPosition;
    private Vector2 _previousPlayerPosition;
    private float _pointCollectingSqDistance;
    private float _pulledSqDistance;

    private Transform _myTransform;
    private Transform _playerTransform;
    private Character _player;
    private Obstacle _currentlyHookedObject;

    // Messages

    void Awake()
    {
        _myTransform = transform;
        _playerTransform = _myTransform.parent;
        _player = GetComponentInParent<Character>();
        _initialPosition = _myTransform.localPosition;
        _currentlyHookedObject = null;

        Collider2D myCollider = GetComponentInChildren<Collider2D>();
        _boxCastSize = new Vector2(myCollider.bounds.max.x - myCollider.bounds.min.x, myCollider.bounds.max.y - myCollider.bounds.min.y);

        _pointCollectingSqDistance = _pointCollectingDistance * _pointCollectingDistance;
    }
	
	void Update()
    {
        if (ApplicationManager.isPaused)
            return;

        switch (state)
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
        switch (state)
        {
            //case States.IDLE:
            //    break;
            case States.FIRED:
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
                break;
            case States.HOOKED:
                {
                    float dx = _previousPlayerPosition.x - _playerTransform.position.x;
                    float dy = _previousPlayerPosition.y - _playerTransform.position.y;
                    _pulledSqDistance += dx * dx + dy * dy;
                    if (_pulledSqDistance >= _pointCollectingSqDistance)
                    {
                        if (Pulled4Distance != null) Pulled4Distance(_currentlyHookedObject.hookedPoints);
                        _pulledSqDistance = 0.0f;
                    }

                    _previousPlayerPosition = _playerTransform.position;
                }
                break;
            //case States.REWINDING:
            //    break;
            //case States.HIT_NOTHING:
            //    break;
            default:
                break;
        }

        _previousPosition = _myTransform.position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (state == States.FIRED)
        {
            //Hook(other.gameObject);
        }
    }

    // Virtual/contract methods

    public void Initialize()
    {
        state = States.IDLE;
        _currentAngle = 0.0f;
        _previousPosition = _myTransform.position;
        _pulledSqDistance = 0.0f;
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
            Vector3 startPosition = _playerTransform.position + _initialPosition;
            _myTransform.position = Vector3.Lerp(startPosition, _targetPosition, _launchStrength.Evaluate(_duration));
            _duration += _realSpeed * Time.deltaTime;
		}
        else
        {
            state = States.REWINDING;
        }
    }

    private void Rewind()
    {
        Vector3 worldInitialPos = _playerTransform.position + _initialPosition;
        if (Vector3.Distance(_myTransform.position, worldInitialPos) > 0.1f)
        {
            _myTransform.position = Vector3.Lerp(_targetPosition, worldInitialPos, _rewindStrength.Evaluate(_duration));
            _duration += _realSpeed * Time.deltaTime;
        }
        else
        {
            state = States.IDLE;
            _myTransform.SetParent(_playerTransform);
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
        _currentlyHookedObject = obstacle.GetComponent<Obstacle>();
        _targetPosition = _currentlyHookedObject.transform.position;
        _myTransform.position = _targetPosition;
        _myTransform.SetParent(obstacle.transform);
        _previousPlayerPosition = _playerTransform.position;
        _pulledSqDistance = 0.0f;

        state = States.HOOKED;
    }

    // Public methods

    public void Launch(Vector3 targetPosition)
    {
        if (state == States.IDLE || state == States.REWINDING)
        {
            _targetPosition = targetPosition;

            _myTransform.LookAt(new Vector3(targetPosition.x, targetPosition.y, _myTransform.position.z), Vector3.back);

            _duration = 0.0f;
            float distance = Vector3.Distance(_myTransform.position, targetPosition);
            _realSpeed = (_launchSpeed + Mathf.Abs(_player.fallMovement)) / distance;

            _myTransform.SetParent(null);

            state = States.FIRED;
        }
    }

    public void Release()
    {
        if (state == States.FIRED || state == States.HOOKED)
        {
            _duration = 0.0f;
            float distance = Vector3.Distance(_myTransform.position, _playerTransform.position + _initialPosition);
            _realSpeed = (_rewindSpeed + Mathf.Abs(_player.fallMovement)) / distance;

            _player.externalForce = Vector2.zero;
            _myTransform.SetParent(null);

            if (_currentlyHookedObject != null)
            {
                _currentlyHookedObject.Clear();
                _currentlyHookedObject = null;
            }

            state = States.REWINDING;
        }
    }

}
