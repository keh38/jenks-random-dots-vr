using System;
using System.IO;
using System.Xml.Serialization;

using ProtoBuf;

namespace JenksVR.VRD
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class ControllerSettings
    {
        public enum Feedback { Chair, HeadTracker, Simulated };

        public Feedback feedback = Feedback.HeadTracker;
        public bool eyeTracking = false;
        public float fov = 130f;
        public float cameraOffset = 0f;
        public string chairAddress = "11.12.13.14";

        public ControllerSettings() { }
    }
}