using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Glam;
using NAudio;
using NAudio.Wave;
using NAudio.Vorbis;

// Warning: older code - this needs to be rewritten to not use obsolete code
namespace Glam.Desktop
{
    public class DesktopMusicPlayer
    {
        private const float FadeOutStep = 0.03f;
        private enum TargetState { Play, Fadeout, Stop, Nothing }

        private IWavePlayer AudioOut;
        private IWaveProvider AudioFileReader;
        private TargetState Target;

        public List<Music> Playlist { get; set; }
        public int CurrentlyPlayingIndex { get; set; }

        public bool IsPlaying
        {
            get
            {
                return this.AudioOut != null && this.AudioOut.PlaybackState == PlaybackState.Playing;
            }
        }

        public DesktopMusicPlayer()
        {
            this.Target = TargetState.Nothing;
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
            this.Stop();

            if (this.Playlist == null || this.Playlist.Count == 0)
            {
                return;
            }

            this.CurrentlyPlayingIndex = 0;
            this.StartTrack(this.Playlist[this.CurrentlyPlayingIndex].Path);

        }

        private void StartTrack(string filePath)
        {
            this.AudioOut = new WaveOutEvent();

            if (filePath.EndsWith(".mp3") || filePath.EndsWith(".wav"))
            {
                this.AudioFileReader = new AudioFileReader(filePath);
            }
            else if (filePath.EndsWith(".ogg"))
            {
                this.AudioFileReader = new VorbisWaveReader(filePath);
            }

            this.AudioOut.Init(this.AudioFileReader);
            this.AudioOut.Volume = 1.0f; // Unfortunately, VorbisFileReader does not have volume control
            this.AudioOut.Play();
            this.AudioOut.PlaybackStopped += OnPlaybackStopped;
            this.Target = TargetState.Nothing;
        }

        public void Resume()
        {
            if (this.AudioOut != null)
            {
                this.AudioOut.Play();
            }
        }

        public void Pause()
        {
            if (this.AudioOut != null)
            {
                this.AudioOut.Pause();
            }    
        }

        // OnPlayback stop, advance to the next track
        private void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            this.CurrentlyPlayingIndex++;
            if (this.CurrentlyPlayingIndex >= this.Playlist.Count())
            {
                this.CurrentlyPlayingIndex = 0;
            }
            
            this.Stop();
            this.StartTrack(this.Playlist[this.CurrentlyPlayingIndex].Path);
        }

        // Used for fading out
        public void Update()
        {
            if (this.Target == TargetState.Fadeout)
            {
                if (this.AudioFileReader != null)
                {
                    if (this.AudioOut.Volume - FadeOutStep < 0)
                    {
                        this.AudioOut.Volume = 0;
                        this.Target = TargetState.Nothing;
                        this.Pause();
                    }
                    else
                    {
                        this.AudioOut.Volume -= FadeOutStep;
                    }
                }
                else
                {
                    this.Target = TargetState.Nothing;
                    this.Pause();
                }
            }
        }

        // Only works if Update() is being called in the main loop
        public void FadeStop()
        {
            if (this.IsPlaying)
            {
                this.Target = TargetState.Fadeout;
            }
        }

        public void Stop()
        {
            if (AudioOut != null && AudioOut.PlaybackState != PlaybackState.Stopped)
            {
                AudioOut.Stop();
            }
            
            if (AudioFileReader is IDisposable && AudioFileReader != null)
            {
                ((IDisposable)AudioFileReader).Dispose();
                AudioFileReader = null;    
            }

            if (AudioOut != null)
            {
                AudioOut.Dispose();
                AudioOut = null;
            }

            this.Target = TargetState.Nothing;
        }

    }
}
