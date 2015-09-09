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

using Turtlesort.Glam.Core;

namespace GlamPlayer
{
    [ComVisibleAttribute(true)]
    public partial class GlamWindow : Form
    {

        private readonly string MusicPage = "https://www.youtube.com/v/{0}?autoplay=1&loop=1&autohide=1&playlist={1}";
        private readonly string EmptyPage = "<html><head><body style=\"font-family: courier; background-color: black; color: white\">No music available or no positional updates received from GW2.</body></html>";
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

            this.browserPanel.DocumentText = EmptyPage;
            this.browserPanel.ObjectForScripting = this;

            /*
            using (StreamReader reader = new StreamReader("YoutubeIFrame.html"))
            {
                this.browserPanel.DocumentText = reader.ReadToEnd();
            }/**/

            this.CurrentMapId = UnknownMap;

            this.Core = new GlamCore();
            this.Core.Load();

            this.InitializeMapChangeListener();
            this.FormClosing += OnWindowClosing;

            this.TitleHeight = this.PointToScreen(Point.Empty).Y - this.Top;  // http://stackoverflow.com/questions/18429425/c-sharp-absolute-position-of-control-on-screen
            this.BorderWidth = this.PointToScreen(Point.Empty).X - this.Left;

        }

        public void InitializeMapChangeListener()
        {
            this.Listener = new MapChangeListener();
            this.Listener.OnMapChange += OnMapChange;
            this.Listener.OnUpdateStop += OnMapUpdateStop;
            this.Listener.Start();

        }

        private void SetWindowTitle(string title)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate { this.Text = title; }));
            }
            else
            {
                this.Text = title;
            }
        }

        private void OnMapUpdateStop(object sender, MapChangeEventArgs e)
        {
            this.SetWindowTitle("Currently in unknown location");
            this.CurrentMapId = UnknownMap;
        }

        private void OnWindowClosing(object sender, FormClosingEventArgs e)
        {
            this.Core.SaveTrackData();

            if (this.Listener != null) { 
                this.Listener.CleanUpLogFiles();
                this.Listener.Stop();
            }

        }

        private void ReloadPlaylist()
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

                this.browserPanel.Navigate(string.Format(MusicPage, tracks[0].Id, playlist.ToString()));

            }
            else
            {
                this.browserPanel.DocumentText = EmptyPage;
            }
        }

        private void OnMapChange(object sender, MapChangeEventArgs e)
        {
            this.CurrentMapId = e.MapID;
            this.SetWindowTitle(this.Core.GetMapName(this.CurrentMapId));
            this.ReloadPlaylist();
            this.RefreshMusicTrackList();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
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

        private void buttonToggleTransparency(object sender, EventArgs e)
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

        private void buttonToggleControlPanel(object sender, EventArgs e)
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

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            this.ReloadPlaylist();
        }

        private void RefreshMusicTrackList()
        {
            List<MusicTrack> tracks = this.Core.GetMapMusic(this.CurrentMapId);

            StringBuilder list = new StringBuilder();

            foreach (MusicTrack track in tracks)
            {
                list.AppendLine("\"" + track.Title + "\"");
            }

            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate { this.listOfMusicTracks.Text = list.ToString(); }));
            }
            else
            {
                this.listOfMusicTracks.Text = list.ToString();
            }

        }

    }
}
