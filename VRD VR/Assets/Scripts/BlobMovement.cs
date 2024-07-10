using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JenksVR.VRD;

public class BlobMovement : MonoBehaviour
{
    public Transform sphere;
    public Material highlight;

    float _arenaHeight = 3;
    float _arenaRadius;

    int _lifeTime_frames = -1;
    int _deadTime_frames = 0;
    bool _isRunning = false;
    float _hfov;
    float _minAngle;
    float _maxAngle;

    bool _addChairVelocity;
    bool _identicalNoise;

    float _horzStdDev;
    float _vertStdDev;

    float _horzRandVel;
    float _vertRandVel;

    int _numFramesThisLife = 0;
    int _numDeadtimeFrames = 0;

    BlobProperties.MovementCategory _category = BlobProperties.MovementCategory.Coherent;

    BlobController _blobController;
    PlayerController _player;
    KLib.GaussianRandom _grn;

    public void Initialize(BlobProperties blobProperties, float arenaHeight, float arenaRadius, float hfov, int lifetime, int deadtime, PlayerController player, KLib.GaussianRandom grn, BlobController blobController)
    {
        _arenaHeight = arenaHeight;
        _arenaRadius = arenaRadius;
        _hfov = hfov;
        _lifeTime_frames = lifetime;
        _deadTime_frames = deadtime;
        _player = player;
        _blobController = blobController;
        _grn = grn;

        _addChairVelocity = blobProperties.addChairVelocity;
        _identicalNoise = blobProperties.identicalNoise;
        _horzStdDev = blobProperties.horizontalStdDev;
        _vertStdDev = blobProperties.verticalStdDev;

        _minAngle = -0.5f * _hfov + 270f;
        _maxAngle = 0.5f * _hfov + 270f;

        sphere.localScale = blobProperties.diameter_cm/ 100f * Vector3.one;
        sphere.localPosition = new Vector3(arenaRadius, 0, 0);

        RandomizePosition();
        RandomizeVelocity();

        _numFramesThisLife = 0;
        _numDeadtimeFrames = 0;
        if (!blobProperties.strobed)
        {
            _numFramesThisLife = Random.Range(0, _lifeTime_frames + _deadTime_frames - 1);
        }
        
        //_isRunning = true;
    }

    public void SetCategory(BlobProperties.MovementCategory category)
    {
        _category = category;

        if (!_isRunning)
        {
            if (_category == BlobProperties.MovementCategory.WhiteNoise)
            {
                RandomizeVelocity();
            }
            _isRunning = true;
        }
    }

    public void Halt()
    {
        _isRunning = false;
    }

    private void RandomizePosition()
    {
        float azimuth = Random.Range(_minAngle, _maxAngle);
        float y = _arenaHeight * Random.Range(-0.5f, 0.5f);
        transform.localPosition = new Vector3(0, y, 0);
        transform.localEulerAngles = new Vector3(0, azimuth, 0);
    }

    private void RandomizeVelocity()
    {
        if (_identicalNoise)
        {
            _horzRandVel = _blobController.HorizontalVelocity;
            _vertRandVel = _blobController.VerticalVelocity;
        }
        else
        {
            _horzRandVel = _grn.Next(0, _horzStdDev);
            _vertRandVel = _grn.Next(0, _vertStdDev);
        }
    }

    private void LateUpdate()
    {
        if (!_isRunning) return;

        float angle = transform.localEulerAngles.y;
        float y = transform.localPosition.y;

        if (_category == BlobProperties.MovementCategory.Coherent)
        {
            angle -= _player.DeltaYaw;
        }
        else if (_category == BlobProperties.MovementCategory.Brownian)
        {
            float rn = Random.Range(0, 2 * Mathf.PI);
            angle += Mathf.Cos(rn) * _player.DeltaYaw;

            float dy_angle = Mathf.Sin(rn) * _player.DeltaYaw * Mathf.Deg2Rad;
            float y_angle = Mathf.Atan2(y, _arenaRadius) + dy_angle;
            y = _arenaRadius * Mathf.Tan(y_angle);
        }
        else if (_category == BlobProperties.MovementCategory.WhiteNoise)
        {
            if (_addChairVelocity) angle -= _player.DeltaYaw;
            //angle += _grn.Next(0, _horzStdDev) * Time.deltaTime;
            angle += _horzRandVel * Time.deltaTime;

            //float dy_angle = _grn.Next(0, _vertStdDev) * Mathf.Deg2Rad * Time.deltaTime;
            float dy_angle = _vertRandVel * Mathf.Deg2Rad * Time.deltaTime;
            float y_angle = Mathf.Atan2(y, _arenaRadius) + dy_angle;
            y = _arenaRadius * Mathf.Tan(y_angle);
        }

        if (angle > _maxAngle)
        {
            angle -= _hfov;
        }
        else if (angle < _minAngle)
        {
            angle += _hfov;
        }

        if (y > _arenaHeight/2)
        {
            //y = -_arenaHeight / 2;
            y -= _arenaHeight;
        }
        else if (y < -_arenaHeight / 2)
        {
            //y = _arenaHeight / 2;
            y += _arenaHeight;
        }

        transform.localEulerAngles = new Vector3(0, angle, 0);
        transform.localPosition = new Vector3(0, y, 0);

        _numFramesThisLife++;
        if (_lifeTime_frames > 0)
        {
            if (_numFramesThisLife == _lifeTime_frames && _deadTime_frames > 0)
            {
                transform.localPosition = new Vector3(0, 1000, 0);
            }
            if (_numFramesThisLife == _lifeTime_frames + _deadTime_frames)
            {
                RandomizePosition();
                if (_category == BlobProperties.MovementCategory.WhiteNoise)
                {
                    RandomizeVelocity();
                }
                _numFramesThisLife = 0;
            }
        }
    }

}
