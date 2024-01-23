using UnityEngine;
using System.IO;

public class UDPDebugLog
{
    public long[] ticks;
    public float[] angle;
    public float[] velocity;
    public float[] tremote;

    private int _lengthIncrement;
    private int _index;

    private long _timeRef;

    public UDPDebugLog() : this(100000) { }

    public UDPDebugLog(int lengthIncrement)
    {
        _lengthIncrement = lengthIncrement;
        _index = 0;

        ticks = new long[lengthIncrement];
        angle = new float[lengthIncrement];
        velocity = new float[lengthIncrement];
        tremote = new float[lengthIncrement];

        var n = System.DateTime.Now;
        var dt = new System.DateTime(n.Year, n.Month, n.Day, 0, 0, 0);
        _timeRef = dt.Ticks;
    }

    public void Add(long ticks, float angle, float velocity, float tremote)
    {
        if (_index == this.ticks.Length)
        {
            int newLen = this.ticks.Length + _lengthIncrement;
            Resize(newLen);
        }

        this.ticks[_index] = ticks;
        this.angle[_index] = angle;
        this.velocity[_index] = velocity;
        this.tremote[_index] = tremote;

        _index++;
    }

    public void Resize(int newLen)
    {
        System.Array.Resize(ref ticks, newLen);
        System.Array.Resize(ref angle, newLen);
        System.Array.Resize(ref velocity, newLen);
        System.Array.Resize(ref tremote, newLen);
    }

    private void Trim(int index)
    {
        System.Array.Resize(ref ticks, index);
        System.Array.Resize(ref angle, index);
        System.Array.Resize(ref velocity, index);
        System.Array.Resize(ref tremote, index);
    }

    public void WriteToDisk()
    {
        string path = Path.Combine(Application.persistentDataPath, $"DebugLog_{System.DateTime.Now.ToString("yyyy.MM.dd.H.m.s")}.udp");

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
            System.Buffer.BlockCopy(angle, 0, bytes, 0, bytes.Length);
            writer.Write(bytes);
            System.Buffer.BlockCopy(velocity, 0, bytes, 0, bytes.Length);
            writer.Write(bytes);
            System.Buffer.BlockCopy(tremote, 0, bytes, 0, bytes.Length);
            writer.Write(bytes);
        }
    }
}