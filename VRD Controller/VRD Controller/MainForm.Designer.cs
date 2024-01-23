namespace VRD_Controller
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.eyeTrackingCheckbox = new System.Windows.Forms.CheckBox();
            this.configDropDown = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mmFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.mmFileSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mmFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.label10 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.ipcContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmPing = new System.Windows.Forms.ToolStripMenuItem();
            this.coherenceNumeric = new KLib.Controls.KNumericBox();
            this.feedbackEnum = new KLib.Controls.EnumDropDown();
            this.densityNumeric = new KLib.Controls.KNumericBox();
            this.diameterNumeric = new KLib.Controls.KNumericBox();
            this.lifetimeNumeric = new KLib.Controls.KNumericBox();
            this.radiusNumeric = new KLib.Controls.KNumericBox();
            this.vrAddressTextBox = new KLib.Controls.KTextBox();
            this.heightNumeric = new KLib.Controls.KNumericBox();
            this.mainMenu.SuspendLayout();
            this.ipcContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(87, 152);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Height (m)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(87, 126);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Radius (m)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(55, 274);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Lifetime (ms)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(55, 221);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Diameter (cm)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(55, 248);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Density (dots/m2)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(392, 211);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "VR Address";
            // 
            // eyeTrackingCheckbox
            // 
            this.eyeTrackingCheckbox.AutoSize = true;
            this.eyeTrackingCheckbox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.eyeTrackingCheckbox.Location = new System.Drawing.Point(324, 152);
            this.eyeTrackingCheckbox.Name = "eyeTrackingCheckbox";
            this.eyeTrackingCheckbox.Size = new System.Drawing.Size(85, 17);
            this.eyeTrackingCheckbox.TabIndex = 17;
            this.eyeTrackingCheckbox.Text = "Eye tracking";
            this.eyeTrackingCheckbox.UseVisualStyleBackColor = true;
            this.eyeTrackingCheckbox.CheckedChanged += new System.EventHandler(this.eyeTrackingCheckbox_CheckedChanged);
            // 
            // configDropDown
            // 
            this.configDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.configDropDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.configDropDown.FormattingEnabled = true;
            this.configDropDown.Location = new System.Drawing.Point(191, 47);
            this.configDropDown.Name = "configDropDown";
            this.configDropDown.Size = new System.Drawing.Size(215, 28);
            this.configDropDown.TabIndex = 18;
            this.configDropDown.SelectedIndexChanged += new System.EventHandler(this.configDropDown_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(194, 31);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(69, 13);
            this.label8.TabIndex = 19;
            this.label8.Text = "Configuration";
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(572, 24);
            this.mainMenu.TabIndex = 21;
            this.mainMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmFileSave,
            this.mmFileSaveAs,
            this.toolStripSeparator1,
            this.mmFileExit});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // mmFileSave
            // 
            this.mmFileSave.Name = "mmFileSave";
            this.mmFileSave.Size = new System.Drawing.Size(187, 22);
            this.mmFileSave.Text = "&Save configuration";
            this.mmFileSave.Click += new System.EventHandler(this.mmFileSave_Click);
            // 
            // mmFileSaveAs
            // 
            this.mmFileSaveAs.Name = "mmFileSaveAs";
            this.mmFileSaveAs.Size = new System.Drawing.Size(187, 22);
            this.mmFileSaveAs.Text = "Save configuration &as";
            this.mmFileSaveAs.Click += new System.EventHandler(this.mmFileSaveAs_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(184, 6);
            // 
            // mmFileExit
            // 
            this.mmFileExit.Name = "mmFileExit";
            this.mmFileExit.Size = new System.Drawing.Size(187, 22);
            this.mmFileExit.Text = "E&xit";
            this.mmFileExit.Click += new System.EventHandler(this.mmFileExit_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(301, 130);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(92, 13);
            this.label10.TabIndex = 22;
            this.label10.Text = "Position feedback";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(168, 99);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(57, 20);
            this.label13.TabIndex = 29;
            this.label13.Text = "Arena";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(173, 194);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(47, 20);
            this.label14.TabIndex = 30;
            this.label14.Text = "Dots";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(423, 100);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(67, 20);
            this.label15.TabIndex = 31;
            this.label15.Text = "Control";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(55, 301);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(83, 13);
            this.label17.TabIndex = 35;
            this.label17.Text = "Coherence (0-1)";
            // 
            // startButton
            // 
            this.startButton.Image = global::VRD_Controller.Properties.Resources.media_play_green;
            this.startButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.startButton.Location = new System.Drawing.Point(395, 270);
            this.startButton.Name = "startButton";
            this.startButton.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.startButton.Size = new System.Drawing.Size(122, 39);
            this.startButton.TabIndex = 15;
            this.startButton.TabStop = false;
            this.startButton.Text = "Start";
            this.startButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Image = global::VRD_Controller.Properties.Resources.Stop;
            this.stopButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.stopButton.Location = new System.Drawing.Point(395, 270);
            this.stopButton.Name = "stopButton";
            this.stopButton.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.stopButton.Size = new System.Drawing.Size(122, 39);
            this.stopButton.TabIndex = 16;
            this.stopButton.TabStop = false;
            this.stopButton.Text = "Stop";
            this.stopButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // ipcContextMenu
            // 
            this.ipcContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmPing});
            this.ipcContextMenu.Name = "ipcContextMenu";
            this.ipcContextMenu.Size = new System.Drawing.Size(99, 26);
            // 
            // cmPing
            // 
            this.cmPing.Name = "cmPing";
            this.cmPing.Size = new System.Drawing.Size(98, 22);
            this.cmPing.Text = "Ping";
            this.cmPing.Click += new System.EventHandler(this.cmPing_Click);
            // 
            // coherenceNumeric
            // 
            this.coherenceNumeric.AllowInf = false;
            this.coherenceNumeric.AutoSize = true;
            this.coherenceNumeric.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.coherenceNumeric.ClearOnDisable = false;
            this.coherenceNumeric.FloatValue = 0F;
            this.coherenceNumeric.IntValue = 0;
            this.coherenceNumeric.IsInteger = false;
            this.coherenceNumeric.Location = new System.Drawing.Point(146, 296);
            this.coherenceNumeric.MaxCoerce = true;
            this.coherenceNumeric.MaximumSize = new System.Drawing.Size(20000, 20);
            this.coherenceNumeric.MaxValue = 1D;
            this.coherenceNumeric.MinCoerce = true;
            this.coherenceNumeric.MinimumSize = new System.Drawing.Size(10, 20);
            this.coherenceNumeric.MinValue = 0D;
            this.coherenceNumeric.Name = "coherenceNumeric";
            this.coherenceNumeric.Size = new System.Drawing.Size(100, 20);
            this.coherenceNumeric.TabIndex = 34;
            this.coherenceNumeric.TextFormat = "K4";
            this.coherenceNumeric.ToolTip = "";
            this.coherenceNumeric.Units = "";
            this.coherenceNumeric.Value = 0D;
            this.coherenceNumeric.ValueChanged += new System.EventHandler(this.coherenceNumeric_ValueChanged);
            // 
            // feedbackEnum
            // 
            this.feedbackEnum.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.feedbackEnum.FormattingEnabled = true;
            this.feedbackEnum.Location = new System.Drawing.Point(396, 126);
            this.feedbackEnum.Name = "feedbackEnum";
            this.feedbackEnum.Size = new System.Drawing.Size(121, 21);
            this.feedbackEnum.Sort = false;
            this.feedbackEnum.TabIndex = 23;
            this.feedbackEnum.ValueChanged += new System.EventHandler(this.feedbackEnum_ValueChanged);
            // 
            // densityNumeric
            // 
            this.densityNumeric.AllowInf = false;
            this.densityNumeric.AutoSize = true;
            this.densityNumeric.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.densityNumeric.ClearOnDisable = false;
            this.densityNumeric.FloatValue = 0F;
            this.densityNumeric.IntValue = 0;
            this.densityNumeric.IsInteger = false;
            this.densityNumeric.Location = new System.Drawing.Point(146, 244);
            this.densityNumeric.MaxCoerce = false;
            this.densityNumeric.MaximumSize = new System.Drawing.Size(20000, 20);
            this.densityNumeric.MaxValue = 1.7976931348623157E+308D;
            this.densityNumeric.MinCoerce = true;
            this.densityNumeric.MinimumSize = new System.Drawing.Size(10, 20);
            this.densityNumeric.MinValue = 0D;
            this.densityNumeric.Name = "densityNumeric";
            this.densityNumeric.Size = new System.Drawing.Size(100, 20);
            this.densityNumeric.TabIndex = 11;
            this.densityNumeric.TextFormat = "K4";
            this.densityNumeric.ToolTip = "";
            this.densityNumeric.Units = "";
            this.densityNumeric.Value = 0D;
            this.densityNumeric.ValueChanged += new System.EventHandler(this.densityNumeric_ValueChanged);
            // 
            // diameterNumeric
            // 
            this.diameterNumeric.AllowInf = false;
            this.diameterNumeric.AutoSize = true;
            this.diameterNumeric.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.diameterNumeric.ClearOnDisable = false;
            this.diameterNumeric.FloatValue = 0F;
            this.diameterNumeric.IntValue = 0;
            this.diameterNumeric.IsInteger = false;
            this.diameterNumeric.Location = new System.Drawing.Point(146, 217);
            this.diameterNumeric.MaxCoerce = false;
            this.diameterNumeric.MaximumSize = new System.Drawing.Size(20000, 20);
            this.diameterNumeric.MaxValue = 1.7976931348623157E+308D;
            this.diameterNumeric.MinCoerce = true;
            this.diameterNumeric.MinimumSize = new System.Drawing.Size(10, 20);
            this.diameterNumeric.MinValue = 0D;
            this.diameterNumeric.Name = "diameterNumeric";
            this.diameterNumeric.Size = new System.Drawing.Size(100, 20);
            this.diameterNumeric.TabIndex = 7;
            this.diameterNumeric.TextFormat = "K4";
            this.diameterNumeric.ToolTip = "";
            this.diameterNumeric.Units = "";
            this.diameterNumeric.Value = 0D;
            this.diameterNumeric.ValueChanged += new System.EventHandler(this.diameterNumeric_ValueChanged);
            // 
            // lifetimeNumeric
            // 
            this.lifetimeNumeric.AllowInf = false;
            this.lifetimeNumeric.AutoSize = true;
            this.lifetimeNumeric.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.lifetimeNumeric.ClearOnDisable = false;
            this.lifetimeNumeric.FloatValue = 0F;
            this.lifetimeNumeric.IntValue = 0;
            this.lifetimeNumeric.IsInteger = false;
            this.lifetimeNumeric.Location = new System.Drawing.Point(146, 270);
            this.lifetimeNumeric.MaxCoerce = false;
            this.lifetimeNumeric.MaximumSize = new System.Drawing.Size(20000, 20);
            this.lifetimeNumeric.MaxValue = 1.7976931348623157E+308D;
            this.lifetimeNumeric.MinCoerce = true;
            this.lifetimeNumeric.MinimumSize = new System.Drawing.Size(10, 20);
            this.lifetimeNumeric.MinValue = 0D;
            this.lifetimeNumeric.Name = "lifetimeNumeric";
            this.lifetimeNumeric.Size = new System.Drawing.Size(100, 20);
            this.lifetimeNumeric.TabIndex = 5;
            this.lifetimeNumeric.TextFormat = "K4";
            this.lifetimeNumeric.ToolTip = "";
            this.lifetimeNumeric.Units = "";
            this.lifetimeNumeric.Value = 0D;
            this.lifetimeNumeric.ValueChanged += new System.EventHandler(this.lifetimeNumeric_ValueChanged);
            // 
            // radiusNumeric
            // 
            this.radiusNumeric.AllowInf = false;
            this.radiusNumeric.AutoSize = true;
            this.radiusNumeric.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.radiusNumeric.ClearOnDisable = false;
            this.radiusNumeric.FloatValue = 0F;
            this.radiusNumeric.IntValue = 0;
            this.radiusNumeric.IsInteger = false;
            this.radiusNumeric.Location = new System.Drawing.Point(146, 122);
            this.radiusNumeric.MaxCoerce = false;
            this.radiusNumeric.MaximumSize = new System.Drawing.Size(20000, 20);
            this.radiusNumeric.MaxValue = 1.7976931348623157E+308D;
            this.radiusNumeric.MinCoerce = true;
            this.radiusNumeric.MinimumSize = new System.Drawing.Size(10, 20);
            this.radiusNumeric.MinValue = 0D;
            this.radiusNumeric.Name = "radiusNumeric";
            this.radiusNumeric.Size = new System.Drawing.Size(100, 20);
            this.radiusNumeric.TabIndex = 3;
            this.radiusNumeric.TextFormat = "K4";
            this.radiusNumeric.ToolTip = "";
            this.radiusNumeric.Units = "";
            this.radiusNumeric.Value = 0D;
            this.radiusNumeric.ValueChanged += new System.EventHandler(this.radiusNumeric_ValueChanged);
            // 
            // vrAddressTextBox
            // 
            this.vrAddressTextBox.AutoSize = true;
            this.vrAddressTextBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.vrAddressTextBox.FontColor = System.Drawing.SystemColors.WindowText;
            this.vrAddressTextBox.IsIPAddress = false;
            this.vrAddressTextBox.Location = new System.Drawing.Point(395, 227);
            this.vrAddressTextBox.Name = "vrAddressTextBox";
            this.vrAddressTextBox.Size = new System.Drawing.Size(122, 20);
            this.vrAddressTextBox.TabIndex = 1;
            this.vrAddressTextBox.ValueChanged += new System.EventHandler(this.vrAddressTextBox_ValueChanged);
            // 
            // heightNumeric
            // 
            this.heightNumeric.AllowInf = false;
            this.heightNumeric.AutoSize = true;
            this.heightNumeric.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.heightNumeric.ClearOnDisable = false;
            this.heightNumeric.FloatValue = 0F;
            this.heightNumeric.IntValue = 0;
            this.heightNumeric.IsInteger = false;
            this.heightNumeric.Location = new System.Drawing.Point(146, 148);
            this.heightNumeric.MaxCoerce = false;
            this.heightNumeric.MaximumSize = new System.Drawing.Size(20000, 20);
            this.heightNumeric.MaxValue = 1.7976931348623157E+308D;
            this.heightNumeric.MinCoerce = true;
            this.heightNumeric.MinimumSize = new System.Drawing.Size(10, 20);
            this.heightNumeric.MinValue = 0D;
            this.heightNumeric.Name = "heightNumeric";
            this.heightNumeric.Size = new System.Drawing.Size(100, 20);
            this.heightNumeric.TabIndex = 0;
            this.heightNumeric.TextFormat = "K4";
            this.heightNumeric.ToolTip = "";
            this.heightNumeric.Units = "";
            this.heightNumeric.Value = 0D;
            this.heightNumeric.ValueChanged += new System.EventHandler(this.heightNumeric_ValueChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 358);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.coherenceNumeric);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.feedbackEnum);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.configDropDown);
            this.Controls.Add(this.eyeTrackingCheckbox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.densityNumeric);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.diameterNumeric);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lifetimeNumeric);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.radiusNumeric);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.vrAddressTextBox);
            this.Controls.Add(this.heightNumeric);
            this.Controls.Add(this.mainMenu);
            this.Controls.Add(this.stopButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenu;
            this.Name = "MainForm";
            this.Text = "Random Dots Controller";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.ipcContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private KLib.Controls.KNumericBox heightNumeric;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private KLib.Controls.KNumericBox radiusNumeric;
        private System.Windows.Forms.Label label3;
        private KLib.Controls.KNumericBox lifetimeNumeric;
        private System.Windows.Forms.Label label4;
        private KLib.Controls.KNumericBox diameterNumeric;
        private System.Windows.Forms.Label label6;
        private KLib.Controls.KNumericBox densityNumeric;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.CheckBox eyeTrackingCheckbox;
        private System.Windows.Forms.ComboBox configDropDown;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.Label label10;
        private KLib.Controls.EnumDropDown feedbackEnum;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label17;
        private KLib.Controls.KNumericBox coherenceNumeric;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mmFileSave;
        private System.Windows.Forms.ToolStripMenuItem mmFileSaveAs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mmFileExit;
        private System.Windows.Forms.ContextMenuStrip ipcContextMenu;
        private System.Windows.Forms.ToolStripMenuItem cmPing;
        private KLib.Controls.KTextBox vrAddressTextBox;
    }
}

