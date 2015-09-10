namespace GlamPlayer
{
    partial class GlamWindow
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
            this.textMusicPath = new System.Windows.Forms.TextBox();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.panelControl = new System.Windows.Forms.Panel();
            this.listOfMusicTracks = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelPlayer = new System.Windows.Forms.Panel();
            this.buttonAddTrack = new System.Windows.Forms.Button();
            this.buttonToggleConrol = new System.Windows.Forms.Button();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.buttonSwitch = new System.Windows.Forms.Button();
            this.textSearchField = new System.Windows.Forms.TextBox();
            this.GlamFrame = new GlamPlayer.GlamFrame();
            this.labelSearchField = new System.Windows.Forms.Label();
            this.labelAddUrl = new System.Windows.Forms.Label();
            this.panelControl.SuspendLayout();
            this.panelPlayer.SuspendLayout();
            this.SuspendLayout();
            // 
            // textMusicPath
            // 
            this.textMusicPath.BackColor = System.Drawing.Color.DimGray;
            this.textMusicPath.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textMusicPath.ForeColor = System.Drawing.Color.Black;
            this.textMusicPath.Location = new System.Drawing.Point(53, 28);
            this.textMusicPath.Name = "textMusicPath";
            this.textMusicPath.Size = new System.Drawing.Size(185, 13);
            this.textMusicPath.TabIndex = 1;
            // 
            // buttonAdd
            // 
            this.buttonAdd.BackColor = System.Drawing.Color.WhiteSmoke;
            this.buttonAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAdd.Location = new System.Drawing.Point(241, 25);
            this.buttonAdd.Margin = new System.Windows.Forms.Padding(0);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(47, 19);
            this.buttonAdd.TabIndex = 2;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = false;
            this.buttonAdd.Click += new System.EventHandler(this.OnAddButtonClick);
            // 
            // panelControl
            // 
            this.panelControl.BackColor = System.Drawing.Color.Black;
            this.panelControl.Controls.Add(this.labelAddUrl);
            this.panelControl.Controls.Add(this.labelSearchField);
            this.panelControl.Controls.Add(this.textSearchField);
            this.panelControl.Controls.Add(this.listOfMusicTracks);
            this.panelControl.Controls.Add(this.textMusicPath);
            this.panelControl.Controls.Add(this.buttonAdd);
            this.panelControl.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelControl.Location = new System.Drawing.Point(331, 0);
            this.panelControl.Name = "panelControl";
            this.panelControl.Size = new System.Drawing.Size(300, 335);
            this.panelControl.TabIndex = 6;
            // 
            // listOfMusicTracks
            // 
            this.listOfMusicTracks.BackColor = System.Drawing.Color.DimGray;
            this.listOfMusicTracks.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listOfMusicTracks.ForeColor = System.Drawing.SystemColors.Window;
            this.listOfMusicTracks.HideSelection = false;
            this.listOfMusicTracks.Location = new System.Drawing.Point(7, 47);
            this.listOfMusicTracks.Multiline = true;
            this.listOfMusicTracks.Name = "listOfMusicTracks";
            this.listOfMusicTracks.ReadOnly = true;
            this.listOfMusicTracks.Size = new System.Drawing.Size(281, 276);
            this.listOfMusicTracks.TabIndex = 3;
            this.listOfMusicTracks.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 335);
            this.panel1.MaximumSize = new System.Drawing.Size(418, 35);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(331, 0);
            this.panel1.TabIndex = 7;
            // 
            // panelPlayer
            // 
            this.panelPlayer.Controls.Add(this.buttonAddTrack);
            this.panelPlayer.Controls.Add(this.buttonToggleConrol);
            this.panelPlayer.Controls.Add(this.buttonRefresh);
            this.panelPlayer.Controls.Add(this.buttonSwitch);
            this.panelPlayer.Controls.Add(this.panel1);
            this.panelPlayer.Controls.Add(this.GlamFrame);
            this.panelPlayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPlayer.Location = new System.Drawing.Point(0, 0);
            this.panelPlayer.MaximumSize = new System.Drawing.Size(1024, 0);
            this.panelPlayer.Name = "panelPlayer";
            this.panelPlayer.Size = new System.Drawing.Size(331, 335);
            this.panelPlayer.TabIndex = 7;
            // 
            // buttonAddTrack
            // 
            this.buttonAddTrack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddTrack.BackColor = System.Drawing.Color.White;
            this.buttonAddTrack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonAddTrack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAddTrack.Location = new System.Drawing.Point(293, 84);
            this.buttonAddTrack.Name = "buttonAddTrack";
            this.buttonAddTrack.Size = new System.Drawing.Size(32, 32);
            this.buttonAddTrack.TabIndex = 4;
            this.buttonAddTrack.UseVisualStyleBackColor = false;
            this.buttonAddTrack.Click += new System.EventHandler(this.OnAddTrackButtonClick);
            // 
            // buttonToggleConrol
            // 
            this.buttonToggleConrol.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonToggleConrol.BackgroundImage = global::GlamPlayer.Properties.Resources.Universal_Multi_Tool;
            this.buttonToggleConrol.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonToggleConrol.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonToggleConrol.Location = new System.Drawing.Point(293, 46);
            this.buttonToggleConrol.Name = "buttonToggleConrol";
            this.buttonToggleConrol.Size = new System.Drawing.Size(32, 32);
            this.buttonToggleConrol.TabIndex = 6;
            this.buttonToggleConrol.UseVisualStyleBackColor = true;
            this.buttonToggleConrol.Click += new System.EventHandler(this.OnControlButtonClick);
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRefresh.BackgroundImage = global::GlamPlayer.Properties.Resources.Mimic;
            this.buttonRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRefresh.Location = new System.Drawing.Point(293, 122);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(32, 32);
            this.buttonRefresh.TabIndex = 4;
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.OnRefreshButtonClick);
            // 
            // buttonSwitch
            // 
            this.buttonSwitch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSwitch.BackgroundImage = global::GlamPlayer.Properties.Resources.Stealth;
            this.buttonSwitch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonSwitch.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.buttonSwitch.FlatAppearance.BorderSize = 0;
            this.buttonSwitch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSwitch.Location = new System.Drawing.Point(293, 8);
            this.buttonSwitch.Name = "buttonSwitch";
            this.buttonSwitch.Size = new System.Drawing.Size(32, 32);
            this.buttonSwitch.TabIndex = 0;
            this.buttonSwitch.UseVisualStyleBackColor = true;
            this.buttonSwitch.Click += new System.EventHandler(this.OnTransparencyButtonClick);
            // 
            // textSearchField
            // 
            this.textSearchField.BackColor = System.Drawing.Color.DimGray;
            this.textSearchField.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textSearchField.ForeColor = System.Drawing.Color.Black;
            this.textSearchField.Location = new System.Drawing.Point(53, 9);
            this.textSearchField.Name = "textSearchField";
            this.textSearchField.Size = new System.Drawing.Size(235, 13);
            this.textSearchField.TabIndex = 4;
            // 
            // GlamFrame
            // 
            this.GlamFrame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GlamFrame.Location = new System.Drawing.Point(0, 0);
            this.GlamFrame.MinimumSize = new System.Drawing.Size(20, 20);
            this.GlamFrame.Name = "GlamFrame";
            this.GlamFrame.ScrollBarsEnabled = false;
            this.GlamFrame.Size = new System.Drawing.Size(331, 335);
            this.GlamFrame.TabIndex = 5;
            this.GlamFrame.VolumeFadeEnabled = false;
            this.GlamFrame.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.OnBrowserPanelLoadingComplete);
            // 
            // labelSearchField
            // 
            this.labelSearchField.BackColor = System.Drawing.Color.DimGray;
            this.labelSearchField.Location = new System.Drawing.Point(6, 9);
            this.labelSearchField.Name = "labelSearchField";
            this.labelSearchField.Size = new System.Drawing.Size(41, 13);
            this.labelSearchField.TabIndex = 5;
            this.labelSearchField.Text = "Search";
            // 
            // labelAddUrl
            // 
            this.labelAddUrl.BackColor = System.Drawing.Color.DimGray;
            this.labelAddUrl.Location = new System.Drawing.Point(6, 28);
            this.labelAddUrl.Name = "labelAddUrl";
            this.labelAddUrl.Size = new System.Drawing.Size(41, 13);
            this.labelAddUrl.TabIndex = 6;
            this.labelAddUrl.Text = "URL";
            // 
            // GlamWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(631, 335);
            this.Controls.Add(this.panelPlayer);
            this.Controls.Add(this.panelControl);
            this.Name = "GlamWindow";
            this.Text = "Glam Player";
            this.panelControl.ResumeLayout(false);
            this.panelControl.PerformLayout();
            this.panelPlayer.ResumeLayout(false);
            this.panelPlayer.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textMusicPath;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.Panel panelControl;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonSwitch;
        private System.Windows.Forms.Button buttonToggleConrol;
        private System.Windows.Forms.Panel panelPlayer;
        private System.Windows.Forms.TextBox listOfMusicTracks;
        private GlamFrame GlamFrame;
        private System.Windows.Forms.Button buttonAddTrack;
        private System.Windows.Forms.TextBox textSearchField;
        private System.Windows.Forms.Label labelSearchField;
        private System.Windows.Forms.Label labelAddUrl;




    }
}

