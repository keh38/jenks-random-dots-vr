using System.IO;
using UnityEngine;

using KLib;

public class VRDState
{
    public string lastConfigName = "";

    public VRDState() { }

    public static VRDState Restore()
    {
        VRDState settings = new VRDState();

        string path = Path.Combine(Application.persistentDataPath, "VRDState.xml");
        if (File.Exists(path))
        {
            settings = FileIO.XmlDeserialize<VRDState>(path);
        }

        return settings;
    }

    public static void Save(string configName)
    {
        VRDState settings = new VRDState() { lastConfigName = configName };
        string path = Path.Combine(Application.persistentDataPath, "VRDState.xml");
        FileIO.XmlSerialize(settings, path);
    }
}
