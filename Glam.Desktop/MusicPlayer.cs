using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimerTask = System.Timers.Timer;
using System.Threading;
using System.Threading.Tasks;
using Glam;
using NAudio;
using NAudio.Wave;
using NAudio.Vorbis;

// Warning: older code - this needs to be rewritten to not use obsolete code
namespace Glam.Desktop
{
    public class MusicPlayer
    {
        private const float FadeOutStep = 0.01f;
        private enum Fade { Out, In, Disabled }

        private IWavePlayer AudioPlayer;
        private IWaveProvider AudioReader;
        private Fade FadeTarget;

        private TimerTask FadeTimer;

        private List<Music> Playlist;
        public int TrackIndex { get; set; }

        private float StartVolume;

        public double Volume {
            get
            {
                if(this.AudioPlayer != null)
                {
                    return this.AudioPlayer.Volume;
                }
                else
                {
                    return this.StartVolume;
                }
            }
            set
            {
                if(this.AudioPlayer != null)
                {
                    this.AudioPlayer.Volume = (float)value;
                }

                this.StartVolume = (float)value;
            }
        }

        public long TrackLength
        {
            get
            {
                if (this.AudioReader != null)
                {
                    if(this.AudioReader is AudioFileReader)
                    {
                        return ((AudioFileReader)this.AudioReader).Length;
                    }
                    else if (this.AudioReader is Mp3FileReader)
                    {
                        return ((Mp3FileReader)this.AudioReader).Length;
                    }
                    else if(this.AudioReader is VorbisWaveReader)
                    {
                        return ((VorbisWaveReader)this.AudioReader).Length;
                    }
                }
                
                return 0;
            }
        }

        public long TrackPosition
        {
            get
            {
                if (this.AudioReader != null)
                {
                    if (this.AudioReader is AudioFileReader)
                    {
                        return ((AudioFileReader)this.AudioReader).Position;
                    }
                    else if(this.AudioReader is Mp3FileReader)
                    {
                        return ((Mp3FileReader)this.AudioReader).Position;
                    }
                    else if (this.AudioReader is VorbisWaveReader)
                    {
                        return ((VorbisWaveReader)this.AudioReader).Position;
                    }
                }

                return 0;
            }
        }


        public string TrackName {
            get
            {
                if(this.Playlist != null && this.Playlist.Count() > 0 && this.TrackIndex >= 0)
                {
                    return this.Playlist[this.TrackIndex].Name;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public bool IsPlaying
        {
            get
            {
                return this.AudioPlayer != null && this.AudioPlayer.PlaybackState == PlaybackState.Playing;
            }
        }

        public MusicPlayer()
        {
            this.StartVolume = 1;
            this.FadeTarget = Fade.Disabled;
            this.FadeTimer = new System.Timers.Timer();
            this.FadeTimer.Interval = 150;
            this.FadeTimer.Elapsed += UpdateFade;
        }

        public void SetPlaylist(List<Music> playlist)
        {
            this.Playlist = playlist;
            this.TrackIndex = 0;
        }

        public void ShufflePlaylist()
        {
            if (this.Playlist != null)
            {
                Random r = new Random();
                this.Playlist = this.Playlist.OrderBy(m => r.Next()).ToList();
            }
        }

        public void StartPlaylist()
        {
            if (this.Playlist != null && this.Playlist.Count > 0)
            {
                this.TrackIndex = 0;
                this.StartTrack(this.Playlist[this.TrackIndex].Path);    
            }
        }

        public int GetPlaylistCount()
        {
            if(this.Playlist != null)
            {
                return this.Playlist.Count();
            }

            else
            {
                return 0;
            }
        }

        public void StartNextTrack()
        {
            this.TrackIndex++;
            if (this.Playlist != null && this.TrackIndex >= this.Playlist.Count())
            {
                this.TrackIndex = 0;
            }
            
            if (this.Playlist != null && this.Playlist.Count > 0)
            {
                this.StartTrack(this.Playlist[this.TrackIndex].Path);
            }
        }

        public void StartPreviousTrack()
        {
            this.TrackIndex--;
            if (this.TrackIndex < 0)
            {
                this.TrackIndex = this.Playlist.Count() - 1;
            }

            if (this.Playlist != null && this.Playlist.Count > 0)
            {
                this.StartTrack(this.Playlist[this.TrackIndex].Path);
            }
        }

        private object StartTrackLock = new object();

        private void StartTrack(string filePath)
        {
            lock (StartTrackLock)
            {
                this.Stop();

                try
                {

                    this.AudioPlayer = new WaveOutEvent();

                    filePath = filePath.ToLower();

                    if (filePath.EndsWith(".mp3"))
                    {
                        this.AudioReader = new Mp3FileReader(filePath);
                    }
                    else if (filePath.ToLower().EndsWith("wav"))
                    {
                        this.AudioReader = new AudioFileReader(filePath);
                    }
                    else if (filePath.EndsWith(".ogg"))
                    {
                        this.AudioReader = new VorbisWaveReader(filePath);
                    }

                    this.AudioPlayer.Init(this.AudioReader);
                    this.AudioPlayer.Volume = this.StartVolume; // Unfortunately, VorbisFileReader does not have volume control
                    this.AudioPlayer.Play();
                    this.AudioPlayer.PlaybackStopped += OnPlaybackStopped;
                    this.FadeTarget = Fade.Disabled;
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.StackTrace);
                }
            }
        }

        public void Resume()
        {
            if (this.AudioPlayer != null)
            {
                this.AudioPlayer.Play();
            }
        }

        public void FadeOut()
        {
            if (this.AudioPlayer != null)
            {
                this.FadeTimer.Stop();
                this.FadeTarget = Fade.Out;
                this.FadeTimer.Start();
            }
        }

        public void FadeIn()
        {
            if (this.AudioPlayer != null)
            {
                this.FadeTimer.Stop();
                this.FadeTarget = Fade.In;
                this.AudioPlayer.Play();
                this.FadeTimer.Start();
            }
        }

        public void Pause()
        {
            if (this.AudioPlayer != null)
            {
                this.AudioPlayer.Pause();
            }    
        }

        // OnPlayback stop, advance to the next track
        private void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            
            if(this.TrackPosition >= this.TrackLength)
            {
                this.StartNextTrack();
            }
            
        }

        // Used for fading out
        public void UpdateFade(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (this.AudioReader != null)
            {
                if (this.FadeTarget == Fade.Out)
                {
                    if (this.AudioPlayer.Volume - FadeOutStep < 0)
                    {
                        this.AudioPlayer.Volume = 0;
                        this.FadeTimer.Stop();
                        this.FadeTarget = Fade.Disabled;
                        this.Pause();
                    }
                    else
                    {
                        this.AudioPlayer.Volume -= FadeOutStep;
                    }
                    
                }
                else if (this.FadeTarget == Fade.In)
                {
                    if (this.AudioPlayer.Volume + FadeOutStep > this.StartVolume)
                    {
                        this.AudioPlayer.Volume = this.StartVolume;
                        this.FadeTarget = Fade.Disabled;
                    }
                    else
                    {
                        this.AudioPlayer.Volume += FadeOutStep;
                    }
                }
            }
            else
            {
                this.FadeTimer.Stop();
                this.FadeTarget = Fade.Disabled;
            }
        }

        public void Stop()
        {
            this.FadeTarget = Fade.Disabled;
            this.FadeTimer.Stop();

            if (AudioPlayer != null)
            {
                AudioPlayer.Stop();
            }

            if (AudioReader != null)
            {
                if (AudioReader is IDisposable)
                {
                    try
                    {
                        ((IDisposable)AudioReader).Dispose();
                    }
                    catch (Exception e)
                    {
                        System.Console.WriteLine(e.InnerException);
                    }
                }

                AudioReader = null;
            }

            if (AudioPlayer != null)
            {
                AudioPlayer.Dispose();
                AudioPlayer = null;
            }
            
        }
    }
}
