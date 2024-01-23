using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XboxCompensationController : MonoBehaviour
{
    public class DeltaCompensation
    {
        public float pitch = 0;
        public float height = 0;
        public float offset = 0;
        public float fov = 0;
    }

    public class CompVar
    {
        private float _value;
        private float _coerced;
        private float _lowVelocity;
        private float _highVelocity;
        private float _minStep;

        float _deadZone = 0.5f;
        float _highZone = 0.999f;

        float _npadHigh = 5;
        float _npad = 0;

        public CompVar(float vlow, float vhigh, float resol)
        {
            _lowVelocity = vlow;
            _highVelocity = vhigh;
            _minStep = resol;
        }

        public float UpdateStep(float inputValue)
        {
            float step = 0;

            if (Mathf.Abs(inputValue) < _deadZone)
            {
            }
            else
            {
                float velocity = 0;
                if (Mathf.Abs(inputValue) < _highZone)
                {
                    velocity = Mathf.Sign(inputValue) * _lowVelocity * (Mathf.Abs(inputValue) - _deadZone) / (_highZone - _deadZone);
                }
                else
                {
                    velocity = Mathf.Sign(inputValue) * _highVelocity * (Mathf.Abs(inputValue) - _highZone) / (1 - _highZone);
                }

                _value += velocity * Time.deltaTime;

                float newCoerced = Mathf.Round(_value / _minStep) * _minStep;

                step = newCoerced - _coerced;
                _coerced = newCoerced;
            }

            return step;
        }

        public  float UpdateStepFromDPAD(float dpadValue)
        {
            float step = 0;

            if (dpadValue != 0)
            {
                float velocity = Mathf.Sign(dpadValue) * (_npad < _npadHigh ? _lowVelocity : _highVelocity);
                _value += velocity * Time.deltaTime;
                float newCoerced = Mathf.Round(_value / _minStep) * _minStep;

                step = newCoerced - _coerced;
                _coerced = newCoerced;

                if (step != 0) _npad++;
            }
            else
            {
                _npad = 0;
            }

            return step;
        }
    }

    CompVar _pitch = new CompVar(0.5f, 5, 0.1f);
    CompVar _height = new CompVar(0.01f * 0.5f, 0.01f * 5, 0.01f * 0.1f);
    CompVar _offset = new CompVar(0.01f * 0.5f, 0.01f * 5, 0.01f * 0.1f);
    CompVar _fov = new CompVar(0.5f, 5, 0.1f);

    DeltaCompensation _delta = new DeltaCompensation();

    public DeltaCompensation GetDeltas()
    {
        _delta.pitch = _pitch.UpdateStep(Input.GetAxis("LeftVertical"));
        _delta.offset = _offset.UpdateStep(Input.GetAxis("RightVertical"));
        _delta.fov = _fov.UpdateStep(Input.GetAxis("Horizontal"));
        _delta.height = _height.UpdateStepFromDPAD(Input.GetAxis("XboxDPADVertical"));

        return _delta;
    }


}