using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Management;

using ViveSR.anipal.Eye;

using JenksVR.VRD;

public class VisualSceneController : MonoBehaviour
{
    internal enum State { Idle, Running, Configuring, MeasuringRTT}

    Configuration _config = new Configuration();

    Canvas _canvas;
    TMPro.TextMeshProUGUI _menu;
    TMPro.TextMeshProUGUI _info;
    TMPro.TextMeshProUGUI _message;
    ConfigPanel _configPanel;

    BlobController _blobController;
    PlayerController _player;
    CompensationRig _compensationRig;

    private TcpListener _listener = null;
    private int _port = 5150;
    private bool _stopServer;

    private State _state = State.Idle;
    private bool _menuIsShowing = true;

    private float _sum_delta_t = 0;
    private int _num_delta_t = 0;
    private float _fps = float.NaN;
    private float _pollInterval = 1f;

    private bool _isVR = false;

    private const string _title = "RandomDotsVR";
    private string _configName;

    private EyeTracker _eyeTracker;
    private VRDataLogger _dataLogger;

    private void Awake()
    {
        _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        _menu = GameObject.Find("Canvas/Menu").GetComponent<TMPro.TextMeshProUGUI>();
        _info = GameObject.Find("Canvas/Info").GetComponent<TMPro.TextMeshProUGUI>();
        _message = GameObject.Find("Canvas/Message").GetComponent<TMPro.TextMeshProUGUI>();

        _configPanel = GameObject.Find("Config Panel").GetComponent<ConfigPanel>();

        _blobController = GameObject.Find("Blob Controller").GetComponent<BlobController>();
        _player = GameObject.Find("Player").GetComponent<PlayerController>();

        _compensationRig = GameObject.Find("Compensation Rig").GetComponent<CompensationRig>();

        _eyeTracker = gameObject.GetComponent<EyeTracker>();
        _dataLogger = gameObject.GetComponent<VRDataLogger>();
    }

    void Start()
    {
        Application.runInBackground = true;
#if UNITY_EDITOR
        Application.targetFrameRate = 90;
#endif
        QualitySettings.maxQueuedFrames = 1;
        Debug.Log("max queued frames = " + QualitySettings.maxQueuedFrames);

        _configPanel.OnFinished = OnEndEdit;
        _configPanel.gameObject.SetActive(false);

        LoadConfig();

        _info.gameObject.SetActive(_config.debug.showFrameRate);

        _isVR = XRGeneralSettings.Instance.Manager.activeLoader != null;

        if (_isVR || _config.debug.windowOnly)
        {
            _compensationRig.gameObject.SetActive(false);
        }
        else
        {
            _player.DisableCamera();
        }

        Screen.SetResolution(1200, 675, false);
        UpdateMenu();
        StartServer();
    }

    void LoadConfig()
    {
        _configName = VRDState.Restore().lastConfigName;

        if (!string.IsNullOrEmpty(_configName))
        {
            string configPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData), "Jenks", _configName + ".vrd.xml");
            if (File.Exists(configPath))
            {
                _config = KLib.FileIO.XmlDeserialize<Configuration>(configPath);
            }
            else
            {
                _configName = "default";
            }
        }
        else
        {
            string configPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData), "Jenks", "last_config.xml");
            if (File.Exists(configPath))
            {
                _config = KLib.FileIO.XmlDeserialize<Configuration>(configPath);
                File.Delete(configPath);
                _configName = "default";
                configPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData), "Jenks", _configName + ".vrd.xml");
                KLib.FileIO.XmlSerialize(_config, configPath);
                VRDState.Save(_configName);
            }
        }

        _configPanel.EnumerateConfigNames(_configName);
    }

    void Update()
    {
        _sum_delta_t += Time.deltaTime;
        _num_delta_t++;
        if (_sum_delta_t > _pollInterval)
        {
            _fps = _num_delta_t / _sum_delta_t;
            _num_delta_t = 0;
            _sum_delta_t = 0;

            if (_config.debug.showFrameRate)
            {
                _info.text = (_isVR ? "headset: " : "screen: ") + _fps.ToString("F1") + " fps";
            }
        }

        if (_state == State.Idle)
        {
            //if (Input.GetKeyUp(KeyCode.Z))
            //{
            //    UnityEngine.SceneManagement.SceneManager.LoadScene("ARMessingAround", UnityEngine.SceneManagement.LoadSceneMode.Single);
            //}
            if (Input.GetKeyUp(KeyCode.C)) EditConfiguration();
            if (Input.GetKeyUp(KeyCode.S) || Input.GetButton("XboxA")) StartBlobs();
            if (Input.GetKeyUp(KeyCode.R)) StartRTT();
            if (Input.GetKeyUp(KeyCode.T) || Input.GetButton("XboxY"))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("The Full Monty", UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
            if (Input.GetKeyUp(KeyCode.T) || Input.GetButton("XboxX"))
            {
#if !UNITY_EDITOR
                if (!_isVR && !_config.debug.windowOnly)
                {
                    Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                }
#endif
                UnityEngine.SceneManagement.SceneManager.LoadScene("Calibration", UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
            if (Input.GetKeyUp(KeyCode.G) && _isVR && _config.controller.eyeTracking)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Gaze Calibration", UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
            if (Input.GetKeyUp(KeyCode.H) && _isVR && _config.controller.eyeTracking)
            {
                SRanipal_Eye_v2.LaunchEyeCalibration();
            }
            if (Input.GetKeyUp(KeyCode.P) && _isVR && _config.controller.eyeTracking)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Pupil Calibration", UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
            if (Input.GetKeyUp(KeyCode.Q))
            {
                // https://answers.unity.com/questions/467030/unity-builds-crash-when-i-exit-1.html
                if (!Application.isEditor) System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }
        else if (_state == State.Running)
        {
            if (Input.GetKeyUp(KeyCode.O) || Input.GetButtonUp("XboxA")) CaptureScreenshot();
            if (Input.GetKeyUp(KeyCode.E) || Input.GetButtonUp("XboxB")) EndBlobs();
        }
        else if (_state == State.Configuring)
        {
        }
        else if (_state == State.MeasuringRTT)
        {
            if (Input.GetKeyUp(KeyCode.R)) StopRTT();
        }
    }

    void EditConfiguration()
    {
        _state = State.Configuring;
        _menu.gameObject.SetActive(false);
        _configPanel.gameObject.SetActive(true);
        _configPanel.Edit(_configName);
    }

    void OnEndEdit(bool changed, string name, Configuration value)
    {
        _configPanel.gameObject.SetActive(false);
        _menu.gameObject.SetActive(_menuIsShowing);
        _state = State.Idle;

        if (changed)
        {
            _configName = name;
            _config = value;

            _info.gameObject.SetActive(_config.debug.showFrameRate);
            UpdateMenu();

            VRDState.Save(_configName);
        }
    }

    void UpdateMenu()
    {
        _menu.text = 
            "<u>Random Dots VR</u><size=24px>\n\n" +
            "C:\tedit configuration\n\n" +
            "S:\tstart random dots\n\n" +
            "E:\tend random dots\n\n";

        if (_isVR)
        {
            if (_config.controller.eyeTracking)
            {
                _menu.text +=
                    "H:\theadset calibration\n\n" +
                    "P:\tpupil calibration\n\n" +
                    "G:\tgaze calibration\n\n";
            }
        }
        else
        {
            _menu.text +=
                "T:\ttest pattern\n\n";
        }

        _menu.text += "Q:\tquit";
    }

    void CaptureScreenshot()
    {
        string folder = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), "VR Data Logs");
        string path = Path.Combine(folder, $"Screenshot_{System.DateTime.Now.ToString("yyyy.MM.dd.H.m.s")}.png");
        if (_isVR)
        {
            ScreenCapture.CaptureScreenshot(path, ScreenCapture.StereoScreenCaptureMode.LeftEye);
        }
        else
        {
            ScreenCapture.CaptureScreenshot(path);
        }
    }

    void StartBlobs()
    {
        float hfov = _isVR ? 104f : 60f;
        _state = State.Running;
        _menu.gameObject.SetActive(false);

        if (_isVR)
        {
            _canvas.gameObject.SetActive(_config.debug.showFrameRate);
            if (_config.controller.eyeTracking)
            {
                _eyeTracker.StartTracking();
            }
        }
        else
        {
#if !UNITY_EDITOR
            if (!_config.debug.windowOnly)
            {            
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
            }
#endif
            if (_config.debug.windowOnly)
            {
                _compensationRig.gameObject.SetActive(false);
                _player.EnableCamera(hfov);
            }
            else
            {
                _player.DisableCamera();
                _compensationRig.gameObject.SetActive(true);
                _compensationRig.EnableCompensation(_player.transform);
            }
        }

        _blobController.gameObject.SetActive(true);
        _blobController.SetProperties(_config.arena, _config.blobs);
        _blobController.InitializeBlobs(_fps, hfov);
        _blobController.gameObject.SetActive(false);

        _dataLogger.StartLogging(_config, _isVR, _player, _eyeTracker);

        _player.StartController(_config.controller, _isVR, _config.debug.createLog);
    }

    void EndBlobs()
    {
        _dataLogger.StopLogging();

        if (!_isVR && !_config.debug.windowOnly)
        {
            _compensationRig.DisableCompensation();
        }
        _player.HaltController();
        _blobController.Halt();

        _blobController.gameObject.SetActive(false);

#if !UNITY_EDITOR
        if (!_isVR)
        {
            Screen.SetResolution(1200, 675, false);
        }
#endif

        if (_isVR)
        {
            _canvas.gameObject.SetActive(true);
            if (_config.controller.eyeTracking)
            {
                _eyeTracker.StopTracking();
            }
        }

        _menu.gameObject.SetActive(_menuIsShowing);
        _state = State.Idle;
    }

    private void StartServer()
    {
        _stopServer = false;

        _listener = new TcpListener(_port);
        _listener.Start();

        StartCoroutine(TCPServer());

        Debug.Log("started Random Dots VR TCP server");
    }

    private void StopServer()
    {
        _stopServer = true;
        if (_listener != null)
        {
            _listener.Stop();
            _listener = null;
        }
    }

    private void StartRTT()
    {
        _state = State.MeasuringRTT;
        _menu.gameObject.SetActive(false);
        _player.StartRTT();
        _message.text = "Running RTT server...";
    }

    private void StopRTT()
    {
        _player.StopRTT();
        _menu.gameObject.SetActive(true);
        _message.text = "";
        _state = State.Idle;
    }

    void OnDestroy()
    {
        StopServer();
    }

    IEnumerator TCPServer()
    {
        while (!_stopServer)
        {
            if (_listener.Pending())
            {
                ProcessMessage();
            }
            yield return null;
        }
    }

    void ProcessMessage()
    {
        var client = _listener.AcceptTcpClient();
        using (var network = client.GetStream())
        using (var theReader = new BinaryReader(network))
        using (var theWriter = new BinaryWriter(network))
        {
            var command = theReader.ReadString();

            Debug.Log("Command received: " + command);

            switch (command)
            {
                case "Ping":
                    break;

                case "Config":
                    var n = theReader.ReadInt32();
                    var pbuf = theReader.ReadBytes(n);
                    _config = Configuration.FromProtoBuf(pbuf);
                    break;

                case "Start":
                    StartBlobs();
                    break;

                case "End":
                    EndBlobs();
                    break;
            }
        }
    }

}
