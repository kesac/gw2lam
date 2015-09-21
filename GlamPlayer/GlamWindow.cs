using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using Turtlesort.Glam.Core;

using Gma.System.MouseKeyHook;

namespace GlamPlayer
{
    [ComVisibleAttribute(true)]
    public partial class GlamWindow : Form
    {
        private readonly string SettingsFile = "application.settings.json";
        private readonly string VideoPlayerLocation = "http://turtlesort.com/glam/";
        private readonly string KeySearchTerms = "searchterms";
        private readonly string KeySearchAuto = "searchauto";
        private readonly string KeyManualPlaylistId = "manualplaylistid";
        private readonly string KeyManualPlaylistAuto = "manualplaylistauto";
        private readonly uint UnknownMap = 0;
        
        private GlamCore Core;
        private MapChangeListener Listener;
        private Settings appSettings;
        private uint CurrentMapId;

        private System.Timers.Timer OnMapUpdateTimer;
        private IKeyboardEvents KeyboardListener;
        
        private int TitleHeight;
        private int ControlPanelHeight;
        private int BorderWidth;

        public GlamWindow()
        {
            InitializeComponent();

            this.TopMost = true;

            this.SetWindowTitle("Loading frame...");
            this.GlamFrame.OnReady += OnGlamFrameReady;
            this.GlamFrame.Initialize(VideoPlayerLocation);
            this.GlamFrame.VolumeFadeEnabled = true;

            ControlPanelHeight = this.panelControl.Height;
            this.Height -= ControlPanelHeight;
            this.panelControl.Hide();

            this.CurrentMapId = UnknownMap;

            this.Core = new GlamCore();
            this.Core.Load();

            this.appSettings = new Settings();

            this.FormClosing += OnWindowClosing;
            this.Resize += OnWindowResize;

            this.TitleHeight = this.PointToScreen(Point.Empty).Y - this.Top;  // http://stackoverflow.com/questions/18429425/c-sharp-absolute-position-of-control-on-screen
            this.BorderWidth = this.PointToScreen(Point.Empty).X - this.Left;

            this.OnMapUpdateTimer = new System.Timers.Timer();
            this.OnMapUpdateTimer.Interval = 1000;
            this.OnMapUpdateTimer.Elapsed += DelayedOnMapUpdateStart;

            this.KeyboardListener = Hook.GlobalEvents();
            
            this.KeyboardListener.KeyDown += OnGlobalKeyDown;

            this.LoadFieldData();

        }

        private void OnGlamFrameReady()
        {
            this.SetWindowTitle("GW2 Location-Aware Music Player");
            this.InitializeMapChangeListener();
        }

        // Called by glam frame when it is done loading its contents
        private void InitializeMapChangeListener()
        {
            this.Listener = new MapChangeListener();
            this.Listener.OnMapChange += OnMapChange;
            this.Listener.OnUpdateStop += OnMapUpdateStop;
            this.Listener.OnUpdateStart += OnMapUpdateStart;
            this.Listener.Start();
        }

        private void ThreadAwareInvocation(Action action)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(action));
            }
            else
            {
                action();
            }
        }

        private void SetWindowTitle(string title)
        {
            this.ThreadAwareInvocation(delegate { this.Text = title; });
        }

        
        private void RefreshMapMusic()
        {
            List<MusicTrack> tracks = new List<MusicTrack>();
            tracks.AddRange(this.Core.GetMapMusic(this.CurrentMapId));

            if (tracks.Count > 0)
            {
                this.GlamFrame.SetPlaylist(tracks);
                this.GlamFrame.StartPlayback();
                //this.RefreshTracklistText();
            }
            else if (this.textboxSearchPlaylistField.Text != string.Empty && this.checkBoxAutoUseSearchTerms.Checked)
            {
                this.LoadSearchPlaylistMusic();
            }
            else if (this.textboxPlaylistId.Text != string.Empty && this.checkBoxAutoUsePlaylistId.Checked)
            {
                this.LoadPlaylistByIdMusic();
            }
        }

        private void LoadSearchPlaylistMusic()
        {
            this.GlamFrame.SetSearchPlaylist(this.textboxSearchPlaylistField.Text.Trim());
            this.GlamFrame.StartPlayback();
            //this.RefreshTracklistText("Custom search", "Custom search terms: " + this.textboxSearchPlaylistField.Text);
        }
        /**/

        private void LoadPlaylistByIdMusic()
        {
            this.GlamFrame.SetPlaylistById(this.textboxPlaylistId.Text.Trim());
            this.GlamFrame.StartPlayback();
            //this.RefreshTracklistText("Custom playlist", "Playlist ID: " + this.textboxPlaylistId.Text);
        }

        // Refreshes the list of tracks in the control panel
        // If no arguments are given, the current map's tracks are use to populate
        // the list.

        /*
        private void RefreshTracklistText()
        {
            if (this.CurrentMapId != this.UnknownMap)
            {
                string mapName = this.Core.GetMapName(this.CurrentMapId);
                List<MusicTrack> tracks = this.Core.GetMapMusic(this.CurrentMapId);

                StringBuilder list = new StringBuilder();

                foreach (MusicTrack track in tracks)
                {
                    list.AppendLine("+" + track.Title);
                }

                this.RefreshTracklistText(mapName, list.ToString());    
            }
        }

        private void RefreshTracklistText(string mapName, string contents)
        {
            this.ThreadAwareInvocation(delegate {
                this.groupPanelDedicatedPlaylist.Text = mapName;
                this.listOfMusicTracks.Text = contents; 
            });
        }*/

        // Ensures the IFrame expands to fit its panel
        private void ResizeGlamFrame()
        {
            Size size = this.GlamFrame.Size;
            this.GlamFrame.SetFrameSize(size.Width, size.Height);
        }

        private void SaveFieldData()
        {
            this.appSettings.data[KeySearchTerms] = this.textboxSearchPlaylistField.Text;
            this.appSettings.data[KeySearchAuto] = this.checkBoxAutoUseSearchTerms.Checked;
            this.appSettings.data[KeyManualPlaylistId] = this.textboxPlaylistId.Text;
            this.appSettings.data[KeyManualPlaylistAuto] = this.checkBoxAutoUsePlaylistId.Checked;
            this.appSettings.SaveToFile(SettingsFile);
        }

        private void LoadFieldData()
        {
            if (File.Exists(SettingsFile))
            {
                this.appSettings.LoadFromFile(SettingsFile);

                if (this.appSettings.data.ContainsKey(KeySearchTerms))
                {
                    this.textboxSearchPlaylistField.Text = (string)this.appSettings.data[KeySearchTerms];
                }

                if (this.appSettings.data.ContainsKey(KeySearchAuto))
                {
                    this.checkBoxAutoUseSearchTerms.Checked = (bool)this.appSettings.data[KeySearchAuto];
                }

                if (this.appSettings.data.ContainsKey(KeyManualPlaylistId))
                {
                    this.textboxPlaylistId.Text = (string)this.appSettings.data[KeyManualPlaylistId];
                }

                if (this.appSettings.data.ContainsKey(KeyManualPlaylistAuto))
                {
                    this.checkBoxAutoUsePlaylistId.Checked = (bool)this.appSettings.data[KeyManualPlaylistAuto];
                }
            }
        }

        // This doesn't catch when the player uses a waypoint in the same map, so we rely
        // on OnMapUpdateStart() instead.
        private void OnMapChange(object sender, MapChangeEventArgs e)
        {
            System.Console.WriteLine("OnMapChange:\t" + e.MapID + "\t" + DateTime.Now);           
        }

        // Sometimes when this event is fired, the GW2 api does not provide the new
        // map ID right away, making it look like the player stayed on the same map.
        // Instead of updating the map music right away, we delay ourselves slightly
        // before retrieving the map id from the map change listener object directly.
        private void OnMapUpdateStart(object sender, MapChangeEventArgs e)
        {
            OnMapUpdateTimer.Start(); // This is set up to call DelayedOnMapUpdateStart()
            System.Console.WriteLine("OnMapUpdateStart:\t" + e.MapID + "\t" + DateTime.Now);
        }

        private void DelayedOnMapUpdateStart(object sender, EventArgs e)
        {
            OnMapUpdateTimer.Stop(); // Prevents additional calls to this method and also resets the timer

            bool mapChanged = this.CurrentMapId != this.Listener.GetCurrentMap();
            this.CurrentMapId = this.Listener.GetCurrentMap();

            this.SetWindowTitle(this.Core.GetMapName(this.CurrentMapId));

            if(mapChanged){
                this.RefreshMapMusic();
            }
            else
            {
                this.GlamFrame.StartPlayback(); // ie. we resume playback since the tracklist stays the same
            }

            System.Console.WriteLine("Delayed OnMapUpdateStart:\t" + this.CurrentMapId + "\t" + DateTime.Now);
        }

        private void OnMapUpdateStop(object sender, MapChangeEventArgs e)
        {
            //this.CurrentMapId = UnknownMap;
            this.SetWindowTitle("Currently in unknown location");
            this.GlamFrame.PausePlayback();

            System.Console.WriteLine("OnMapUpdateStop:\t" + e.MapID + "\t" + DateTime.Now);
        }

        private void OnAddNewTrackButtonClick(object sender, EventArgs e)
        {
            string path = this.textboxTrackUrl.Text;

            bool success = false;
            if (this.CurrentMapId != UnknownMap)
            {
                if (Regex.IsMatch(path, "^https://www\\.youtube\\.com/watch\\?v=(.+)$"))
                {
                    WebClient client = new WebClient();
                    string rawData = client.DownloadString(path);

                    MatchCollection matches = Regex.Matches(rawData, "<meta name=\"title\" content=(.*?)>");
                    string title = matches[0].Groups[1].Value;
                    title = title.Replace("\"", "");

                    if (title == null)
                    {
                        title = "Unknown";
                    }

                    success = this.Core.AddMapMusic(this.CurrentMapId, path, title);
                }
            }

            this.textboxTrackUrl.Clear();

            if (success)
            {
                //this.RefreshTracklistText();
            }
            else
            {
                MessageBox.Show("Could not add " + path + ". A duplicate entry may already exist. Must also be in the format https://www.youtube.com/watch?v=<id>.");
            }
        }

        private void OnGetCurrentTrackUrlButtonClick(object sender, EventArgs e)
        {
            string url = this.GlamFrame.getCurrentVideoUrl();

            if (url != null && url.Trim().Length > 0)
            {
                MatchCollection matches = Regex.Matches(url,"^.*?v\\=(.*?)(&.*)?$");
                url = "https://www.youtube.com/watch?v=" + matches[0].Groups[1].Value;
                this.textboxTrackUrl.Text = url;
            }
        }

        private void OnRemoveCurrentTrackButtonClick(object sender, EventArgs e)
        {
            string url = this.GlamFrame.getCurrentVideoUrl();

            if (url != null && url.Trim().Length > 0)
            {
                url = url.Replace("feature=player_embedded&", "");

                // MusicTrack's Id property extracts the value from the url
                MusicTrack track = new MusicTrack(UnknownMap, url, null);
                string trackId = track.Id;
                bool success = this.Core.RemoveMapMusic(this.CurrentMapId, trackId);

                if (success) { 
                    //this.RefreshTracklistText();
                }
                else
                {
                    MessageBox.Show("Could not remove " + url + ". It did not exist in playlist.");
                }
            }
        }

        private void OnStartSearchPlaylistButtonClick(object sender, EventArgs e)
        {
            if (this.textboxSearchPlaylistField.Text != string.Empty)
            {
                this.LoadSearchPlaylistMusic();
            }
            else
            {
                MessageBox.Show("Enter at least one term in the search field.");
            }
        }

        private void OnStartPlaylistByIdButtonClick(object sender, EventArgs e)
        {
            if (this.textboxPlaylistId.Text != string.Empty)
            {
                this.LoadPlaylistByIdMusic();
            }
            else
            {
                MessageBox.Show("Please provide a playlist ID in the playlist field.");
            }
        }

        private void OnTransparencyButtonClick(object sender, EventArgs e)
        {
            if (this.FormBorderStyle != System.Windows.Forms.FormBorderStyle.None)
            {
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                this.Top += TitleHeight;
                this.Left += BorderWidth;

                this.Opacity = 0.50;
                this.BackColor = Color.Magenta;
                this.TransparencyKey = Color.Magenta;
            }
            else
            {
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                this.Top -= TitleHeight;
                this.Left -= BorderWidth;

                this.BackColor = Color.Black;
                this.TransparencyKey = Color.Magenta;
                this.Opacity = 1;
            }
        }

        private void OnControlButtonClick(object sender, EventArgs e)
        {
            if (this.panelControl.Visible)
            {
                this.panelControl.Hide();
                this.Height -= ControlPanelHeight;
            }
            else
            {
                this.panelControl.Show();
                this.Height += ControlPanelHeight;
            }
        }

        private void OnRefreshButtonClick(object sender, EventArgs e)
        {
            this.RefreshMapMusic();
        }

        private void OnBrowserPanelLoadingComplete(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //this.InitializeMapChangeListener();
            this.ResizeGlamFrame();
        }

        public void OnGlobalKeyDown(object sender, KeyEventArgs keyEvent)
        {
            KeyEventArgsExt e = (KeyEventArgsExt)keyEvent;

            if (e.Control)
            {
                if (e.KeyCode == Keys.NumPad4)
                {
                    this.GlamFrame.StartPreviousTrack();
                }
                else if (e.KeyCode == Keys.NumPad5)
                {
                    this.GlamFrame.TogglePlayPause();
                }
                else if (e.KeyCode == Keys.NumPad6)
                {
                    this.GlamFrame.StartNextTrack();
                }
                else if (e.KeyCode == Keys.NumPad8)
                {
                    this.GlamFrame.IncreaseVolume();
                }
                else if (e.KeyCode == Keys.NumPad2)
                {
                    this.GlamFrame.DecreaseVolume();
                }
            }
        }

        private void OnWindowResize(object sender, EventArgs e)
        {
            this.ResizeGlamFrame();
        }

        private void OnWindowClosing(object sender, FormClosingEventArgs e)
        {
            this.SaveFieldData();
            this.Core.SaveTrackData();
            this.OnMapUpdateTimer.Dispose();

            if (this.Listener != null) // if the html page's iframe never loaded, this will be null
            {
                this.Listener.Stop();
                this.Listener.CleanUpLogFiles();
            }

            System.Environment.Exit(1);

        }

    }
}
