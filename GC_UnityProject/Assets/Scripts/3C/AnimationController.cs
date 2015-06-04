using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour, IPersistent
{

    // Inspector variables

    [SerializeField]
    private Animator _body;
    [SerializeField]
    private Animator _grapnel;
    [SerializeField]
    private Transform _rocketRight;
    [SerializeField]
    private Transform _rocketLeft;
    [SerializeField]
    private float _rocketRotationSpeed = 1.0f;
    [SerializeField]
    private float _rocketMaxAngle = 80.0f;

    // Private members

    private Character _character;

    void Awake()
    {
        _character = GetComponentInParent<Character>();
    }

	void Start()
    {
        Initialize();

        Grapnel grapnel = _grapnel.GetComponent<Grapnel>();
        grapnel.OnHooked            += CharacterHookAnimation;
        grapnel.OnDetached          += CharacterIdleAnimation;
        grapnel.OnLaunched          += GrapnelLaunchAnimation;
        grapnel.OnRewinding         += GrapnelRewindAnimation;
        grapnel.OnHooked            += GrapnelHookAnimation;
        grapnel.OnFinishedRewinding += GrapnelIdleAnimation;
	}
	
	void LateUpdate()
    {
        float tilt = _character.tilt * Time.deltaTime * _rocketRotationSpeed;
        float leftRocketAngle = Mathf.Lerp(0.0f, _rocketMaxAngle, tilt);
        float rightRocketAngle = Mathf.Lerp(0.0f, -1.0f * _rocketMaxAngle, -1.0f * tilt);

        _rocketLeft.eulerAngles = new Vector3(0.0f, 0.0f, leftRocketAngle);
        _rocketRight.eulerAngles = new Vector3(0.0f, 0.0f, rightRocketAngle);
	}

    // Virtual/contract methods

    public void Initialize()
    {
        _body.SetBool("IsHooked", false);
        _grapnel.SetBool("IsRewinding", false);
        _grapnel.SetBool("IsLaunched", false);
        _grapnel.SetBool("IsHooked", false);
    }

    public void Clear()
    {
        
    }

    // Private methods

    private void CharacterHookAnimation()
    {
        _body.SetBool("IsHooked", true);
    }

    private void CharacterIdleAnimation()
    {
        _body.SetBool("IsHooked", false);
    }

    private void GrapnelLaunchAnimation()
    {
        _grapnel.SetBool("IsLaunched", true);
    }

    private void GrapnelHookAnimation()
    {
        _grapnel.SetBool("IsHooked", true);
        _grapnel.SetBool("IsLaunched", false);
    }

    private void GrapnelRewindAnimation()
    {
        _grapnel.SetBool("IsRewinding", true);
        _grapnel.SetBool("IsLaunched", false);
        _grapnel.SetBool("IsHooked", false);
    }

    private void GrapnelIdleAnimation()
    {
        _grapnel.SetBool("IsRewinding", false);
    }

}
