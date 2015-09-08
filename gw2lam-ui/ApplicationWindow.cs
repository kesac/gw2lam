using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using gw2lam;

namespace gw2lam.UI
{
    public partial class ApplicationWindow : Form
    {
        private readonly string MusicPage = "https://www.youtube.com/v/{0}?autoplay=1&loop=1&autohide=0&playlist={1}";
        private readonly string EmptyPage = "<html><head><body style=\"font-family: courier; background-color: black; color: white\">Either no music has been defined for your current area or no positional updates are being received from Guild Wars 2. Modify <b>" + MusicManager.OnlineMusicFile +"</b> to add music!</body></html>";

        private MapManagerOld maps;
        private MusicManager musicManager;
        private PlayerTracker tracker;

        public ApplicationWindow()
        {
            InitializeComponent();
            this.Text = "";
            this.browser.Disposed += browser_Disposed;
            this.browser.DocumentText = EmptyPage;

            this.maps = new MapManagerOld();
            this.maps.InitializeLocalCache();

            this.musicManager = new MusicManager(MusicMode.Online);

            this.tracker = new PlayerTracker();
            this.tracker.OnMapChange += OnMapChange;
            this.tracker.OnUpdateStop += OnUpdateStop;
            this.tracker.OnUpdateStart += OnUpdateStart;
            this.tracker.Start();
        }

        /// <summary>
        /// TODO: Allow method to receive multiple videos
        /// </summary>
        /// <param name="id"></param>
        private void SetVideo(params string[] ids)
        {
            if (ids == null && ids.Length > 0)
            {
                this.browser.DocumentText = EmptyPage;
            }
            else
            {
                StringBuilder playlist = new StringBuilder();

                if (ids.Length > 1) // additional tracks must be placed in the playlist parameter
                { 
                    for (int i = 1; i < ids.Length; i++)
                    {
                        playlist.Append(ids[i] + ",");
                    }
                    playlist.Remove(playlist.Length - 1, 1); // remove trailing comma
                }
                else // If there's only one track, we must place the single track in its own playlist or it won't loop
                {
                    playlist.AppendLine(ids[0]);
                }

                this.browser.Navigate(string.Format(MusicPage, ids[0], playlist.ToString()));
            }
        }


        private void OnMapChange(object sender, PlayerTrackerEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Map changed to " + e.MapID);

            string title = "GW2-LAM: " + this.tracker.PlayerName + " currently in " + this.maps.GetName(e.MapID);

            if (this.InvokeRequired) {
                this.Invoke(new MethodInvoker(delegate { this.Text = title; }));
            }
            else
            {
                this.Text = title;
            }

            List<string> tracks = this.musicManager.GetTracks(this.maps.GetName(e.MapID));

            if (tracks.Count > 0)
            {
                this.ShuffleList(tracks);
                this.SetVideo(tracks.ToArray());
            }
            else
            {
                this.SetVideo(null);
            }
        }

        private void ShuffleList(List<string> tracks)
        {
            Random r = new Random();
            int size = tracks.Count;
            for (int i = size - 1; i > 0; i--)
            {
                int index = r.Next(i + 1);
                string swap = tracks[index];
                tracks.RemoveAt(index);
                tracks.Add(swap);
            }

        }

        private void OnUpdateStop(object sender, PlayerTrackerEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Updates stopped on " + DateTime.Now);
            this.SetVideo(null);
            string title = "GW2-LAM: " + this.tracker.PlayerName + " currently in an unknown area";
        }

        private void OnUpdateStart(object sender, PlayerTrackerEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Updates started on " + DateTime.Now);
        }


        private void browser_Disposed(object sender, EventArgs e)
        {
            // application will keep running even if the window is closed unless the tracker is stopped as well
            this.tracker.CleanUpLogFiles();
            this.tracker.Stop();
        }

        private void browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }
    }
}
