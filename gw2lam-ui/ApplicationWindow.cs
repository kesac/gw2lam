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

namespace gw2lam_ui
{
    public partial class ApplicationWindow : Form
    {

        private readonly string musicPage = "<html><body><iframe width=\"320\" height=\"192\" src=\"https://www.youtube.com/embed/{0}?autoplay=1\" frameborder=\"0\" allowfullscreen></iframe></body></html>";
        private readonly string emptyPage = "<html><head><body>Not playing!</body></html>";
        
        private PlayerTracker tracker;
        private MusicPlayer music;

        public ApplicationWindow()
        {
            InitializeComponent();
            this.tracker = new PlayerTracker();
            this.music = new MusicPlayer();

            this.tracker.OnMapChange += tracker_OnMapChange;
            this.browser.DocumentText = emptyPage;
            this.tracker.Start();

        }

        private void tracker_OnMapChange(object sender, PlayerTrackerEventArgs e)
        {
            if (e.MapID == 15)
            {
                this.browser.DocumentText = string.Format(musicPage, "Jxnf5uaeRDE");
            }
            else
            {
                this.browser.DocumentText = emptyPage;
            }
        }

        private void browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }
    }
}
