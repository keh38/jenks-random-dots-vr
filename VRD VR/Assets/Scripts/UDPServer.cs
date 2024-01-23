using UnityEngine;
using System.Collections;
using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class UDPServer : MonoBehaviour
{
    private float _angle;
    private float _velocity;

    Thread _readThread = null;

    UdpClient _client;
    private int _port = 61557;
    IPEndPoint _remoteIP;

    private bool _useLog;
    private UDPDebugLog _log = null;

    private byte[] _udpData;

    private void Start()
    {
    }

    public void Initialize(string chairAddress)
    {
        _client = new UdpClient(_port);
        _client.Client.ReceiveTimeout = 1000;

        if (chairAddress.Equals("localhost"))
        {
            _remoteIP = new IPEndPoint(IPAddress.Loopback, _port + 1);
        }
        else
        {
            _remoteIP = new IPEndPoint(IPAddress.Parse(chairAddress), _port + 1);
        }

        _udpData = new byte[8];
    }

    public float Angle { get { return _angle; } }
    public float Velocity { get { return _velocity; } }

    //public void StartReceiving()
    //{
    //    // create thread for reading UDP messages
    //    _readThread = new Thread(new ThreadStart(ReceiveData));
    //    _readThread.IsBackground = true;

    //    _angle = float.NaN;
    //    _angle = 0;

    //    _readThread.Start();
    //    Debug.Log("started chair UDP server thread");
    //}

    //public void StopReceiving()
    //{
    //    StopThread();
    //}


    public void StartRTT()
    {
        // create thread for reading UDP messages
        _readThread = new Thread(new ThreadStart(PerformRTT));
        _readThread.IsBackground = true;

        _angle = float.NaN;
        _angle = 0;

        _readThread.Start();
        Debug.Log("started RTT UDP server thread");
    }

    public void StopRTT()
    {
        StopThread();
    }

    //public void StartLog()
    //{
    //    _useLog = true;
    //    _log = new UDPDebugLog();
    //}

    //public void StopLog()
    //{
    //    _useLog = false;
    //    _log.WriteToDisk();
    //}

    // Stop reading UDP messages
    private void StopThread()
    {
        if (_readThread != null &&_readThread.IsAlive)
        {
            _readThread.Abort();
            _client.Close();
            Debug.Log("UDP thread aborted");
        }
    }

    public void Halt()
    {
        _client.Close();
    }

    private void OnDestroy()
    {
        StopThread();
    }

    //private void ReceiveData()
    //{
    //    _client = new UdpClient(_port);
    //    _client.Client.ReceiveTimeout = 1000;

    //    while (true)
    //    {
    //        try
    //        {
    //            // receive bytes
    //            IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, _port);
    //            byte[] data = _client.Receive(ref anyIP);

    //            // Note the minus sign: NKI and Unity have opposite conventions
    //            _angle = -BitConverter.ToSingle(data, 0);
    //            _velocity = BitConverter.ToSingle(data, 4);
    //            float time = BitConverter.ToSingle(data, 8);

    //            if (_useLog)
    //            {
    //                _log.Add(DateTime.Now.Ticks, _angle, _velocity, time);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            //Debug.Log(ex.Message);
    //        }
    //    }
    //}

    public float RequestStateVariables()
    {
        _client.Send(BitConverter.GetBytes(DateTime.Now.Ticks), 8, _remoteIP);

        IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, _port);
        byte[] data = _client.Receive(ref anyIP);

        // Note the minus sign: NKI and Unity have opposite conventions
        _angle = -BitConverter.ToSingle(data, 0);
        _velocity = BitConverter.ToSingle(data, 4);

        return _velocity;
    }

    private void PerformRTT()
    {
        IPEndPoint remoteIP = null;
        _client = new UdpClient(_port + 2);

        while (true)
        {
            try
            {
                // receive bytes
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, _port + 2);
                byte[] data = _client.Receive(ref anyIP);

                // Note the minus sign: NKI and Unity have opposite conventions
                _angle = -BitConverter.ToSingle(data, 0);

                if (remoteIP == null)
                {
                    remoteIP = new IPEndPoint(anyIP.Address, _port + 3);
                }

                _client.Send(data, data.Length, remoteIP);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }
    }
}