using ProtoBuf;

namespace JenksVR.VRD
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class BlobProperties
    {
        public enum MovementMode { Brownian, WhiteNoise}
        public enum MovementCategory { Coherent, Brownian, WhiteNoise}
        public enum NoiseUnits { DegPerSec, PercentOfChair}

        [ProtoMember(1, IsRequired = true)]
        public float diameter_cm = 1f;

        [ProtoMember(2, IsRequired = true)]
        public float lifeTime_ms = 50;

        [ProtoMember(3, IsRequired = true)]
        public float deadTime_ms = 0;

        [ProtoMember(4, IsRequired = true)]
        public bool strobed = false;

        [ProtoMember(5, IsRequired = true)]
        public float density = 450;

        [ProtoMember(6, IsRequired = true)]
        public MovementMode movementMode = MovementMode.Brownian;

        [ProtoMember(7, IsRequired = true)]
        public float coherence = 0;

        [ProtoMember(8, IsRequired = true)]
        public bool addChairVelocity = true;

        [ProtoMember(9, IsRequired = true)]
        public float horizontalStdDev;

        [ProtoMember(10, IsRequired = true)]
        public float verticalStdDev;

        [ProtoMember(11, IsRequired = true)]
        public bool identicalNoise;

        [ProtoMember(12, IsRequired = true)]
        public NoiseUnits noiseUnits;
    }
}
