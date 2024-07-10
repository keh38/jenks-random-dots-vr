using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using JenksVR.VRD;

public class BlobController : MonoBehaviour
{
    ArenaProperties _arenaProperties = null;
    BlobProperties _blobProperties = null;
    PlayerController _player;

    private BlobMovement _blobPrefab;
    private List<BlobMovement> _blobs = new List<BlobMovement>();

    private Camera _headCamera;
    private float _orthoSize = 1;
    private float _fov = 120;
    private float _pitch = 0;

    private bool _isRunning = false;
    private bool _isTesting = false;

    private bool _dpadRefractory = false;

    private int _numDots;
    private int _numCoherent;
    private int _numBrownian;
    private int _numWhiteNoise;

    float _horzRandVel;
    float _vertRandVel;

    private KLib.GaussianRandom _grn;

    private void Awake()
    {
        _blobPrefab = GameObject.Find("Blob Controller/Blob").GetComponent<BlobMovement>();
        _blobPrefab.gameObject.SetActive(false);
        _player = GameObject.Find("Player").GetComponent<PlayerController>();

        _headCamera = GameObject.Find("Player").GetComponentInChildren<Camera>();
    }

    private void Start()
    {
        _grn = new KLib.GaussianRandom();
    }

    private void OnDestroy()
    {
        _blobs.Clear();
    }

    public void SetProperties(ArenaProperties arenaProperties, BlobProperties blobProperties)
    {
        _arenaProperties = arenaProperties;
        _blobProperties = blobProperties;
    }

    public float HorizontalVelocity { get { return _horzRandVel; } }
    public float VerticalVelocity { get { return _vertRandVel; } }

    public void InitializeBlobs(float frameRate, float hfov)
    {
        _fov = hfov;
        var aspectRatio = (float)Screen.width / Screen.height;
        float verticalFOV = Mathf.Atan(Mathf.Tan((_fov * Mathf.Deg2Rad) / 2f) / aspectRatio);
        float height = 2 * _arenaProperties.radius * Mathf.Tan(verticalFOV);

        float surfaceArea = height * Mathf.PI * 2 * _arenaProperties.radius * _fov / 360f;
        _numDots = Mathf.RoundToInt(surfaceArea * _blobProperties.density);
        Debug.Log("hfov = " +  hfov);
        Debug.Log("ndots = " + _numDots);

        if (_blobProperties.movementMode == BlobProperties.MovementMode.Brownian)
        {
            _numWhiteNoise = 0;
            _numCoherent = Mathf.RoundToInt(_numDots * _blobProperties.coherence);
            _numBrownian = _numDots - _numCoherent;
        }
        else if(_blobProperties.movementMode == BlobProperties.MovementMode.WhiteNoise)
        {
            _numCoherent = 0;
            _numBrownian = 0;
            _numWhiteNoise = _numDots;
            RandomizeVelocity();
        }

        int lifetime = -1;
        if (_blobProperties.lifeTime_ms > 0)
        {
            lifetime = Mathf.RoundToInt(frameRate * _blobProperties.lifeTime_ms / 1000f);
        }
        int deadtime = 0;
        if (_blobProperties.deadTime_ms > 0)
        {
            deadtime = Mathf.RoundToInt(frameRate * _blobProperties.deadTime_ms / 1000f);
        }

        _blobPrefab.gameObject.SetActive(true);

        if (_blobs.Count > _numDots)
        {
            for (int k= _numDots; k<_blobs.Count- _numDots; k++)
            {
                Destroy(_blobs[k].gameObject);
            }
            _blobs.RemoveRange(_numDots, _blobs.Count - _numDots);
        }

        for (int k=_blobs.Count; k< _numDots; k++)
        {
            var obj = GameObject.Instantiate(_blobPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
            obj.name = "Blob" + _blobs.Count.ToString();

            var b = obj.GetComponent<BlobMovement>();
            _blobs.Add(b);
        }

        foreach (var b in _blobs)
        {
            b.Initialize(_blobProperties, height, _arenaProperties.radius, hfov,
                lifetime, deadtime, _player, _grn, this);
        }

        _blobPrefab.gameObject.SetActive(false);

        RandomizeCategories();

        _isRunning = true;
    }

    private void Update()
    {
        RandomizeCategories();
        if (_isRunning)
        {
            RandomizeVelocity();
        }
    }

    private void RandomizeCategories()
    {
        int[] order = KLib.KMath.Permute(_numDots);

        for (int k = 0; k < _numCoherent; k++) _blobs[order[k]].SetCategory(BlobProperties.MovementCategory.Coherent);
        for (int k = 0; k < _numBrownian; k++) _blobs[order[k + _numCoherent]].SetCategory(BlobProperties.MovementCategory.Brownian);
        for (int k = 0; k < _numWhiteNoise; k++) _blobs[order[k + _numCoherent + _numBrownian]].SetCategory(BlobProperties.MovementCategory.WhiteNoise);
    }

    private void RandomizeVelocity()
    {
        _horzRandVel = _grn.Next(0, _blobProperties.horizontalStdDev);
        _vertRandVel = _grn.Next(0, _blobProperties.verticalStdDev);
    }

    public void Halt()
    {
        _isRunning = false;
        foreach (var b in _blobs) b.Halt();
    }

}
