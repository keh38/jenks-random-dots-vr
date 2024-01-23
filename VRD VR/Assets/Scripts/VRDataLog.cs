using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class VRDataLog
{
    private StringBuilder _data;
    private int _lengthIncrement;

    private string _dataPath;


    public VRDataLog() : this(30000) { }

    public VRDataLog(int lengthIncrement)
    {
        _lengthIncrement = lengthIncrement;
    }

    public void Initialize(string header, bool eyeTracking, bool gazeCal)
    {
        List<string> columnHeads = new List<string>();
        columnHeads.Add("Time_s");
        if (gazeCal)
        {
            columnHeads.Add("TargetX");
            columnHeads.Add("TargetY");
        }
        else
        {
            columnHeads.Add("Velocity");
        }

        if (eyeTracking)
        {
            columnHeads.Add("GazeAngX");
            columnHeads.Add("GazeAngY");
            columnHeads.Add("GazeX");
            columnHeads.Add("GazeY");
            columnHeads.Add("GazeZ");
            columnHeads.Add("LeftPosX");
            columnHeads.Add("LeftPosY");
            columnHeads.Add("LeftAngX");
            columnHeads.Add("LeftAngY");
            columnHeads.Add("LeftGazeX");
            columnHeads.Add("LeftGazeY");
            columnHeads.Add("LeftGazeZ");
            columnHeads.Add("LeftSize");
            columnHeads.Add("LeftOpen");
            columnHeads.Add("RightPosX");
            columnHeads.Add("RightPosY");
            columnHeads.Add("RightAngX");
            columnHeads.Add("RightAngY");
            columnHeads.Add("RightGazeX");
            columnHeads.Add("RightGazeY");
            columnHeads.Add("RightGazeZ");
            columnHeads.Add("RightSize");
            columnHeads.Add("RightOpen");
        }

        var dt = System.DateTime.Now;

        int ncols = columnHeads.Count;
        int charsPerDatum = 10;
        _data = new StringBuilder(header.Length + _lengthIncrement * ncols * charsPerDatum);
        _data.AppendLine("info.date = " + dt.ToString("G"));
        _data.Append(header);

        string headerStr = $"{ columnHeads[0],15}";
        for (int k = 1; k < columnHeads.Count; k++)
        {
            headerStr += $"\t{columnHeads[k],10}";
        }
        _data.AppendLine(headerStr);

        string folder = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), "VR Data Logs");
        if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
        _dataPath = Path.Combine(folder, $"VRDataLog_{dt.ToString("yyyy.MM.dd.H.m.s")}.txt");
    }

    public void StartEntry(float time, float velocity)
    {
        _data.Append($"{time,15:F4}\t{velocity,10:F4}");
    }

    public void StartGazeEntry(float time, Vector2 targetPosition)
    {
        _data.Append($"{time,15:F4}\t{targetPosition.x,10:F4}\t{targetPosition.y,10:F4}");
    }

    public void AddGaze(Vector2 gazeAngle, Vector3 gazeDir)
    {
        _data.Append($"\t{gazeAngle.x,10:F4}\t{gazeAngle.y,10:F4}\t{gazeDir.x,10:F4}\t{gazeDir.y,10:F4}\t{gazeDir.z,10:F4}");
    }

    public void AddEye(Vector2 position, Vector2 gaze, Vector3 gazeDir, float size, float openness)
    {
        _data.Append($"\t{position.x,10:F4}\t{position.y,10:F4}");
        _data.Append($"\t{gaze.x,10:F4}\t{gaze.y,10:F4}");
        _data.Append($"\t{gazeDir.x,10:F4}\t{gazeDir.y,10:F4}\t{gazeDir.z,10:F4}");
        _data.Append($"\t{size,10:F4}\t{openness,10:F4}");
    }

    public void EndEntry()
    {
        _data.Append(System.Environment.NewLine);
    }

    public void WriteToDisk(string path)
    {
        File.WriteAllText(path, _data.ToString());
    }

    public void WriteToDisk()
    {
        WriteToDisk(_dataPath);
    }
}