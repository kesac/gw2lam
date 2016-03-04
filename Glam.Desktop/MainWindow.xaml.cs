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
using MahApps.Metro.Controls;
using Glam;


namespace Glam.Desktop
{

    public partial class GlamDesktop : MetroWindow
    {

        private MapService MapService;
        private DesktopMusicProvider MusicProvider;
        private DesktopMusicPlayer MusicPlayer;
        private PlayerMonitor Monitor;
        private uint LastMapId;

        private TimerTask DelayedUpdateStartTimer;
        private TimerTask TrackDisplayTimer;

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

            this.TrackDisplayTimer = new TimerTask();
            this.TrackDisplayTimer.Interval = 1000;
            this.TrackDisplayTimer.Elapsed += UpdateTrackDisplay;
            this.TrackDisplayTimer.Start();
        }

        private void UpdateTrackDisplay(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (this.MusicPlayer.IsPlaying)
            {
                this.Dispatcher.BeginInvoke((Action)(() => this.LabelCurrentTrack.Content = this.MusicPlayer.TrackName));
                this.Dispatcher.BeginInvoke((Action)(() => this.ProgressBar.Value = this.MusicPlayer.TrackPosition));
                this.Dispatcher.BeginInvoke((Action)(() => this.ProgressBar.Maximum = this.MusicPlayer.TrackLength));
            }
            else
            {
                this.Dispatcher.BeginInvoke((Action)(() => this.LabelCurrentTrack.Content = string.Empty));
                this.Dispatcher.BeginInvoke((Action)(() => this.ProgressBar.Value = 0));
                this.Dispatcher.BeginInvoke((Action)(() => this.ProgressBar.Maximum = 1));
            }
        }

        private void OnUpdateStopped(object sender, MapChangeEventArgs e)
        {
            this.Dispatcher.BeginInvoke((Action)(() => this.Title = "Unknown Location"));
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
            this.Dispatcher.BeginInvoke((Action)(() => this.Title = mapName));

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
            this.DelayedUpdateStartTimer.Stop();
            this.TrackDisplayTimer.Stop();
        }
    }
}
