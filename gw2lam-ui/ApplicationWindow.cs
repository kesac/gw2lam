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
        private readonly string MusicPage = "https://www.youtube.com/v/{0}?autoplay=1&loop=1&playlist={1}";
        private readonly string EmptyPage = "<html><head><body style=\"font-family: serif; background-color: black; color: white\">No music available for this area.</body></html>";

        private MapManager maps;
        private MusicManager musicManager;
        private PlayerTracker tracker;

        public ApplicationWindow()
        {
            InitializeComponent();
            this.Text = "";
            this.browser.Disposed += browser_Disposed;
            this.browser.DocumentText = EmptyPage;

            this.maps = new MapManager();
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
        private void SetVideo(string id)
        {
            if (id == null)
            {
                this.browser.DocumentText = EmptyPage;
            }
            else
            {
                this.browser.Navigate(string.Format(MusicPage, id, id));
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
                this.SetVideo(tracks[0]);
            }
            else
            {
                this.SetVideo(null);
            }
        }


        private void OnUpdateStop(object sender, PlayerTrackerEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Updates stopped on " + DateTime.Now);
            this.SetVideo(null);
        }

        private void OnUpdateStart(object sender, PlayerTrackerEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Updates started on " + DateTime.Now);
        }


        private void browser_Disposed(object sender, EventArgs e)
        {
            // application will keep running even if the window is closed unless the tracker is stopped as well
            this.tracker.Stop();
        }

        private void browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }
    }
}
