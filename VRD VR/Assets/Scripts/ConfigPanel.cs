using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

using JenksVR.VRD;

public class ConfigPanel : MonoBehaviour
{
    private Configuration _config;

    // Config name
    private Dropdown _nameMenu;
    private InputField _nameEdit;

    // Arena
    private InputField _radius;
    private InputField _height;
    // Dots
    private InputField _diameter;
    private InputField _density;
    private InputField _lifetime;
    private InputField _deadtime;
    private Toggle _strobed;
    // Motion
    private Dropdown _motionMode;
    private InputField _coherence;
    private InputField _horzStdDev;
    private InputField _vertStdDev;
    private Dropdown _noiseUnits;
    private Toggle _addVChair;
    private Toggle _identicalNoise;
    // Control
    private Dropdown _feedback;
    private InputField _camOffset;
    private InputField _chairAddress;
    private Toggle _eyeTracking;
    // Debugging
    private Toggle _showFrameRate;
    private Toggle _windowOnly;
    private Toggle _createLog;

    public delegate void OnFinishedDelegate(bool changed, string name, Configuration value);
    private OnFinishedDelegate _onFinished = null;

    public OnFinishedDelegate OnFinished { set { _onFinished = value; } }

    private void Awake()
    {
        _nameMenu = GameObject.Find("Config Menu").GetComponent<Dropdown>();
        _nameEdit = GameObject.Find("Config Name Edit").GetComponent<InputField>();

        _radius = GameObject.Find("Arena/Radius Input").GetComponent<InputField>();
        _height = GameObject.Find("Arena/Height Input").GetComponent<InputField>();

        _diameter = GameObject.Find("Dots/Diameter Input").GetComponent<InputField>();
        _density = GameObject.Find("Dots/Density Input").GetComponent<InputField>();
        _lifetime = GameObject.Find("Dots/Lifetime Input").GetComponent<InputField>();
        _deadtime = GameObject.Find("Dots/Deadtime Input").GetComponent<InputField>();
        _strobed = GameObject.Find("Dots/Strobed").GetComponent<Toggle>();

        _motionMode = GameObject.Find("Motion/Motion Mode").GetComponent<Dropdown>();
        _coherence = GameObject.Find("Motion/Coherence Input").GetComponent<InputField>();
        _horzStdDev = GameObject.Find("Motion/Horz Std Dev").GetComponent<InputField>();
        _vertStdDev = GameObject.Find("Motion/Vert Std Dev").GetComponent<InputField>();
        _noiseUnits = GameObject.Find("Motion/Noise Units").GetComponent<Dropdown>();
        _addVChair = GameObject.Find("Motion/Add Chair Velocity").GetComponent<Toggle>();
        _identicalNoise = GameObject.Find("Motion/Identical Noise").GetComponent<Toggle>();

        _feedback = GameObject.Find("Control/Feedback").GetComponent<Dropdown>();
        _chairAddress = GameObject.Find("Control/Chair Address").GetComponent<InputField>();
        _camOffset = GameObject.Find("Control/Offset Input").GetComponent<InputField>();
        _eyeTracking = GameObject.Find("Control/Eye Tracking").GetComponent<Toggle>();

        _showFrameRate = GameObject.Find("Debug/Show Frame Rate").GetComponent<Toggle>();
        _windowOnly = GameObject.Find("Debug/Window Only").GetComponent<Toggle>();
        _createLog = GameObject.Find("Debug/Create Log").GetComponent<Toggle>();
    }

    private void Start()
    {
    }

    public void EnumerateConfigNames(string selected)
    {
        var items = new List<Dropdown.OptionData>();
        var files = Directory.EnumerateFiles(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData), "Jenks"), "*.vrd.xml");
        foreach (var f in files)
        {
            var n = Path.GetFileNameWithoutExtension(f);
            items.Add(new Dropdown.OptionData(n.Replace(".vrd","")));
        }

        _nameMenu.options = items;
        _nameMenu.value = items.FindIndex(x => x.text.Equals(selected));
    }

    public void NameSelectionChanged()
    {
        string configName = _nameMenu.options[_nameMenu.value].text;
        string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData), "Jenks", configName + ".vrd.xml");

        if (File.Exists(path))
        {
            _config = KLib.FileIO.XmlDeserialize<Configuration>(path);
            ShowValues(_config);
        }
    }

    public void SaveButtonPressed()
    {
        _nameEdit.text = _nameMenu.options[_nameMenu.value].text;
        _nameEdit.Select();
        _nameMenu.gameObject.SetActive(false);
    }

    public void OnEndNameEdit()
    {
        _nameMenu.gameObject.SetActive(true);
        if (!string.IsNullOrEmpty(_nameEdit.text))
        {
            ScanConfig();
            string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData), "Jenks", _nameEdit.text + ".vrd.xml");
            KLib.FileIO.XmlSerialize(_config, path);
            EnumerateConfigNames(_nameEdit.text);
        }
    }

    public void Edit(string configName)
    {
        string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData), "Jenks", configName + ".vrd.xml");
        if (File.Exists(path))
        {
            _config = KLib.FileIO.XmlDeserialize<Configuration>(path);
        }
        else
        {
            _config = new Configuration();
            configName = "default";
        }

        _nameMenu.value = _nameMenu.options.FindIndex(x => x.text.Equals(configName));
        ShowPanel();
    }

    private void ShowPanel()
    {
        ShowValues(_config);

        var rt = gameObject.GetComponent<RectTransform>();
        rt.anchoredPosition = Vector2.zero;
    }

    private void HidePanel()
    {
        var rt = gameObject.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(-1100, 0);
    }

    private void ShowValues(Configuration c)
    {
        _radius.text = c.arena.radius.ToString();
        _height.text = c.arena.height.ToString();

        _diameter.text = c.blobs.diameter_cm.ToString();
        _density.text = c.blobs.density.ToString();
        _lifetime.text = c.blobs.lifeTime_ms.ToString();
        _deadtime.text = c.blobs.deadTime_ms.ToString();
        _strobed.isOn = c.blobs.strobed;

        _motionMode.value = (int) c.blobs.movementMode;
        _coherence.text = c.blobs.coherence.ToString();
        _horzStdDev.text = c.blobs.horizontalStdDev.ToString();
        _vertStdDev.text = c.blobs.verticalStdDev.ToString();
        _noiseUnits.value = (int) c.blobs.noiseUnits;
        _addVChair.isOn = c.blobs.addChairVelocity;
        _identicalNoise.isOn = c.blobs.identicalNoise;

        _feedback.value = (int)c.controller.feedback;
        _chairAddress.text = c.controller.chairAddress;
        _camOffset.text = c.controller.cameraOffset.ToString();
        _eyeTracking.isOn = c.controller.eyeTracking;

        _showFrameRate.isOn = c.debug.showFrameRate;
        _createLog.isOn = c.debug.createLog;
        _windowOnly.isOn = c.debug.windowOnly;

        ShowValidControls();
    }

    private void ShowValidControls()
    {
        bool showBrownian = _motionMode.value == 0;
        _coherence.gameObject.SetActive(showBrownian);
        _horzStdDev.gameObject.SetActive(!showBrownian);
        _vertStdDev.gameObject.SetActive(!showBrownian);
        _noiseUnits.gameObject.SetActive(!showBrownian);
        _addVChair.gameObject.SetActive(!showBrownian);
        _identicalNoise.gameObject.SetActive(!showBrownian);
    }

    private void ScanConfig()
    {
        _config.arena.radius = float.Parse(_radius.text);
        _config.arena.height = float.Parse(_height.text);

        _config.blobs.diameter_cm = float.Parse(_diameter.text);
        _config.blobs.density = float.Parse(_density.text);
        _config.blobs.lifeTime_ms = float.Parse(_lifetime.text);
        _config.blobs.deadTime_ms = float.Parse(_deadtime.text);
        _config.blobs.strobed = _strobed.isOn;

        _config.blobs.movementMode = (BlobProperties.MovementMode)_motionMode.value;
        _config.blobs.coherence = float.Parse(_coherence.text);
        _config.blobs.horizontalStdDev = float.Parse(_horzStdDev.text);
        _config.blobs.verticalStdDev = float.Parse(_vertStdDev.text);
        _config.blobs.noiseUnits = (BlobProperties.NoiseUnits)_noiseUnits.value;
        _config.blobs.addChairVelocity = _addVChair.isOn;
        _config.blobs.identicalNoise = _identicalNoise.isOn;

        _config.controller.feedback = (ControllerSettings.Feedback)_feedback.value;
        _config.controller.chairAddress = _chairAddress.text;
        _config.controller.cameraOffset = float.Parse(_camOffset.text);
        _config.controller.eyeTracking = _eyeTracking.isOn;

        _config.debug.showFrameRate = _showFrameRate.isOn;
        _config.debug.createLog = _createLog.isOn;
        _config.debug.windowOnly = _windowOnly.isOn;
    }

    public void ModeChanged()
    {
        ShowValidControls();
    }

    public void OKButton_Pressed()
    {
        ScanConfig();

        HidePanel();
        if (_onFinished != null)
        {
            _onFinished(true, _nameMenu.options[_nameMenu.value].text, _config);
        }
    }

    public void CancelButton_Pressed()
    {
        HidePanel();
        if (_onFinished != null)
        {
            _onFinished(false, null, null);
        }
    }

    public void DefaultsButton_Pressed()
    {
        ShowValues(new Configuration());
    }
}
