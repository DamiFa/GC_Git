using UnityEngine;
using System.Collections;

public class Chain : MonoBehaviour, IPersistent
{

    // Private members

    private Renderer _chainRenderer;
    private Transform _chainTransform;
    private Transform _grapnelTransform;
    private Grapnel _grapnel;

    private bool _isGrapnelLaunched = false;
    private Vector3 _initialScale;

    // Messages

    void Awake()
    {
        _chainRenderer = GetComponentInChildren<Renderer>();

        _chainTransform = transform;

        _grapnel = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Grapnel>();
        _grapnel.OnStateChanged += ToggleChainUpdate;

        _grapnelTransform = _grapnel.transform;

        _initialScale = _chainTransform.localScale;
    }

    void OnDestroy()
    {
        if (_grapnel != null) _grapnel.OnStateChanged -= ToggleChainUpdate;
    }

    // Virtual/contract methods

    public void Initialize()
    {
        _chainTransform.localScale = _initialScale;
    }

    public void Clear()
    {
        _isGrapnelLaunched = false;
    }

    // Private methods

    private void ToggleChainUpdate(Grapnel.States state)
    {
        if (state == Grapnel.States.FIRED)
        {
            StartCoroutine(UpdateChainScale());
        }
        else if (state == Grapnel.States.IDLE)
        {
            _isGrapnelLaunched = false;
            _chainTransform.localScale = _initialScale;
            _chainTransform.localEulerAngles = new Vector3(90.0f, 180.0f, 0.0f);
        }
    }

    private IEnumerator UpdateChainScale()
    {
        _isGrapnelLaunched = true;

        float distance;

        while (_isGrapnelLaunched)
        {
            _chainTransform.LookAt(new Vector3(_grapnel.targetPosition.x, _grapnel.targetPosition.y, _chainTransform.position.z), Vector3.back);

            distance = Vector3.Distance(_chainTransform.position, _grapnelTransform.position);

            _chainTransform.localScale = new Vector3(_chainTransform.localScale.x,
                _chainTransform.localScale.y, distance /*+ (0.2f * distance)*/);

            _chainRenderer.material.mainTextureScale = new Vector2(1.0f, distance + (0.2f * distance));

            yield return null;
        }
    }

}
