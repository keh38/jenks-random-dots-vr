using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MIxedRealityTest : MonoBehaviour
{
    private bool _isRunning = false;
    EyeTracker _eyeTracker = null;
    private VRDataLog _log = new VRDataLog();

    private void Awake()
    {
        _eyeTracker = gameObject.GetComponent<EyeTracker>();
    }

    void Start()
    {
        _eyeTracker.StartTracking();

        _log.Initialize("", true, true);
        _isRunning = true;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Z))
        {
            Finish();
        }
    }

    void LateUpdate()
    {
        if (!_isRunning) return;

        _log.StartGazeEntry(Time.timeSinceLevelLoad, new Vector2(float.NaN, float.NaN));

        _log.AddGaze(_eyeTracker.GazeAngle, _eyeTracker.GazeDir);
        _log.AddEye(_eyeTracker.LeftPosition, _eyeTracker.LeftGazeAngle, _eyeTracker.LeftGaze, _eyeTracker.LeftSize, _eyeTracker.LeftOpenness);
        _log.AddEye(_eyeTracker.RightPosition, _eyeTracker.RightGazeAngle, _eyeTracker.RightGaze, _eyeTracker.RightSize, _eyeTracker.RightOpenness);

        _log.EndEntry();
    }

    void Finish()
    {
        _isRunning = false;
        _eyeTracker.StopTracking();
        string folder = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), "VR Data Logs");
        if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

        var dt = System.DateTime.Now;
        string dataPath = Path.Combine(folder, $"VRGazeCalibration_{dt.ToString("yyyy.MM.dd.H.mm.ss")}.txt");
        _log.WriteToDisk(dataPath);

        UnityEngine.SceneManagement.SceneManager.LoadScene("Visual Scene", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
