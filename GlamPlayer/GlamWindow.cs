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

namespace GlamPlayer
{
    [ComVisibleAttribute(true)]
    public partial class GlamWindow : Form
    {
        private readonly string VideoPlayerLocation = "http://turtlesort.com/glam/";
        private readonly uint UnknownMap = 0;
        private readonly int ControlPanelWidth = 300; // in pixels

        private GlamCore Core;
        private MapChangeListener Listener;
        private uint CurrentMapId;

        private int TitleHeight;
        private int BorderWidth;

        public GlamWindow()
        {
            InitializeComponent();
            this.TopMost = true;
            this.panelControl.Width = ControlPanelWidth;


            this.browserPanel.ObjectForScripting = this;

            // We're using the YT IFrame API, but the event callbacks
            // will not work if loading from a local page because of browser security settings.
            // It's easier to load an existing template page off the web.
            this.browserPanel.Navigate(VideoPlayerLocation);

            this.CurrentMapId = UnknownMap;

            this.Core = new GlamCore();
            this.Core.Load();

            //this.InitializeMapChangeListener();
            this.FormClosing += OnWindowClosing;

            this.TitleHeight = this.PointToScreen(Point.Empty).Y - this.Top;  // http://stackoverflow.com/questions/18429425/c-sharp-absolute-position-of-control-on-screen
            this.BorderWidth = this.PointToScreen(Point.Empty).X - this.Left;

        }

        // This is called by the YT IFrame after it is done loading...
        public void InitializeMapChangeListener()
        {
            this.Listener = new MapChangeListener();
            this.Listener.OnMapChange += OnMapChange;
            this.Listener.OnUpdateStop += OnMapUpdateStop;
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

        private void PlayMapMusic()
        {
            List<MusicTrack> tracks = this.Core.GetMapMusic(this.CurrentMapId);
            if (tracks.Count > 0)
            {

                StringBuilder playlist = new StringBuilder();

                if (tracks.Count > 1) // additional tracks must be placed in the playlist parameter
                {
                    for (int i = 1; i < tracks.Count; i++)
                    {
                        playlist.Append(tracks[i].Id + ",");
                    }
                    playlist.Remove(playlist.Length - 1, 1); // remove trailing comma
                }
                else // If there's only one track, we must place the single track in its own playlist or it won't loop
                {
                    playlist.AppendLine(tracks[0].Id);
                }

                //this.browserPanel.Navigate(string.Format(MusicPage, tracks[0].Id, playlist.ToString()));
                this.ThreadAwareInvocation(delegate { this.browserPanel.Document.InvokeScript("play", new object[] { tracks[0].Id }); });

            }
            else
            {
                this.ThreadAwareInvocation(delegate { this.browserPanel.Document.InvokeScript("stop"); });
            }
        }

        private void RefreshMusicTrackList()
        {
            List<MusicTrack> tracks = this.Core.GetMapMusic(this.CurrentMapId);

            StringBuilder list = new StringBuilder();

            foreach (MusicTrack track in tracks)
            {
                list.AppendLine("\"" + track.Title + "\"");
            }

            this.ThreadAwareInvocation(delegate { this.listOfMusicTracks.Text = list.ToString(); });
        }


        private void OnMapChange(object sender, MapChangeEventArgs e)
        {
            this.CurrentMapId = e.MapID;
            this.SetWindowTitle(this.Core.GetMapName(this.CurrentMapId));
            this.PlayMapMusic();
            this.RefreshMusicTrackList();
        }

        private void OnMapUpdateStop(object sender, MapChangeEventArgs e)
        {
            this.SetWindowTitle("Currently in unknown location");
            this.CurrentMapId = UnknownMap;
        }



        private void OnAddButtonClick(object sender, EventArgs e)
        {
            string path = this.textMusicPath.Text;

            if (this.CurrentMapId != UnknownMap)
            {
                WebClient client = new WebClient();
                string rawData = client.DownloadString(path);

                System.IO.File.WriteAllText("test.txt", rawData);

                MatchCollection matches = Regex.Matches(rawData, "<meta name=\"title\" content=(.*?)>");
                string title = matches[0].Groups[1].Value;

                if (title == null)
                {
                    title = "Unknown";
                }

                MusicTrack track = new MusicTrack(this.CurrentMapId, path, title);

                this.Core.AddMapMusic(track);
            }

            this.textMusicPath.Clear();
            this.RefreshMusicTrackList();
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
                this.Width -= ControlPanelWidth;
            }
            else
            {
                this.panelControl.Show();
                this.Width += ControlPanelWidth;
            }
        }

        private void OnRefreshButtonClick(object sender, EventArgs e)
        {
            this.PlayMapMusic();
        }


        private void OnBrowserPanelLoadingComplete(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            // InitializeMapChangeListener();
        }

        private void OnWindowClosing(object sender, FormClosingEventArgs e)
        {
            this.Core.SaveTrackData();

            if (this.Listener != null) // if the html page's iframe never loaded, this will be null
            {
                this.Listener.CleanUpLogFiles();
                this.Listener.Stop();
            }

        }

    }
}
