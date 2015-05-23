using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{

    // Constants

    private const float INVERSE_180 = 1.0f / 180.0f;

    // Properties

    public static Character player { get; private set; }
    public bool isDead
    {
        get
        {
            return _isDead;
        }
        private set
        {
            _isDead = value;
            if (value && HasDied != null)
                HasDied();
        }
    }
    public float fallMovement { get { return _fallMovement; } }
    public Vector2 externalForce { get; set; }

    // Events

    public event System.Action HasDied;

    // Inspector variables

    [SerializeField]
    private AnimationCurve _fallingStrength;
    [SerializeField]
    private float _fallingSpeed;
    [SerializeField]
    private AnimationCurve _tiltStrength;
    [SerializeField]
    private float _tiltSpeed;
    [SerializeField]
    private float _horizontalClamp;
    [SerializeField]
    private bool _isPlayer;
#if UNITY_EDITOR
    [SerializeField]
    private bool _isInvinsible;
#endif

    // Private members

    private bool _isDead;
    private float _velocityX;
    private float _velocityY;
    private float _fallMovement;
    private float _tiltMovement;

    private Transform _myTransform;
    private Rigidbody2D _myRigidbody;
    private GameManager _gameManager;

    // Messages

    void Awake()
    {
        isDead = false;
        _fallMovement = 1.0f;

        _myTransform = transform;
        _myRigidbody = GetComponent<Rigidbody2D>();

        if (_isPlayer)
            player = this;
    }

	void Start()
    {
        _gameManager = GameManager.singleton;
	}
	
	void Update()
    {
        if (_isDead)
            return;

        Fall();

        Vector2 additionalForce = externalForce;
        _velocityX = _tiltMovement + additionalForce.x;
        _velocityY = _fallMovement + additionalForce.y;

        // Clamp
        float xPosition = Mathf.Clamp(_myTransform.position.x, -_horizontalClamp, _horizontalClamp);
        _myTransform.position = new Vector3(xPosition, _myTransform.position.y, _myTransform.position.z);
	}

    void FixedUpdate()
    {
        _myRigidbody.velocity = _isDead ? Vector3.zero : new Vector3(_velocityX, _velocityY, 0.0f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == Obstacle.LAYER)
        {
#if UNITY_EDITOR
            if (!_isInvinsible)
#endif
            isDead = true;
        }
    }

    // Private methods

    private void Fall()
    {
        _fallMovement = -_fallingStrength.Evaluate(GameManager.singleton.currentTime * INVERSE_180) * _fallingSpeed;
    }

    // Public methods

    public void Tilt(float input)
    {
        _tiltMovement = _tiltStrength.Evaluate(Mathf.Abs(input)) * Mathf.Sign(input) * _tiltSpeed;
    }

}
