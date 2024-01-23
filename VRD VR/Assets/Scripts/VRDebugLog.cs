using UnityEngine;
using System.IO;

public class VRDebugLog
{
    public long[] ticks;
    public float[] time;
    public float[] yaw;
    public float[] velocity;
    public float[] chairAngle;
    public float[] chairVelocity;

    private int _lengthIncrement;
    private int _index;

    private long _timeRef;

    public VRDebugLog() : this(10000) { }

    public VRDebugLog(int lengthIncrement)
    {
        _lengthIncrement = lengthIncrement;
        _index = 0;

        ticks = new long[lengthIncrement];
        time = new float[lengthIncrement];
        yaw = new float[lengthIncrement];
        velocity = new float[lengthIncrement];
        chairAngle = new float[lengthIncrement];
        chairVelocity = new float[lengthIncrement];

        var n = System.DateTime.Now;
        var dt = new System.DateTime(n.Year, n.Month, n.Day, 0, 0, 0);
        _timeRef = dt.Ticks;
    }

    public void Add(long ticks, float time, float yaw, float velocity, float udpAngle, float udpVelocity)
    {
        if (_index == this.ticks.Length)
        {
            int newLen = this.ticks.Length + _lengthIncrement;
            Resize(newLen);
        }

        this.ticks[_index] = ticks;
        this.time[_index] = time;
        this.yaw[_index] = yaw;
        this.velocity[_index] = velocity;
        this.chairAngle[_index] = udpAngle;
        this.chairVelocity[_index] = udpVelocity;

        _index++;
    }

    public void Resize(int newLen)
    {
        System.Array.Resize(ref ticks, newLen);
        System.Array.Resize(ref time, newLen);
        System.Array.Resize(ref yaw, newLen);
        System.Array.Resize(ref velocity, newLen);
        System.Array.Resize(ref chairAngle, newLen);
        System.Array.Resize(ref chairVelocity, newLen);
    }

    private void Trim(int index)
    {
        System.Array.Resize(ref ticks, index);
        System.Array.Resize(ref time, index);
        System.Array.Resize(ref yaw, index);
        System.Array.Resize(ref velocity, index);
        System.Array.Resize(ref chairAngle, index);
        System.Array.Resize(ref chairVelocity, index);
    }

    public void WriteToDisk()
    {
        string folder = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), "Debug Logs");
        if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
        string path = Path.Combine(folder, $"DebugLog_{System.DateTime.Now.ToString("yyyy.MM.dd.H.m.s")}.rdl");

        using (var s = File.Create(path))
        using (var writer = new BinaryWriter(s))
        {
            writer.Write(_timeRef);
            writer.Write(_index);

            Trim(_index);

            var bytes = new byte[_index * sizeof(long)];

            System.Buffer.BlockCopy(ticks, 0, bytes, 0, bytes.Length);
            writer.Write(bytes);

            bytes = new byte[_index * sizeof(float)];
            System.Buffer.BlockCopy(time, 0, bytes, 0, bytes.Length);
            writer.Write(bytes);
            System.Buffer.BlockCopy(yaw, 0, bytes, 0, bytes.Length);
            writer.Write(bytes);
            System.Buffer.BlockCopy(velocity, 0, bytes, 0, bytes.Length);
            writer.Write(bytes);
            System.Buffer.BlockCopy(chairAngle, 0, bytes, 0, bytes.Length);
            writer.Write(bytes);
            System.Buffer.BlockCopy(chairVelocity, 0, bytes, 0, bytes.Length);
            writer.Write(bytes);
        }
    }
}