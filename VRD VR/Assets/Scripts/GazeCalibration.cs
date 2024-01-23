using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GazeCalibration : MonoBehaviour
{
    private Text _prompt;
    private GameObject _target;

    private List<Vector2> _targetLocations;
    private float _dwellTime_s = 2;
    private float _scale = 0.75f;

    EyeTracker _eyeTracker = null;

    private VRDataLog _log = new VRDataLog();
    private bool _isRunning = false;

    private Vector2 _currentTarget = new Vector2(float.NaN, float.NaN);

    private void Awake()
    {
        _prompt = GameObject.Find("Canvas/Text").GetComponent<Text>();
        _target = GameObject.Find("Target");

        _eyeTracker = gameObject.GetComponent<EyeTracker>();
    }

    void Start()
    {
        //var camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        //var halfVerticalFOV = 0.5f * camera.fieldOfView * Mathf.Deg2Rad;
        //Debug.Log("FOV = " + camera.fieldOfView);
        //Debug.Log("H = " + camera.pixelHeight);
        //Debug.Log("Focal length = " + (0.5f * (float)camera.pixelHeight) / Mathf.Tan(halfVerticalFOV));

        _target.SetActive(false);
        SetTargetLocations();

        _eyeTracker.StartTracking();

        _log.Initialize("", true, true);
        _isRunning = true;

        StartCoroutine(ShowTargets());
    }

    void SetTargetLocations()
    {
        var width = Mathf.Tan(15 * Mathf.Deg2Rad);
        width = 30;
        var height = width;

        _targetLocations = new List<Vector2>();
        _targetLocations.Add(new Vector2(0, 0));
        for (float k = width; k >= -width; k -= 10)
            _targetLocations.Add(new Vector2(k, 0));
        _targetLocations.Add(new Vector2(0, 0));
        for (float k = height; k >= -height; k -= 10)
            _targetLocations.Add(new Vector2(0, k));
        _targetLocations.Add(new Vector2(0, 0));
    }


    IEnumerator ShowTargets()
    {
        yield return new WaitForSeconds(_dwellTime_s);
        _prompt.text = "";
        _target.SetActive(true);

        foreach (var v in _targetLocations)
        {
            _currentTarget = v;
            _target.transform.localEulerAngles = new Vector3(v.y, v.x, 0);
            yield return new WaitForSeconds(_dwellTime_s);
        }

        _target.SetActive(false);
        _prompt.text = "Finished.";
        yield return new WaitForSeconds(_dwellTime_s);

        Finish();
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

    void LateUpdate()
    {
        if (!_isRunning) return;

        _log.StartGazeEntry(Time.timeSinceLevelLoad, _currentTarget);

        _log.AddGaze(_eyeTracker.GazeAngle, _eyeTracker.GazeDir);
        _log.AddEye(_eyeTracker.LeftPosition, _eyeTracker.LeftGazeAngle, _eyeTracker.LeftGaze, _eyeTracker.LeftSize, _eyeTracker.LeftOpenness);
        _log.AddEye(_eyeTracker.RightPosition, _eyeTracker.RightGazeAngle, _eyeTracker.RightGaze, _eyeTracker.RightSize, _eyeTracker.RightOpenness);

        _log.EndEntry();
    }
}
