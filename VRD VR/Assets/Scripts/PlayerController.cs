using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using JenksVR.VRD;

public class PlayerController : MonoBehaviour
{
    private UDPServer _udpServerChair;
    //private float _startAngle = float.NaN;
    private float _lastAngle = float.NaN;

    private Gamepad _gamepad;
    private float _yawVelocity;

    private ControllerSettings _controllerSettings;
    private bool _isRunning = false;
    
    private Camera _headCamera;
    private UnityEngine.SpatialTracking.TrackedPoseDriver _trackedPoseDriver;

    private bool _saveLog = false;
    private VRDebugLog _debugLog = new VRDebugLog();

    public float DeltaYaw { private set; get; }
    public float YawVelocity { get { return _yawVelocity; } }

    public GameObject _blobController;

    private void Awake()
    {
        _udpServerChair = gameObject.GetComponent<UDPServer>();

        _headCamera = GameObject.Find("Player").GetComponentInChildren<Camera>();
        _trackedPoseDriver = _headCamera.gameObject.GetComponent<UnityEngine.SpatialTracking.TrackedPoseDriver>();
        _blobController = GameObject.Find("Blob Controller");
    }

    void Start()
    {
        _gamepad = Gamepad.current;
        DeltaYaw = 0;

        //_udpServerChair.StartReceiving();
    }
       
    public void EnableCamera(float fov)
    {
        var c = GameObject.Find("Player/Camera").GetComponent<Camera>();
        c.enabled = true;
        var aspectRatio = (float)Screen.width / Screen.height;
        float verticalFOV = Mathf.Rad2Deg * 2 * Mathf.Atan(Mathf.Tan((fov * Mathf.Deg2Rad) / 2f) / aspectRatio);
        c.fieldOfView = verticalFOV;
    }

    public float GetHorizontalFOV()
    {
        return 60f;
    }

    public void DisableCamera()
    {
        GameObject.Find("Player/Camera").GetComponent<Camera>().enabled = false;
    }

    public void StartRTT()
    {
        //_udpServerChair.StopReceiving();
        _udpServerChair.StartRTT();
    }

    public void StopRTT()
    {
        _udpServerChair.StopRTT();
        //_udpServerChair.StartReceiving();
    }

    public void StartController(ControllerSettings controllerSettings, bool isVR, bool saveLog)
    {
        _controllerSettings = controllerSettings;
        _saveLog = saveLog;

        if (_controllerSettings.feedback == ControllerSettings.Feedback.HeadTracker && !isVR)
        {
            Debug.Log("WARNING: headtracking enabled but no VR headset found. Using chair feedback instead");
            _controllerSettings.feedback = ControllerSettings.Feedback.Chair;
        }

        _trackedPoseDriver.enabled = (_controllerSettings.feedback == ControllerSettings.Feedback.HeadTracker);

        if (_controllerSettings.feedback == ControllerSettings.Feedback.HeadTracker)
        {
            transform.position = new Vector3(controllerSettings.cameraOffset, -_headCamera.transform.localPosition.y, 0);
        }

        _udpServerChair.Initialize(controllerSettings.chairAddress);
        //_startAngle = _udpServerChair.Angle;
        _isRunning = true;

        if (_saveLog)
        {
            _debugLog = new VRDebugLog();
            //_udpServerChair.StartLog();
        }
    }

    public void HaltController()
    {
        _isRunning = false;
        if (_saveLog)
        {
            _debugLog.WriteToDisk();
            //_udpServerChair.StopLog();
        }
        _udpServerChair.Halt();
    }

    void Update()
    {
        if (!_isRunning) return;

        if (_controllerSettings.feedback == ControllerSettings.Feedback.Simulated)
        {
            float rval = 0;

            //
            // Yaw rotation: this is just for development/debugging. In the full application, the chair rotation is controlled by the 
            // external joystick app. But a couple of notes:
            // 1. The game pad (Input System) is not recognized in the standalone app with this version of Unity. (But did work in NAVI VR using 2019.1.xxx)
            // 2. The old Input does not run in the background, so the standalone app must be in focus.
            // Since this is just for testing, I'm not going to look into it further (specifically, getting the Input System to work correctly).
            //
            if (_gamepad != null)
            {
                rval = _gamepad.leftStick.ReadValue().x;
            }
            else
            {
                rval = Input.GetAxisRaw("Horizontal");
            }

            if (Mathf.Abs(rval) < 0.1f) rval = 0;
            UpdateYawVelocity(rval);

            float dr = _yawVelocity * Time.deltaTime;
            //transform.Rotate(0, dr, 0);

            DeltaYaw = dr;
        }
        else if (_controllerSettings.feedback == ControllerSettings.Feedback.Chair)
        {
            _yawVelocity = _udpServerChair.RequestStateVariables();
            if (!float.IsNaN(_yawVelocity))
            {
                _blobController.SetActive(true);
                DeltaYaw = _yawVelocity * Time.deltaTime;
            }
            else
            {
                _blobController.SetActive(false);
            }
            //transform.Rotate(0, DeltaYaw, 0);
            //if (!float.IsNaN(_udpServerChair.Angle))
            //{
            //    float newAngle = _udpServerChair.Angle - _startAngle;
            //    DeltaYaw = newAngle - transform.eulerAngles.y;
            //    transform.eulerAngles = new Vector3(0, newAngle, 0);
            //    _yawVelocity = DeltaYaw / Time.deltaTime;
            //}
        }
        else if (_controllerSettings.feedback == ControllerSettings.Feedback.HeadTracker)
        {
            //if (float.IsNaN(_lastAngle))
            //{
            //    _lastAngle = _headCamera.transform.eulerAngles.y;
            //}
            //else
            //{
            //    DeltaYaw = _headCamera.transform.eulerAngles.y - _lastAngle;
            //    _lastAngle = _headCamera.transform.eulerAngles.y;
            //}
        }

        if (_saveLog)
        {
            _debugLog.Add(System.DateTime.Now.Ticks, Time.unscaledTime, transform.eulerAngles.y, _yawVelocity, _udpServerChair.Angle, _udpServerChair.Velocity);
        }
    }

    private void UpdateYawVelocity(float inputValue)
    {
        float vtarget = inputValue * 100; 
        vtarget = Mathf.Sign(vtarget) * Mathf.Min(Mathf.Abs(vtarget), 100);

        _yawVelocity = vtarget;
    }
}
