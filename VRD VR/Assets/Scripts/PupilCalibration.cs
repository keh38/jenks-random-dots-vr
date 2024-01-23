using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class PupilCalibration : MonoBehaviour
{
    internal class DataEntry
    {
        public float time_s;
        public float intensity;
        public float leftSize;
        public float rightSize;

        public DataEntry(float time_s, float intensity, float leftSize, float rightSize)
        {
            this.time_s = time_s;
            this.intensity = intensity;
            this.leftSize = leftSize;
            this.rightSize = rightSize;
        }
    }

    EyeTracker _eyeTracker = null;

    bool _running = false;
    bool _abort = false;

    float _intensity = 0;
    float _period_s = 20;
    int _numReps = 5;
    float _onsetBaseline_s = 2.5f;
    float _offsetBaseline_s = 2.5f;
    float _t0 = 0;
    float _tmax;

    Camera _cam;
    List<DataEntry> _log;

    private void Awake()
    {
        _eyeTracker = gameObject.GetComponent<EyeTracker>();
        _cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    void Start()
    {
        _tmax = _onsetBaseline_s + _numReps * _period_s;

        int n = Mathf.RoundToInt((_tmax + _offsetBaseline_s) * 100);
        _log = new List<DataEntry>(n);
    }

    void Update()
    {
        if (!_running && Input.GetKeyUp(KeyCode.S))
        {
            _eyeTracker.StartTracking();
            _t0 = Time.timeSinceLevelLoad;
            _running = true;
            return; // else first pupil size will be zero
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            _abort = true;
        }

        if (!_running)
        {
            if (_abort)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Visual Scene", UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
            else return;
        }


        float t = Time.timeSinceLevelLoad - _t0;
        if (t < _onsetBaseline_s)
        {
            _intensity = 0;
        }
        else if (t < _tmax)
        {
            _intensity = 0.5f * (1 - Mathf.Cos(2 * Mathf.PI * (t - _onsetBaseline_s) / _period_s));
        }
        else
        {
            _intensity = 0;
        }
        _cam.backgroundColor = new Color(_intensity, _intensity, _intensity);

        _log.Add(new DataEntry(Time.timeSinceLevelLoad - _onsetBaseline_s, _intensity, _eyeTracker.LeftSize, _eyeTracker.RightSize));

        _running = (t <= _tmax + _offsetBaseline_s && !_abort);

        if (!_running)
        {
            _eyeTracker.StopTracking();
            if (!_abort)
            {
                SaveData();
            }
            UnityEngine.SceneManagement.SceneManager.LoadScene("Visual Scene", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
    }

    private void SaveData()
    {
        List<string> columnHeads = new List<string>();
        columnHeads.Add("Time_s");
        columnHeads.Add("Intensity");
        columnHeads.Add("LeftSize");
        columnHeads.Add("RightSize");

        var dt = System.DateTime.Now;

        int ncols = 4;
        int charsPerDatum = 10;
        var sb = new StringBuilder(_log.Count * ncols * charsPerDatum);
        sb.AppendLine("info.date = " + dt.ToString("G"));

        string headerStr = $"{ columnHeads[0],15}";
        for (int k = 1; k < columnHeads.Count; k++)
        {
            headerStr += $"\t{columnHeads[k],10}";
        }
        sb.AppendLine(headerStr);

        foreach (var e in _log)
        {
            sb.AppendLine($"{e.time_s,15:F4}\t{e.intensity,10:F4}\t{e.leftSize,10:F4}\t{e.rightSize,10:F4}");
        }

        string folder = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), "VR Data Logs");
        if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

        string dataPath = Path.Combine(folder, $"VRPupilCalibration_{dt.ToString("yyyy.MM.dd.H.m.s")}.txt");
        File.WriteAllText(dataPath, sb.ToString());
    }

}
