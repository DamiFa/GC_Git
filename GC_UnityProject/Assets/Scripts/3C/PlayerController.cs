using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    // Inspector variables

    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private Grapnel _grapnel;

    // Private members

    private int _fingerCount;
    private int _prevFingerCount;
    private Vector3 _targetPosition;
    private float _cameraDistance;

    private Character _character;

    void Awake()
    {
        _character = GetComponent<Character>();
        _cameraDistance = Mathf.Abs(_camera.transform.position.z);
    }

	void Update()
    {
        if (_character.isDead)
            return;

        _prevFingerCount = _fingerCount;
        _fingerCount = Input.touchCount;

#if UNITY_EDITOR // Controls in Unity Editor

        float input = Input.acceleration.x != 0.0f ? Input.acceleration.x : Input.GetAxis("Horizontal");
        _character.Tilt(input);

        if (_fingerCount >= 1 && _prevFingerCount == 0)
        {
            Vector2 fingerPosition = Input.GetTouch(0).position;
            _targetPosition = _camera.ScreenToWorldPoint(new Vector3(fingerPosition.x, fingerPosition.y, _cameraDistance));

            _grapnel.Launch(_targetPosition);
        }
        else if (Input.GetMouseButtonDown(0))
        {
            _targetPosition = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _cameraDistance));

            _grapnel.Launch(_targetPosition);
        }

        if (_fingerCount == 0 && _prevFingerCount > 0 || Input.GetMouseButtonUp(0))
        {
            _grapnel.Release();
        }

#else // Controls on builds

        _character.Tilt(Input.acceleration.x);

        if (_fingerCount >= 1 && _prevFingerCount == 0)
        {
            Vector2 fingerPosition = Input.GetTouch(0).position;
            _targetPosition = _camera.ScreenToWorldPoint(new Vector3(fingerPosition.x, fingerPosition.y, _cameraDistance));

            _grapnel.Launch(_targetPosition);
        }

        if (_fingerCount == 0 && _prevFingerCount > 0)
        {
            _grapnel.Release();
        }

#endif

    }

}
