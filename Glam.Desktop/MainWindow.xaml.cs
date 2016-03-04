using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TimerTask = System.Timers.Timer;
using Glam;

namespace Glam.Desktop
{

    public partial class GlamDesktop : Window
    {

        private MapService MapService;
        private DesktopMusicProvider MusicProvider;
        private DesktopMusicPlayer MusicPlayer;
        private PlayerMonitor Monitor;
        private uint LastMapId;

        private TimerTask DelayedUpdateStartTimer;

        public GlamDesktop()
        {
            InitializeComponent();

            this.MapService = new MapService();
            this.MapService.LoadMapData(); // this makes calls to GW2's web API if a cache of the map data doesn't exist yet

            this.MusicProvider = new DesktopMusicProvider("music");
            this.MusicPlayer = new DesktopMusicPlayer();

            this.Monitor = new PlayerMonitor();
            this.Monitor.UpdateStarted += OnUpdateStarted;
            this.Monitor.UpdateStopped += OnUpdateStopped;
            this.Monitor.Start();
            this.Closed += OnWindowClosed;

            this.LastMapId = 0;

            this.DelayedUpdateStartTimer = new TimerTask();
            this.DelayedUpdateStartTimer.Interval = 1000;
            this.DelayedUpdateStartTimer.Elapsed += OnDelayedUpdateStart;
        }
        
        private void OnUpdateStopped(object sender, MapChangeEventArgs e)
        {
            this.Dispatcher.BeginInvoke((Action)(() => this.LabelLocation.Content = "Unknown Location"));
            this.MusicPlayer.FadeOut();
        }

        private void OnUpdateStarted(object sender, MapChangeEventArgs e)
        {
            this.DelayedUpdateStartTimer.Start();
        }

        private void OnDelayedUpdateStart(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.DelayedUpdateStartTimer.Stop();
            
            uint currentMapId = this.Monitor.GetCurrentMap();

            string mapName = this.MapService.ResolveName(currentMapId);
            this.Dispatcher.BeginInvoke((Action)(() => this.LabelLocation.Content = mapName));

            if (currentMapId == this.LastMapId)
            {
                this.MusicPlayer.FadeIn();
            }
            else
            {
                this.LastMapId = currentMapId;
                this.MusicPlayer.Playlist = this.MusicProvider.GetMapMusic(mapName);
                this.MusicPlayer.ShufflePlaylist();
                this.MusicPlayer.StartPlaylist();
            }
        }
        
        private void OnWindowClosed(object sender, EventArgs e)
        {
            this.MusicPlayer.Stop();
            this.Monitor.Stop();
        }
    }
}
