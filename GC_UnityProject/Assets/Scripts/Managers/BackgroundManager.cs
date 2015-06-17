using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackgroundManager : MonoBehaviour, IPersistent
{

    public static BackgroundManager singleton { get; private set; }

    // Enums

    private enum Background { GRADIENT, STARS, NEBULA };

    // Inspector variables

    [SerializeField]
    private Transform[] _gradientTextures;
    [SerializeField]
    private float _gradientSize;
    [SerializeField]
    private float _minGradientInterval = 1.0f;
    [SerializeField]
    private float _maxGradientInterval = 3.0f;
    [SerializeField]
    private Transform[] _starTextures;
    [SerializeField]
    private float _minStarsInterval = 1.0f;
    [SerializeField]
    private float _maxStarsInterval = 3.0f;
    [SerializeField]
    private float _initialStarsOffset = 0.5f;
    [SerializeField]
    private Transform[] _planetTextures;
    [SerializeField]
    private Transform[] _nebulaeTextures;
    [SerializeField]
    private float _minNebualeInterval = 1.0f;
    [SerializeField]
    private float _maxNebulaeInterval = 3.0f;
    [SerializeField]
    private float _initialNebulaeOffset = 0.5f;

    // Private members

    private float _cameraSize;
    private int _replayCount;
    private float _starsSize;
    private float _nebulaSize;
    private List<Transform> _availableStars;
    private List<Transform> _availableNebulae;

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
        _replayCount = 0;
        _starsSize = _starTextures[0].GetComponent<SpriteRenderer>().sprite.bounds.size.y;
        _nebulaSize = _nebulaeTextures[0].GetComponent<SpriteRenderer>().sprite.bounds.size.y;

        _availableStars = new List<Transform>(_starTextures);
        _availableNebulae = new List<Transform>(_nebulaeTextures);
    }

    // Virtual/contract methods

    public void Initialize()
    {
        PrepareGradient();
        PlaceInitialSprites(Background.STARS);
        PlaceInitialSprites(Background.NEBULA);
    }

    public void Clear()
    {
        ++_replayCount;
        _availableStars.Clear();
        _availableStars.AddRange(_starTextures);
        _availableNebulae.Clear();
        _availableNebulae.AddRange(_nebulaeTextures);
    }

    // Private methods

    private void PrepareGradient()
    {
        int gradientIndex = Random.Range(0, _gradientTextures.Length);

        float gradientYPosition = _camera.position.y - _cameraSize - 1.0f;
        _gradientTextures[gradientIndex].position = new Vector3(0.0f, gradientYPosition, 0.0f);

        StartCoroutine(CheckPlayerPosition(_replayCount, gradientYPosition - _gradientSize, Background.GRADIENT));
    }

    private void PlaceInitialSprites(Background background)
    {
        float spriteYPosition = _camera.position.y - _cameraSize - 1.0f;
        float offset = 0.0f;
        List<Transform> sprites = null;
        switch (background)
        {
            case Background.STARS:
                offset = _initialStarsOffset * _starsSize;
                sprites = _availableStars;
                break;
            case Background.NEBULA:
                offset = _initialNebulaeOffset * _nebulaSize;
                sprites = _availableNebulae;
                break;
            default:
                break;
        }

        for (int i = 0; i < 3; ++i)
        {
            int index = Random.Range(0, sprites.Count);
            sprites[index].position = new Vector3(0.0f, spriteYPosition, 0.0f);
            spriteYPosition -= offset;
            StartCoroutine(CheckPlayerPosition(_replayCount, spriteYPosition, background, sprites[index]));
            sprites.RemoveAt(index);
        }
    }

    private void PlaceSprite(Background background)
    {
        List<Transform> sprites = null;
        float size = 0.0f;
        switch (background)
        {
            case Background.STARS:
                sprites = _availableStars;
                size = _starsSize;
                break;
            case Background.NEBULA:
                sprites = _availableNebulae;
                size = _nebulaSize;
                break;
            default:
                break;
        }
        int index = Random.Range(0, sprites.Count);

        float spriteYPosition = _camera.position.y - _cameraSize - 1.0f;
        sprites[index].position = new Vector3(0.0f, spriteYPosition, 0.0f);

        StartCoroutine(CheckPlayerPosition(_replayCount, spriteYPosition - size, background, sprites[index]));

        sprites.RemoveAt(index);
    }

    private IEnumerator CheckPlayerPosition(int replayCount, float endHeight, Background background, Transform sprite = null)
    {
        while (_camera.position.y + _cameraSize > endHeight && replayCount == _replayCount)
            yield return null;

        if (replayCount == _replayCount)
        {
            switch (background)
            {
                case Background.GRADIENT:
                    StartCoroutine(GradientInterval(replayCount));
                    break;
                case Background.STARS:
                    _availableStars.Add(sprite);
                    StartCoroutine(SpriteInterval(replayCount, background, _minStarsInterval, _maxStarsInterval));
                    break;
                case Background.NEBULA:
                    _availableNebulae.Add(sprite);
                    StartCoroutine(SpriteInterval(replayCount, background, _minNebualeInterval, _maxNebulaeInterval));
                    break;
                default:
                    break;
            }
        }
    }

    private IEnumerator GradientInterval(int replayCount)
    {
        float seconds = Random.Range(_minGradientInterval, _maxGradientInterval);

        yield return new WaitForSeconds(seconds);

        if (replayCount == _replayCount)
            PrepareGradient();
    }

    private IEnumerator SpriteInterval(int replayCount, Background background, float minInterval, float maxInterval)
    {
        float seconds = Random.Range(minInterval, maxInterval);

        yield return new WaitForSeconds(seconds);

        if (replayCount == _replayCount)
            PlaceSprite(background);
    }
    
}
