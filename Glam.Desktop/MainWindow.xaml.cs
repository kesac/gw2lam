using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
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
using System.Diagnostics;

namespace Glam.Desktop
{

    public partial class GlamDesktop : MetroWindow
    {

        private MapService MapService;
        private VisualsProvider VisualsProvider;
        private MusicProvider MusicProvider;
        private MusicPlayer MusicPlayer;
        private PlayerMonitor Monitor;
        private uint LastMapId;

        private TimerTask DelayedUpdateStartTimer;
        private TimerTask TrackDisplayTimer;

        public GlamDesktop()
        {
            InitializeComponent();

            this.MapService = new MapService();

            if (!this.MapService.CacheExists)
            {
                MessageBoxResult result = MessageBox.Show("Glam cannot find the file '"+this.MapService.CacheFileName+"'. This file is required to resolve map names properly. Glam can recreate this file by making calls to the official Guild Wars 2 web API.\n\n(See https://wiki.guildwars2.com/wiki/API:Main)\n\nWould you like to recreate this file?",
                    "Glam",
                    System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Information);

                if (result == MessageBoxResult.Yes)
                {
                    this.MapService.LoadFromWebApi(); // this makes calls to GW2's web API if a cache of the map data doesn't exist yet
                }
                else
                {
                    MessageBox.Show("Glam will continue running, however you must use IDs to reference maps instead of names when customizing soundtracks.", "Glam", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                this.MapService.LoadFromCache();
            }

            this.MusicProvider = new MusicProvider("music");
            this.MusicPlayer = new MusicPlayer();

            this.VisualsProvider = new VisualsProvider("visuals");

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
            this.TrackDisplayTimer.Interval = 250;
            this.TrackDisplayTimer.Elapsed += UpdateTrackDisplay;
            this.TrackDisplayTimer.Start();
        }

        // TODO: Rethink this method
        private void UpdateTrackDisplay(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (this.MusicPlayer.IsPlaying)
            {
                this.Dispatcher.BeginInvoke((Action)(() => this.LabelCurrentTrack.Content = this.MusicPlayer.TrackName));
                this.Dispatcher.BeginInvoke((Action)(() => this.ProgressBar.Value = this.MusicPlayer.TrackPosition));
                this.Dispatcher.BeginInvoke((Action)(() => this.ProgressBar.Maximum = this.MusicPlayer.TrackLength));
                this.Dispatcher.BeginInvoke((Action)(() => this.ButtonTogglePlay.Content = FindResource("appbar_control_pause")));
            }
            else
            {
                this.Dispatcher.BeginInvoke((Action)(() => this.LabelCurrentTrack.Content = string.Empty));
                this.Dispatcher.BeginInvoke((Action)(() => this.ProgressBar.Value = 0));
                this.Dispatcher.BeginInvoke((Action)(() => this.ProgressBar.Maximum = 1));
                this.Dispatcher.BeginInvoke((Action)(() => this.ButtonTogglePlay.Content = FindResource("appbar_control_play")));
            }
        }

        private void OnUpdateStopped(object sender, MapChangeEventArgs e)
        {
            this.Dispatcher.BeginInvoke((Action)(() => this.Title = "Unknown Location"));
            this.Dispatcher.BeginInvoke((Action)(() => this.TabControl.SelectedItem = this.TabStartup));
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
                this.MusicPlayer.SetPlaylist(null);
                this.LastMapId = currentMapId;
                this.MusicPlayer.Stop(); // should have already faded out at this point, we release resources this way
                this.MusicPlayer.SetPlaylist(this.MusicProvider.GetMapMusic(mapName));
                this.MusicPlayer.ShufflePlaylist();
                this.MusicPlayer.StartPlaylist();
                this.Dispatcher.BeginInvoke((Action)(() => this.MusicPlayer.Volume = this.SliderVolume.Value / 100));
            }

            if (this.MusicPlayer.GetPlaylistCount() == 0)
            {
                this.Dispatcher.BeginInvoke((Action)(() => this.TabControl.SelectedItem = this.TabNoTracks));
            }
            else
            {
                this.Dispatcher.BeginInvoke((Action)(() => this.TabControl.SelectedItem = this.TabPlayer));

                if (this.VisualsProvider.HasVisuals())
                {
                    this.Dispatcher.BeginInvoke((Action)(() => this.ImageVisual.Source = new BitmapImage(new Uri(this.VisualsProvider.GetRandomImageFile()))));
                }
            }

        }
        
        private void OnWindowClosed(object sender, EventArgs e)
        {
            this.MusicPlayer.Stop();
            this.Monitor.Stop();
            this.DelayedUpdateStartTimer.Stop();
            this.TrackDisplayTimer.Stop();
        }

        private void OnTogglePlayPause(object sender, RoutedEventArgs e)
        {
            //FocusManager.SetFocusedElement(FocusManager.GetFocusScope(this.ButtonTogglePlay), null);
            Keyboard.ClearFocus();

            if (this.MusicPlayer.IsPlaying)
            {
                this.MusicPlayer.Pause();
                
            }
            else
            {
                this.MusicPlayer.Resume();
                this.TrackDisplayTimer.Start();
            }
        }

        private void OnSkipToNextTrack(object sender, RoutedEventArgs e)
        {
            this.MusicPlayer.StartNextTrack();
            this.TrackDisplayTimer.Start();
        }

        private void OnSkipToPreviousTrack(object sender, RoutedEventArgs e)
        {
            this.MusicPlayer.StartPreviousTrack();
            this.TrackDisplayTimer.Start();
        }

        private void OnSliderVolumeChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(this.MusicPlayer != null)
            {
                this.MusicPlayer.Volume = e.NewValue/100;
            }
        }

        private void OnGoToMusicFolder(object sender, RoutedEventArgs e)
        {
            string musicFolderPath = System.IO.Path.GetFullPath(this.MusicProvider.RootMusicFolder);
            Process.Start(musicFolderPath);
        }

        private void OnGoToMapMusicFolder(object sender, RoutedEventArgs e)
        {

            string mapName = this.MapService.ResolveName(this.Monitor.GetCurrentMap());
            string musicRootFolderPath = System.IO.Path.GetFullPath(this.MusicProvider.RootMusicFolder);
            string musicFolderPath = musicRootFolderPath + "\\" + mapName;

            try
            {
                if (!Directory.Exists(musicFolderPath))
                {
                    Directory.CreateDirectory(musicFolderPath);
                }

                Process.Start(musicFolderPath);
            }
            catch (Exception e2)
            {
                Process.Start(musicRootFolderPath);
            }
            
        }
    }
}
