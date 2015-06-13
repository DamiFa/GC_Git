using UnityEngine;
using System.Collections;

public class BackgroundManager : MonoBehaviour, IPersistent
{

    public static BackgroundManager singleton { get; private set; }

    // Inspector variables

    [SerializeField]
    private Transform[] _gradientTextures;
    [SerializeField]
    private float _minGradientInterval = 1.0f;
    [SerializeField]
    private float _maxGradientInterval = 3.0f;
    [SerializeField]
    private Transform[] _starTextures;
    [SerializeField]
    private Transform[] _planetTextures;
    [SerializeField]
    private Transform[] _nebulaeTextures;

    // Private members

    private float _cameraSize;
    private float _gradientSize;
    private float _nextGradientEnd;
    private int _replayCount;

    private Transform _camera;

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

        _cameraSize = Camera.main.orthographicSize;
        _camera = Camera.main.transform;
        _gradientSize = _gradientTextures[0].GetComponent<SpriteRenderer>().sprite.bounds.size.y;
        _replayCount = 0;
    }

    // Virtual/contract methods

    public void Initialize()
    {
        PrepareGradient();
    }

    public void Clear()
    {
        ++_replayCount;
    }

    // Private methods

    private void PrepareGradient()
    {
        int gradientIndex = Random.Range(0, _gradientTextures.Length);

        float gradientYPosition = _camera.position.y - _cameraSize - 1.0f;
        _gradientTextures[gradientIndex].position = new Vector3(0.0f, gradientYPosition, 0.0f);

        _nextGradientEnd = gradientYPosition - _gradientSize;

        StartCoroutine(CheckPlayerPosition(_replayCount));
    }

    private IEnumerator CheckPlayerPosition(int replayCount)
    {
        while (_camera.position.y > _nextGradientEnd && replayCount == _replayCount)
            yield return null;

        if (replayCount == _replayCount)
            StartCoroutine(GradientInterval(replayCount));
    }

    private IEnumerator GradientInterval(int replayCount)
    {
        float seconds = Random.Range(_minGradientInterval, _maxGradientInterval);

        yield return new WaitForSeconds(seconds);

        if (replayCount == _replayCount)
            PrepareGradient();
    }
    
}
