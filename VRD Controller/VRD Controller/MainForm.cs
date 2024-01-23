using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using JenksVR.VRD;
using KLib.Controls;
using KLib.Utilities;

namespace VRD_Controller
{
    public partial class MainForm : Form
    {
        private Configuration _config = new Configuration();
        private VRDControllerSettings _settings = new VRDControllerSettings();

        private bool _ignoreEvents = false;
        private string _configFolder;
        private string _settingsPath;

        public MainForm()
        {
            InitializeComponent();

            feedbackEnum.Clear();
            feedbackEnum.AddItem(ControllerSettings.Feedback.Chair, "Chair");
            feedbackEnum.AddItem(ControllerSettings.Feedback.HeadTracker, "Head tracker");
            feedbackEnum.AddItem(ControllerSettings.Feedback.Simulated, "Simulated");

            _configFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Jenks", "Random Dots VR");
            _settingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Jenks", "RandomDotsVR Controller.xml");

            vrAddressTextBox.SetContextMenu(ipcContextMenu);


            RestoreSettings();
        }


        private void MainForm_Shown(object sender, EventArgs e)
        {
            EnumerateConfigFiles();
            RestoreConfiguration();
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }

        private void SaveSettings()
        {
            _settings.X = Location.X;
            _settings.Y = Location.Y;
            FileIO.XmlSerialize(_settings, _settingsPath);
        }

        private void RestoreSettings()
        {
            if (File.Exists(_settingsPath))
            {
                _settings = FileIO.XmlDeserialize<VRDControllerSettings>(_settingsPath);
            }

            if (_settings.X > 0)
            {
                StartPosition = FormStartPosition.Manual;
                Location = new Point(_settings.X, _settings.Y);
            }
            vrAddressTextBox.Text = _settings.vrIPAddress;

            ShowConfiguration();
        }

        private void RestoreConfiguration()
        {
            if (File.Exists(_settings.lastConfigFile))
            {
                _config = FileIO.XmlDeserialize<Configuration>(_settings.lastConfigFile);
                SetConfigDropDownText(Path.GetFileNameWithoutExtension(_settings.lastConfigFile).Replace(".vrd", ""));
            }
            else
            {
                _settings.lastConfigFile = "";
            }

            ShowConfiguration();
        }

        private void SetConfigDropDownText(string text)
        {
            _ignoreEvents = true;
            configDropDown.Text = text;
            _ignoreEvents = false;
        }


        private void EnumerateConfigFiles()
        {
            if (!Directory.Exists(_configFolder))
            {
                Directory.CreateDirectory(_configFolder);
            }

            var flist = Directory.EnumerateFiles(_configFolder, "*.vrd.xml");
            configDropDown.Items.Clear();
            foreach (var f in flist) configDropDown.Items.Add(Path.GetFileNameWithoutExtension(f).Replace(".vrd", ""));
        }

        private void ShowConfiguration()
        {
            _ignoreEvents = true;

            radiusNumeric.FloatValue = _config.arena.radius;
            heightNumeric.FloatValue = _config.arena.height;

            diameterNumeric.FloatValue = _config.blobs.diameter_cm;
            densityNumeric.FloatValue = _config.blobs.density;
            lifetimeNumeric.FloatValue = _config.blobs.lifeTime_ms;
            coherenceNumeric.FloatValue = _config.blobs.coherence;

            feedbackEnum.SetEnumValue(_config.controller.feedback);
            eyeTrackingCheckbox.Checked = _config.controller.eyeTracking;

            _ignoreEvents = false;
        }

        private void radiusNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (!_ignoreEvents)
            {
                _config.arena.radius = radiusNumeric.FloatValue;
            }
        }

        private void heightNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (!_ignoreEvents)
            {
                _config.arena.height = heightNumeric.FloatValue;
            }
        }

        private void diameterNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (!_ignoreEvents)
            {
                _config.blobs.diameter_cm = diameterNumeric.FloatValue;
            }
        }

        private void densityNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (!_ignoreEvents)
            {
                _config.blobs.density = densityNumeric.FloatValue;
            }
        }

        private void lifetimeNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (!_ignoreEvents)
            {
                _config.blobs.lifeTime_ms = lifetimeNumeric.FloatValue;
            }
        }

        private void coherenceNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (!_ignoreEvents)
            {
                _config.blobs.coherence = coherenceNumeric.FloatValue;
            }
        }

        private void feedbackEnum_ValueChanged(object sender, EventArgs e)
        {
            if (!_ignoreEvents)
            {
                _config.controller.feedback = (ControllerSettings.Feedback)feedbackEnum.Value;
            }
        }

        private void eyeTrackingCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (!_ignoreEvents)
            {
                _config.controller.eyeTracking = eyeTrackingCheckbox.Checked;
            }
        }

        private void vrAddressTextBox_ValueChanged(object sender, EventArgs e)
        {
            if (!_ignoreEvents)
            {
                _settings.vrIPAddress = vrAddressTextBox.Text;
                SaveSettings();
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            SendToVR("Config", _config.ToProtoBuf());
            SendToVR("Start");
            startButton.Visible = false;
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            SendToVR("End");
            startButton.Visible = true;
        }

        private void configDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_ignoreEvents)
            {
                var filePath = Path.Combine(_configFolder, configDropDown.Text + ".vrd.xml");
                if (File.Exists(filePath))
                {
                    _settings.lastConfigFile = filePath;
                    SaveSettings();

                    _config = FileIO.XmlDeserialize<Configuration>(_settings.lastConfigFile);

                    ShowConfiguration();
                }
                else
                {
                    MessageBox.Show($"Error loading config file '{configDropDown.Text}'", "Random Dots", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void mmFileSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_settings.lastConfigFile))
            {
                mmFileSaveAs_Click(sender, e);
            }
            else
            {
                FileIO.XmlSerialize(_config, _settings.lastConfigFile);
            }
        }

        private void mmFileSaveAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();

            // Set filter options and filter index.
            dlg.Filter = "VRD XML Files (.vrd.xml)|*.vrd.xml|All Files (*.*)|*.*";
            dlg.FilterIndex = 1;
            dlg.OverwritePrompt = true;

            if (File.Exists(_settings.lastConfigFile))
            {
                FileInfo fi = new FileInfo(_settings.lastConfigFile);
                dlg.InitialDirectory = fi.DirectoryName;
                dlg.FileName = Path.GetFileName(_settings.lastConfigFile);
            }
            else
            {
                if (Directory.Exists(_configFolder))
                {
                    dlg.InitialDirectory = _configFolder;
                }
                dlg.FileName = "Untitled.vrd.xml";
            }

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (!dlg.FileName.EndsWith(".vrd.xml"))
                {
                    dlg.FileName += ".vrd.xml";
                }

                FileIO.XmlSerialize(_config, dlg.FileName);
                _settings.lastConfigFile = dlg.FileName;
                SaveSettings();

                if (Path.GetDirectoryName(dlg.FileName).Equals(_configFolder))
                {
                    EnumerateConfigFiles();
                    SetConfigDropDownText(Path.GetFileNameWithoutExtension(dlg.FileName).Replace(".vrd", ""));
                }
            }
        }

        private void mmFileExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cmPing_Click(object sender, EventArgs e)
        {
            var result = SendToVR("Ping");

            if (result)
            { 
                MsgBox.Show("Ping succeeded!", "Random Dots", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private bool SendToVR(string message)
        {
            return SendToVR(message, null);
        }

        private bool SendToVR(string message, byte[] data)
        {
            return SendMessageToVR(_settings.vrIPAddress, message, data);
        }

        private bool SendMessageToVR(string address, string message, byte[] data)
        {
            bool result = false;

            try
            {
                byte[] inStream = new byte[100];

                using (TcpClient mySocket = new TcpClient(address, 5150))
                using (NetworkStream theStream = mySocket.GetStream())

                using (BinaryWriter theWriter = new BinaryWriter(theStream))
                using (StreamReader theReader = new StreamReader(theStream))
                {
                    theStream.ReadTimeout = 2000;
                    mySocket.ReceiveTimeout = 2000;
                    theWriter.Write(message);

                    if (data != null)
                    {
                        theWriter.Write(data.Length);
                        theWriter.Write(data);
                    }

                    theWriter.Flush();

                    //int nread = theStream.Read(inStream, 0, inStream.Length);
                    //result = nread == 1 && (UnityStatus)inStream[0] == UnityStatus.Acknowledged;

                    mySocket.Close();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                MsgBox.Show($"Error sending '{message}':{Environment.NewLine}{ex.Message}", "Random Dots", MessageBoxButtons.OK, MessageBoxIcon.Error);
                result = false;
            }

            return result;
        }

    }
}
