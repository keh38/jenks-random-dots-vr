using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JenksVR.VRD;

public class VRDataLogger : MonoBehaviour
{
    private PlayerController _player;
    private EyeTracker _tracker;
    private VRDataLog _log = new VRDataLog();

    private bool _isTracking = false;
    private bool _isRunning = false;

    public void StartLogging(Configuration config, bool isVR, PlayerController player, EyeTracker tracker)
    {
        _player = player;
        _tracker = tracker;

        _isTracking = isVR && config.controller.eyeTracking;
        _isRunning = true;

        _log.Initialize(config.ToString(), _isTracking, false);
    }

    public void StopLogging()
    {
        _isRunning = false;
        _log.WriteToDisk();
    }

    void LateUpdate()
    {
        if (!_isRunning) return;

        _log.StartEntry(Time.timeSinceLevelLoad, _player.YawVelocity);

        if (_isTracking)
        {
            _log.AddGaze(_tracker.GazeAngle, _tracker.GazeDir);
            _log.AddEye(_tracker.LeftPosition, _tracker.LeftGazeAngle, _tracker.LeftGaze, _tracker.LeftSize, _tracker.LeftOpenness);
            _log.AddEye(_tracker.RightPosition, _tracker.RightGazeAngle, _tracker.RightGaze, _tracker.RightSize, _tracker.RightOpenness);
        }

        _log.EndEntry();
    }
}
