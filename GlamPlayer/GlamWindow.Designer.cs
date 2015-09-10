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
            this.textboxTrackUrl = new System.Windows.Forms.TextBox();
            this.buttonAddTrackUrl = new System.Windows.Forms.Button();
            this.panelControl = new System.Windows.Forms.Panel();
            this.dedicatedMapPlaylist = new System.Windows.Forms.GroupBox();
            this.listOfMusicTracks = new System.Windows.Forms.TextBox();
            this.groupPanelDedicatedPlaylist = new System.Windows.Forms.GroupBox();
            this.buttonRemoveCurrentTrack = new System.Windows.Forms.Button();
            this.buttonGetCurrentTrackUrl = new System.Windows.Forms.Button();
            this.labelAddUrl = new System.Windows.Forms.Label();
            this.groupPanelManualPlaylist = new System.Windows.Forms.GroupBox();
            this.checkBoxAutoUsePlaylistId = new System.Windows.Forms.CheckBox();
            this.buttonLoadPlaylistById = new System.Windows.Forms.Button();
            this.textboxPlaylistId = new System.Windows.Forms.TextBox();
            this.labelManualPlaylistId = new System.Windows.Forms.Label();
            this.groupPanelSearchlist = new System.Windows.Forms.GroupBox();
            this.checkBoxAutoUseSearchTerms = new System.Windows.Forms.CheckBox();
            this.buttonSetSearchPlaylist = new System.Windows.Forms.Button();
            this.labelSearchField = new System.Windows.Forms.Label();
            this.textboxSearchPlaylistField = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelPlayer = new System.Windows.Forms.Panel();
            this.buttonToggleConrol = new System.Windows.Forms.Button();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.buttonSwitch = new System.Windows.Forms.Button();
            this.GlamFrame = new GlamPlayer.GlamFrame();
            this.panelControl.SuspendLayout();
            this.dedicatedMapPlaylist.SuspendLayout();
            this.groupPanelDedicatedPlaylist.SuspendLayout();
            this.groupPanelManualPlaylist.SuspendLayout();
            this.groupPanelSearchlist.SuspendLayout();
            this.panelPlayer.SuspendLayout();
            this.SuspendLayout();
            // 
            // textboxTrackUrl
            // 
            this.textboxTrackUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textboxTrackUrl.BackColor = System.Drawing.Color.DimGray;
            this.textboxTrackUrl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textboxTrackUrl.ForeColor = System.Drawing.Color.Black;
            this.textboxTrackUrl.Location = new System.Drawing.Point(89, 16);
            this.textboxTrackUrl.Name = "textboxTrackUrl";
            this.textboxTrackUrl.Size = new System.Drawing.Size(211, 20);
            this.textboxTrackUrl.TabIndex = 1;
            // 
            // buttonAddTrackUrl
            // 
            this.buttonAddTrackUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddTrackUrl.BackColor = System.Drawing.Color.Gray;
            this.buttonAddTrackUrl.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAddTrackUrl.Location = new System.Drawing.Point(89, 39);
            this.buttonAddTrackUrl.Margin = new System.Windows.Forms.Padding(0);
            this.buttonAddTrackUrl.Name = "buttonAddTrackUrl";
            this.buttonAddTrackUrl.Size = new System.Drawing.Size(210, 25);
            this.buttonAddTrackUrl.TabIndex = 2;
            this.buttonAddTrackUrl.Text = "Add specified track URL to map playlist";
            this.buttonAddTrackUrl.UseVisualStyleBackColor = false;
            this.buttonAddTrackUrl.Click += new System.EventHandler(this.OnAddNewTrackButtonClick);
            // 
            // panelControl
            // 
            this.panelControl.BackColor = System.Drawing.Color.Black;
            this.panelControl.Controls.Add(this.dedicatedMapPlaylist);
            this.panelControl.Controls.Add(this.groupPanelDedicatedPlaylist);
            this.panelControl.Controls.Add(this.groupPanelManualPlaylist);
            this.panelControl.Controls.Add(this.groupPanelSearchlist);
            this.panelControl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl.Location = new System.Drawing.Point(0, 161);
            this.panelControl.Name = "panelControl";
            this.panelControl.Size = new System.Drawing.Size(318, 519);
            this.panelControl.TabIndex = 6;
            // 
            // dedicatedMapPlaylist
            // 
            this.dedicatedMapPlaylist.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dedicatedMapPlaylist.Controls.Add(this.listOfMusicTracks);
            this.dedicatedMapPlaylist.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dedicatedMapPlaylist.ForeColor = System.Drawing.Color.White;
            this.dedicatedMapPlaylist.Location = new System.Drawing.Point(6, 6);
            this.dedicatedMapPlaylist.Name = "dedicatedMapPlaylist";
            this.dedicatedMapPlaylist.Size = new System.Drawing.Size(306, 177);
            this.dedicatedMapPlaylist.TabIndex = 10;
            this.dedicatedMapPlaylist.TabStop = false;
            this.dedicatedMapPlaylist.Text = "TRACKS IN DEDICATED MAP PLAYLIST";
            // 
            // listOfMusicTracks
            // 
            this.listOfMusicTracks.BackColor = System.Drawing.Color.Black;
            this.listOfMusicTracks.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listOfMusicTracks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listOfMusicTracks.ForeColor = System.Drawing.SystemColors.Window;
            this.listOfMusicTracks.HideSelection = false;
            this.listOfMusicTracks.Location = new System.Drawing.Point(3, 16);
            this.listOfMusicTracks.Multiline = true;
            this.listOfMusicTracks.Name = "listOfMusicTracks";
            this.listOfMusicTracks.ReadOnly = true;
            this.listOfMusicTracks.Size = new System.Drawing.Size(300, 158);
            this.listOfMusicTracks.TabIndex = 3;
            this.listOfMusicTracks.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupPanelDedicatedPlaylist
            // 
            this.groupPanelDedicatedPlaylist.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupPanelDedicatedPlaylist.Controls.Add(this.buttonRemoveCurrentTrack);
            this.groupPanelDedicatedPlaylist.Controls.Add(this.buttonGetCurrentTrackUrl);
            this.groupPanelDedicatedPlaylist.Controls.Add(this.labelAddUrl);
            this.groupPanelDedicatedPlaylist.Controls.Add(this.textboxTrackUrl);
            this.groupPanelDedicatedPlaylist.Controls.Add(this.buttonAddTrackUrl);
            this.groupPanelDedicatedPlaylist.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupPanelDedicatedPlaylist.ForeColor = System.Drawing.Color.White;
            this.groupPanelDedicatedPlaylist.Location = new System.Drawing.Point(6, 189);
            this.groupPanelDedicatedPlaylist.Name = "groupPanelDedicatedPlaylist";
            this.groupPanelDedicatedPlaylist.Size = new System.Drawing.Size(306, 121);
            this.groupPanelDedicatedPlaylist.TabIndex = 9;
            this.groupPanelDedicatedPlaylist.TabStop = false;
            this.groupPanelDedicatedPlaylist.Text = "MODIFY MAP PLAYLIST";
            // 
            // buttonRemoveCurrentTrack
            // 
            this.buttonRemoveCurrentTrack.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRemoveCurrentTrack.BackColor = System.Drawing.Color.Gray;
            this.buttonRemoveCurrentTrack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRemoveCurrentTrack.Location = new System.Drawing.Point(89, 93);
            this.buttonRemoveCurrentTrack.Name = "buttonRemoveCurrentTrack";
            this.buttonRemoveCurrentTrack.Size = new System.Drawing.Size(210, 23);
            this.buttonRemoveCurrentTrack.TabIndex = 8;
            this.buttonRemoveCurrentTrack.Text = "Remove current track from map playlist";
            this.buttonRemoveCurrentTrack.UseVisualStyleBackColor = false;
            this.buttonRemoveCurrentTrack.Click += new System.EventHandler(this.OnRemoveCurrentTrackButtonClick);
            // 
            // buttonGetCurrentTrackUrl
            // 
            this.buttonGetCurrentTrackUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGetCurrentTrackUrl.BackColor = System.Drawing.Color.Gray;
            this.buttonGetCurrentTrackUrl.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonGetCurrentTrackUrl.Location = new System.Drawing.Point(89, 67);
            this.buttonGetCurrentTrackUrl.Name = "buttonGetCurrentTrackUrl";
            this.buttonGetCurrentTrackUrl.Size = new System.Drawing.Size(210, 23);
            this.buttonGetCurrentTrackUrl.TabIndex = 7;
            this.buttonGetCurrentTrackUrl.Text = "Get URL of Current Track";
            this.buttonGetCurrentTrackUrl.UseVisualStyleBackColor = false;
            this.buttonGetCurrentTrackUrl.Click += new System.EventHandler(this.OnGetCurrentTrackUrlButtonClick);
            // 
            // labelAddUrl
            // 
            this.labelAddUrl.BackColor = System.Drawing.Color.Transparent;
            this.labelAddUrl.Location = new System.Drawing.Point(18, 16);
            this.labelAddUrl.Margin = new System.Windows.Forms.Padding(0);
            this.labelAddUrl.Name = "labelAddUrl";
            this.labelAddUrl.Size = new System.Drawing.Size(65, 20);
            this.labelAddUrl.TabIndex = 6;
            this.labelAddUrl.Text = "Track URL";
            this.labelAddUrl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupPanelManualPlaylist
            // 
            this.groupPanelManualPlaylist.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupPanelManualPlaylist.Controls.Add(this.checkBoxAutoUsePlaylistId);
            this.groupPanelManualPlaylist.Controls.Add(this.buttonLoadPlaylistById);
            this.groupPanelManualPlaylist.Controls.Add(this.textboxPlaylistId);
            this.groupPanelManualPlaylist.Controls.Add(this.labelManualPlaylistId);
            this.groupPanelManualPlaylist.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupPanelManualPlaylist.ForeColor = System.Drawing.Color.White;
            this.groupPanelManualPlaylist.Location = new System.Drawing.Point(6, 415);
            this.groupPanelManualPlaylist.Name = "groupPanelManualPlaylist";
            this.groupPanelManualPlaylist.Size = new System.Drawing.Size(306, 95);
            this.groupPanelManualPlaylist.TabIndex = 8;
            this.groupPanelManualPlaylist.TabStop = false;
            this.groupPanelManualPlaylist.Text = "MANUAL PLAYLIST";
            // 
            // checkBoxAutoUsePlaylistId
            // 
            this.checkBoxAutoUsePlaylistId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxAutoUsePlaylistId.Location = new System.Drawing.Point(87, 73);
            this.checkBoxAutoUsePlaylistId.Name = "checkBoxAutoUsePlaylistId";
            this.checkBoxAutoUsePlaylistId.Size = new System.Drawing.Size(212, 18);
            this.checkBoxAutoUsePlaylistId.TabIndex = 3;
            this.checkBoxAutoUsePlaylistId.Text = "Automatically use this playlist for maps with no defined tracks";
            this.checkBoxAutoUsePlaylistId.UseVisualStyleBackColor = true;
            // 
            // buttonLoadPlaylistById
            // 
            this.buttonLoadPlaylistById.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLoadPlaylistById.BackColor = System.Drawing.Color.Gray;
            this.buttonLoadPlaylistById.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLoadPlaylistById.Location = new System.Drawing.Point(88, 43);
            this.buttonLoadPlaylistById.Name = "buttonLoadPlaylistById";
            this.buttonLoadPlaylistById.Size = new System.Drawing.Size(211, 23);
            this.buttonLoadPlaylistById.TabIndex = 2;
            this.buttonLoadPlaylistById.Text = "Start playlist with specified ID";
            this.buttonLoadPlaylistById.UseVisualStyleBackColor = false;
            this.buttonLoadPlaylistById.Click += new System.EventHandler(this.OnStartPlaylistByIdButtonClick);
            // 
            // textboxPlaylistId
            // 
            this.textboxPlaylistId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textboxPlaylistId.BackColor = System.Drawing.Color.DimGray;
            this.textboxPlaylistId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textboxPlaylistId.Location = new System.Drawing.Point(87, 16);
            this.textboxPlaylistId.Name = "textboxPlaylistId";
            this.textboxPlaylistId.Size = new System.Drawing.Size(212, 20);
            this.textboxPlaylistId.TabIndex = 1;
            // 
            // labelManualPlaylistId
            // 
            this.labelManualPlaylistId.Location = new System.Drawing.Point(8, 16);
            this.labelManualPlaylistId.Margin = new System.Windows.Forms.Padding(0);
            this.labelManualPlaylistId.Name = "labelManualPlaylistId";
            this.labelManualPlaylistId.Size = new System.Drawing.Size(73, 20);
            this.labelManualPlaylistId.TabIndex = 0;
            this.labelManualPlaylistId.Text = "Playlist ID";
            this.labelManualPlaylistId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupPanelSearchlist
            // 
            this.groupPanelSearchlist.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupPanelSearchlist.BackColor = System.Drawing.Color.Transparent;
            this.groupPanelSearchlist.Controls.Add(this.checkBoxAutoUseSearchTerms);
            this.groupPanelSearchlist.Controls.Add(this.buttonSetSearchPlaylist);
            this.groupPanelSearchlist.Controls.Add(this.labelSearchField);
            this.groupPanelSearchlist.Controls.Add(this.textboxSearchPlaylistField);
            this.groupPanelSearchlist.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupPanelSearchlist.ForeColor = System.Drawing.Color.White;
            this.groupPanelSearchlist.Location = new System.Drawing.Point(6, 316);
            this.groupPanelSearchlist.Name = "groupPanelSearchlist";
            this.groupPanelSearchlist.Size = new System.Drawing.Size(306, 93);
            this.groupPanelSearchlist.TabIndex = 7;
            this.groupPanelSearchlist.TabStop = false;
            this.groupPanelSearchlist.Text = "SEARCH PLAYLIST";
            // 
            // checkBoxAutoUseSearchTerms
            // 
            this.checkBoxAutoUseSearchTerms.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxAutoUseSearchTerms.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.checkBoxAutoUseSearchTerms.Location = new System.Drawing.Point(87, 71);
            this.checkBoxAutoUseSearchTerms.Name = "checkBoxAutoUseSearchTerms";
            this.checkBoxAutoUseSearchTerms.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.checkBoxAutoUseSearchTerms.Size = new System.Drawing.Size(212, 18);
            this.checkBoxAutoUseSearchTerms.TabIndex = 7;
            this.checkBoxAutoUseSearchTerms.Text = "Automatically use search playlist for maps with no defined tracks";
            this.checkBoxAutoUseSearchTerms.UseVisualStyleBackColor = true;
            // 
            // buttonSetSearchPlaylist
            // 
            this.buttonSetSearchPlaylist.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSetSearchPlaylist.BackColor = System.Drawing.Color.Gray;
            this.buttonSetSearchPlaylist.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonSetSearchPlaylist.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSetSearchPlaylist.Location = new System.Drawing.Point(88, 42);
            this.buttonSetSearchPlaylist.Name = "buttonSetSearchPlaylist";
            this.buttonSetSearchPlaylist.Size = new System.Drawing.Size(211, 23);
            this.buttonSetSearchPlaylist.TabIndex = 6;
            this.buttonSetSearchPlaylist.Text = "Start playlist using search terms";
            this.buttonSetSearchPlaylist.UseVisualStyleBackColor = false;
            this.buttonSetSearchPlaylist.Click += new System.EventHandler(this.OnStartSearchPlaylistButtonClick);
            // 
            // labelSearchField
            // 
            this.labelSearchField.BackColor = System.Drawing.Color.Transparent;
            this.labelSearchField.Location = new System.Drawing.Point(8, 16);
            this.labelSearchField.Margin = new System.Windows.Forms.Padding(0);
            this.labelSearchField.Name = "labelSearchField";
            this.labelSearchField.Size = new System.Drawing.Size(76, 20);
            this.labelSearchField.TabIndex = 5;
            this.labelSearchField.Text = "Search Terms";
            this.labelSearchField.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textboxSearchPlaylistField
            // 
            this.textboxSearchPlaylistField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textboxSearchPlaylistField.BackColor = System.Drawing.Color.DimGray;
            this.textboxSearchPlaylistField.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textboxSearchPlaylistField.ForeColor = System.Drawing.Color.Black;
            this.textboxSearchPlaylistField.Location = new System.Drawing.Point(87, 16);
            this.textboxSearchPlaylistField.Name = "textboxSearchPlaylistField";
            this.textboxSearchPlaylistField.Size = new System.Drawing.Size(212, 20);
            this.textboxSearchPlaylistField.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 161);
            this.panel1.MaximumSize = new System.Drawing.Size(418, 35);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(318, 0);
            this.panel1.TabIndex = 7;
            // 
            // panelPlayer
            // 
            this.panelPlayer.Controls.Add(this.buttonToggleConrol);
            this.panelPlayer.Controls.Add(this.buttonRefresh);
            this.panelPlayer.Controls.Add(this.buttonSwitch);
            this.panelPlayer.Controls.Add(this.panel1);
            this.panelPlayer.Controls.Add(this.GlamFrame);
            this.panelPlayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPlayer.Location = new System.Drawing.Point(0, 0);
            this.panelPlayer.MaximumSize = new System.Drawing.Size(1024, 0);
            this.panelPlayer.Name = "panelPlayer";
            this.panelPlayer.Size = new System.Drawing.Size(318, 161);
            this.panelPlayer.TabIndex = 7;
            // 
            // buttonToggleConrol
            // 
            this.buttonToggleConrol.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonToggleConrol.BackgroundImage = global::GlamPlayer.Properties.Resources.Universal_Multi_Tool;
            this.buttonToggleConrol.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonToggleConrol.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonToggleConrol.Location = new System.Drawing.Point(280, 84);
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
            this.buttonRefresh.Location = new System.Drawing.Point(280, 46);
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
            this.buttonSwitch.Location = new System.Drawing.Point(280, 8);
            this.buttonSwitch.Name = "buttonSwitch";
            this.buttonSwitch.Size = new System.Drawing.Size(32, 32);
            this.buttonSwitch.TabIndex = 0;
            this.buttonSwitch.UseVisualStyleBackColor = true;
            this.buttonSwitch.Click += new System.EventHandler(this.OnTransparencyButtonClick);
            // 
            // GlamFrame
            // 
            this.GlamFrame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GlamFrame.Location = new System.Drawing.Point(0, 0);
            this.GlamFrame.MinimumSize = new System.Drawing.Size(20, 20);
            this.GlamFrame.Name = "GlamFrame";
            this.GlamFrame.ScrollBarsEnabled = false;
            this.GlamFrame.Size = new System.Drawing.Size(318, 161);
            this.GlamFrame.TabIndex = 5;
            this.GlamFrame.VolumeFadeEnabled = false;
            this.GlamFrame.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.OnBrowserPanelLoadingComplete);
            // 
            // GlamWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(318, 680);
            this.Controls.Add(this.panelPlayer);
            this.Controls.Add(this.panelControl);
            this.Name = "GlamWindow";
            this.Text = "Glam Player";
            this.panelControl.ResumeLayout(false);
            this.dedicatedMapPlaylist.ResumeLayout(false);
            this.dedicatedMapPlaylist.PerformLayout();
            this.groupPanelDedicatedPlaylist.ResumeLayout(false);
            this.groupPanelDedicatedPlaylist.PerformLayout();
            this.groupPanelManualPlaylist.ResumeLayout(false);
            this.groupPanelManualPlaylist.PerformLayout();
            this.groupPanelSearchlist.ResumeLayout(false);
            this.groupPanelSearchlist.PerformLayout();
            this.panelPlayer.ResumeLayout(false);
            this.panelPlayer.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textboxTrackUrl;
        private System.Windows.Forms.Button buttonAddTrackUrl;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.Panel panelControl;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonSwitch;
        private System.Windows.Forms.Button buttonToggleConrol;
        private System.Windows.Forms.Panel panelPlayer;
        private System.Windows.Forms.TextBox listOfMusicTracks;
        private GlamFrame GlamFrame;
        private System.Windows.Forms.TextBox textboxSearchPlaylistField;
        private System.Windows.Forms.Label labelSearchField;
        private System.Windows.Forms.Label labelAddUrl;
        private System.Windows.Forms.GroupBox groupPanelSearchlist;
        private System.Windows.Forms.CheckBox checkBoxAutoUseSearchTerms;
        private System.Windows.Forms.Button buttonSetSearchPlaylist;
        private System.Windows.Forms.GroupBox groupPanelManualPlaylist;
        private System.Windows.Forms.CheckBox checkBoxAutoUsePlaylistId;
        private System.Windows.Forms.Button buttonLoadPlaylistById;
        private System.Windows.Forms.TextBox textboxPlaylistId;
        private System.Windows.Forms.Label labelManualPlaylistId;
        private System.Windows.Forms.GroupBox groupPanelDedicatedPlaylist;
        private System.Windows.Forms.GroupBox dedicatedMapPlaylist;
        private System.Windows.Forms.Button buttonGetCurrentTrackUrl;
        private System.Windows.Forms.Button buttonRemoveCurrentTrack;




    }
}

