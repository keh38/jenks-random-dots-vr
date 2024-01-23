using ProtoBuf;

namespace JenksVR.VRD
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class ArenaProperties
    {
        public float radius = 1.14f;
        public float height = 3.5f;
    }
}
