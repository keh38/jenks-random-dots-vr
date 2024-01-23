using ProtoBuf;

namespace JenksVR.VRD
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class DebugSettings
    {
        [ProtoMember(1, IsRequired = true)]
        public bool showFrameRate = false;

        [ProtoMember(2, IsRequired = true)]
        public bool windowOnly = false;

        [ProtoMember(3, IsRequired = true)]
        public bool createLog = false;
    }
}
