using System.IO;
using UnityEngine;


public class ProjectorSettings
{
    public float pitch = 36.5f;
    public float yOffset = 0;
    public float zOffset = 0.3f;
    public float fov = 60f;

    public ProjectorSettings() { }

    public static ProjectorSettings Restore()
    {
        ProjectorSettings settings = new ProjectorSettings();

        //string path = Path.Combine(Application.persistentDataPath, "projector_settings.xml");
        string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData), "Jenks", "projector_settings.xml");
        if (File.Exists(path))
        {
            settings = KLib.FileIO.XmlDeserialize<ProjectorSettings>(path);
        }

        return settings;
    }

    public void Save()
    {
        string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData), "Jenks", "projector_settings.xml");
        KLib.FileIO.XmlSerialize(this, path);
    }
}
