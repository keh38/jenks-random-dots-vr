using System.Collections;
using System.Collections.Generic;

using ProtoBuf;

namespace JenksVR.VRD
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class Configuration
    {
        public ArenaProperties arena = new ArenaProperties();
        public BlobProperties blobs = new BlobProperties();
        public ControllerSettings controller = new ControllerSettings();
        public DebugSettings debug = new DebugSettings();

        public byte[] ToProtoBuf()
        {
            byte[] pbuf;
            using (var ms = new System.IO.MemoryStream())
            {
                Serializer.Serialize<Configuration>(ms, this);
                pbuf = ms.ToArray();
            }
            return pbuf;
        }

        public static Configuration FromProtoBuf(byte[] pbuf)
        {
            Configuration config = null;
            using (var ms = new System.IO.MemoryStream(pbuf))
            {
                config = Serializer.Deserialize<Configuration>(ms);
            }

            return config;
        }

        public override string ToString()
        {
            string result = "";
            result += "arena.height_m = " + arena.height.ToString() + System.Environment.NewLine;
            result += "arena.radius_m = " + arena.radius.ToString() + System.Environment.NewLine;

            result += "blob.diameter_cm = " + blobs.diameter_cm.ToString() + System.Environment.NewLine;
            result += "blob.lifeTime_ms = " + blobs.lifeTime_ms.ToString() + System.Environment.NewLine;
            result += "blob.deadTime_ms = " + blobs.deadTime_ms.ToString() + System.Environment.NewLine;
            result += "blob.strobed = " + blobs.strobed.ToString() + System.Environment.NewLine;
            result += "blob.density = " + blobs.density.ToString() + System.Environment.NewLine;
            result += "blob.movementMode = " + blobs.movementMode.ToString() + System.Environment.NewLine;
            result += "blob.coherence = " + blobs.coherence.ToString() + System.Environment.NewLine;
            result += "blob.addChairVelocity = " + blobs.addChairVelocity.ToString() + System.Environment.NewLine;
            result += "blob.horizontalStdDev = " + blobs.horizontalStdDev.ToString() + System.Environment.NewLine;
            result += "blob.verticalStdDev = " + blobs.verticalStdDev.ToString() + System.Environment.NewLine;

            result += "controller.feedback = " + controller.feedback.ToString() + System.Environment.NewLine;
            result += "controller.eyeTracking = " + controller.eyeTracking.ToString() + System.Environment.NewLine;
            result += "controller.fov = " + controller.fov.ToString() + System.Environment.NewLine;
            result += "cameraOffset.feedback = " + controller.cameraOffset.ToString() + System.Environment.NewLine;
            result += "chairAddress.feedback = " + controller.chairAddress.ToString() + System.Environment.NewLine;

            return result;
        }
    }
}