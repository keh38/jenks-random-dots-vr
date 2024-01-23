using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using ViveSR.anipal.Eye;

public class EyeTracker : MonoBehaviour
{
    protected static EyeData_v2 _eyeData = new EyeData_v2();
    Vector2 _leftPupilPosition;
    Vector2 _rightPupilPosition;
    float _leftOpenness;
    float _rightOpenness;
    Vector3 _gazeOrigin;
    Vector3 _gazeDirection;
    Vector3 _leftGaze;
    Vector3 _rightGaze;

    bool _isTracking = false;
    bool _eyeCallbackRegistered = false;

    void Start()
    {
//        Debug.Log("enabled: " + SRanipal_Eye_Framework.Instance.EnableEye.ToString());
    }

    public Vector2 LeftGazeAngle { get; private set; }
    public Vector2 RightGazeAngle { get; private set; }
    public Vector2 GazeAngle { get; private set; }
    public Vector2 LeftPosition { get { return _leftPupilPosition; } }
    public Vector2 RightPosition { get { return _rightPupilPosition; } }
    public float LeftSize { get;  private set;  }
    public float RightSize { get; private set; }
    public float LeftOpenness { get { return _leftOpenness; } }
    public float RightOpenness { get { return _rightOpenness; } }
    public Vector3 LeftGaze { get { return _leftGaze; } }
    public Vector3 RightGaze { get { return _rightGaze; } }
    public Vector3 GazeDir { get { return _gazeDirection; } }

    public void StartTracking()
    {
        _isTracking = true;
    }

    public void StopTracking()
    {
        _isTracking = false;
    }

    void Update()
    {
        if (!_isTracking) return;

        if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING &&
                        SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.NOT_SUPPORT) return;

        if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == true && _eyeCallbackRegistered == false)
        {
            SRanipal_Eye_v2.WrapperRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
            _eyeCallbackRegistered = true;
        }
        else if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == false && _eyeCallbackRegistered == true)
        {
            SRanipal_Eye_v2.WrapperUnRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
            _eyeCallbackRegistered = false;
        }


        EyeData_v2 e2 = new EyeData_v2();
        SRanipal_Eye_API.GetEyeData_v2(ref e2);


        // Pupil diameter
        LeftSize = float.NaN;
        RightSize = float.NaN;

        bool isValid;
        isValid = e2.verbose_data.left.GetValidity(SingleEyeDataValidity.SINGLE_EYE_DATA_PUPIL_DIAMETER_VALIDITY);
        if (isValid)
        {
            LeftSize = e2.verbose_data.left.pupil_diameter_mm;
        }
        isValid = e2.verbose_data.right.GetValidity(SingleEyeDataValidity.SINGLE_EYE_DATA_PUPIL_DIAMETER_VALIDITY);
        if (isValid)
        {
            RightSize = e2.verbose_data.right.pupil_diameter_mm;
        }

        bool leftPupilValid;
        bool rightPupilValid;
        bool gazeValid;
        bool leftGazeValid;
        bool rightGazeValid;
        bool leftOpenValid;
        bool rightOpenValid;

        if (_eyeCallbackRegistered)
        {
            leftPupilValid = SRanipal_Eye_v2.GetPupilPosition(EyeIndex.LEFT, out _leftPupilPosition, _eyeData);

            rightPupilValid = SRanipal_Eye_v2.GetPupilPosition(EyeIndex.RIGHT, out _rightPupilPosition, _eyeData);

            gazeValid = SRanipal_Eye_v2.GetGazeRay(GazeIndex.COMBINE, out _gazeOrigin, out _gazeDirection, _eyeData);
            leftGazeValid = SRanipal_Eye_v2.GetGazeRay(GazeIndex.LEFT, out _gazeOrigin, out _leftGaze, _eyeData);
            rightGazeValid = SRanipal_Eye_v2.GetGazeRay(GazeIndex.RIGHT, out _gazeOrigin, out _rightGaze, _eyeData);

            leftOpenValid = SRanipal_Eye_v2.GetEyeOpenness(EyeIndex.LEFT, out _leftOpenness, _eyeData);
            rightOpenValid = SRanipal_Eye_v2.GetEyeOpenness(EyeIndex.RIGHT, out _rightOpenness, _eyeData);
        }
        else
        {
            leftPupilValid = SRanipal_Eye_v2.GetPupilPosition(EyeIndex.LEFT, out _leftPupilPosition);
            rightPupilValid = SRanipal_Eye_v2.GetPupilPosition(EyeIndex.RIGHT, out _rightPupilPosition);

            gazeValid = SRanipal_Eye_v2.GetGazeRay(GazeIndex.COMBINE, out _gazeOrigin, out _gazeDirection);
            leftGazeValid = SRanipal_Eye_v2.GetGazeRay(GazeIndex.COMBINE, out _gazeOrigin, out _leftGaze);
            rightGazeValid = SRanipal_Eye_v2.GetGazeRay(GazeIndex.COMBINE, out _gazeOrigin, out _rightGaze);

            leftOpenValid = SRanipal_Eye_v2.GetEyeOpenness(EyeIndex.LEFT, out _leftOpenness);
            rightOpenValid = SRanipal_Eye_v2.GetEyeOpenness(EyeIndex.RIGHT, out _rightOpenness);
        }

        if (!leftPupilValid) _leftPupilPosition = new Vector2(float.NaN, float.NaN);
        if (!rightPupilValid) _rightPupilPosition = new Vector2(float.NaN, float.NaN);
        if (!leftOpenValid) _leftOpenness = float.NaN;
        if (!rightOpenValid) _rightOpenness = float.NaN;

        if (leftGazeValid)
        {
            float x = Vector3.Angle(_leftGaze, new Vector3(0, _leftGaze.y, _leftGaze.z));
            float y = Vector3.Angle(_leftGaze, new Vector3(_leftGaze.x, 0, _leftGaze.z));

            if (_leftGaze.x < 0) x = -x;
            if (_leftGaze.y < 0) y = -y;

            LeftGazeAngle = new Vector2(x, y);
        }
        else
        {
            LeftGazeAngle = new Vector2(float.NaN, float.NaN);
            _leftGaze = new Vector3(float.NaN, float.NaN, float.NaN);
        }

        if (rightGazeValid)
        {
            float x = Vector3.Angle(_rightGaze, new Vector3(0, _rightGaze.y, _rightGaze.z));
            float y = Vector3.Angle(_rightGaze, new Vector3(_rightGaze.x, 0, _rightGaze.z));

            if (_rightGaze.x < 0) x = -x;
            if (_rightGaze.y < 0) y = -y;

            RightGazeAngle = new Vector2(x, y);
        }
        else
        {
            RightGazeAngle = new Vector2(float.NaN, float.NaN);
            _rightGaze = new Vector3(float.NaN, float.NaN, float.NaN);
        }

        if (gazeValid)
        {
            float x = Vector3.Angle(_gazeDirection, new Vector3(0, _gazeDirection.y, _gazeDirection.z));
            float y = Vector3.Angle(_gazeDirection, new Vector3(_gazeDirection.x, 0, _gazeDirection.z));

            if (_gazeDirection.x < 0) x = -x;
            if (_gazeDirection.y < 0) y = -y;

            GazeAngle = new Vector2(x, y);
        }
        else
        {
            GazeAngle = new Vector2(float.NaN, float.NaN);
        }
    }

    private static void EyeCallback(ref EyeData_v2 eyeData)
    {
        _eyeData = eyeData;
    }

}
