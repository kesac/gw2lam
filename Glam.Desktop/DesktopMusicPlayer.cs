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
    public class DesktopMusicPlayer
    {
        private const float FadeOutStep = 0.03f;
        private enum Fade { Out, In, Disabled }

        private IWavePlayer AudioOut;
        private IWaveProvider AudioFile;
        private Fade FadeTarget;

        private TimerTask FadeTimer;

        public List<Music> Playlist { get; set; }
        public int TrackIndex { get; set; }

        public long TrackLength
        {
            get
            {
                if (this.IsPlaying && this.TrackIndex >= 0)
                {
                    if(this.AudioFile is AudioFileReader)
                    {
                        return ((AudioFileReader)this.AudioFile).Length;
                    }
                    else if(this.AudioFile is VorbisWaveReader)
                    {
                        return ((VorbisWaveReader)this.AudioFile).Length;
                    }
                }
                
                return 0;
            }
        }

        public long TrackPosition
        {
            get
            {
                if (this.IsPlaying && this.TrackIndex >= 0)
                {
                    if (this.AudioFile is AudioFileReader)
                    {
                        return ((AudioFileReader)this.AudioFile).Position;
                    }
                    else if (this.AudioFile is VorbisWaveReader)
                    {
                        return ((VorbisWaveReader)this.AudioFile).Position;
                    }
                }

                return 0;
            }
        }


        public string TrackName {
            get
            {
                if(this.IsPlaying && this.TrackIndex >= 0)
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
                return this.AudioOut != null && this.AudioOut.PlaybackState == PlaybackState.Playing;
            }
        }

        public DesktopMusicPlayer()
        {
            this.FadeTarget = Fade.Disabled;
            this.FadeTimer = new System.Timers.Timer();
            this.FadeTimer.Interval = 50;
            this.FadeTimer.Elapsed += UpdateFade;
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

        public void StartNextTrack()
        {
            this.TrackIndex++;
            if (this.TrackIndex >= this.Playlist.Count())
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

        private void StartTrack(string filePath)
        {
            try
            {
                this.Stop();
                this.AudioOut = new WaveOutEvent();

                if (filePath.EndsWith(".mp3") || filePath.EndsWith(".wav"))
                {
                    this.AudioFile = new AudioFileReader(filePath);
                }
                else if (filePath.EndsWith(".ogg"))
                {
                    this.AudioFile = new VorbisWaveReader(filePath);
                }

                this.AudioOut.Init(this.AudioFile);
                this.AudioOut.Volume = 1.0f; // Unfortunately, VorbisFileReader does not have volume control
                this.AudioOut.Play();
                this.AudioOut.PlaybackStopped += OnPlaybackStopped;
                this.FadeTarget = Fade.Disabled;
            }
            catch(Exception e)
            {
                this.Stop();
                System.Console.WriteLine(e.StackTrace);
            }
        }

        public void Resume()
        {
            if (this.AudioOut != null)
            {
                this.AudioOut.Play();
            }
        }

        public void FadeOut()
        {
            if (this.AudioOut != null)
            {
                this.FadeTimer.Stop();
                this.FadeTarget = Fade.Out;
                //this.AudioOut.Volume = 1;
                this.AudioOut.Play();
                this.FadeTimer.Start();
            }
        }

        public void FadeIn()
        {
            if (this.AudioOut != null)
            {
                this.FadeTimer.Stop();
                this.FadeTarget = Fade.In;
                //this.AudioOut.Volume = 0;
                this.AudioOut.Play();
                this.FadeTimer.Start();
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
            this.StartNextTrack();
        }

        // Used for fading out
        public void UpdateFade(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (this.AudioFile != null)
            {
                if (this.FadeTarget == Fade.Out)
                {
                    if (this.AudioOut.Volume - FadeOutStep < 0)
                    {
                        this.AudioOut.Volume = 0;
                        this.FadeTimer.Stop();
                        this.FadeTarget = Fade.Disabled;
                        this.Pause();
                    }
                    else
                    {
                        this.AudioOut.Volume -= FadeOutStep;
                    }
                    
                }
                else if (this.FadeTarget == Fade.In)
                {
                    if (this.AudioOut.Volume + FadeOutStep > 1)
                    {
                        this.AudioOut.Volume = 1;
                        this.FadeTarget = Fade.Disabled;
                    }
                    else
                    {
                        this.AudioOut.Volume += FadeOutStep;
                    }
                }
            }
            else
            {
                this.FadeTimer.Stop();
                this.FadeTarget = Fade.Disabled;
            }
        }

        // Only works if Update() is being called in the main loop
        public void FadeStop()
        {
            if (this.IsPlaying)
            {
                this.FadeTarget = Fade.Out;
            }
        }

        public void Stop()
        {
            if (AudioOut != null)
            {
                AudioOut.Stop();
            }
            
            if (AudioFile != null)
            {
                if(AudioFile is IDisposable)
                {
                    try
                    {
                        ((IDisposable)AudioFile).Dispose();
                    }
                    catch (Exception e)
                    {
                        System.Console.WriteLine(e.InnerException);
                    }
                }

                AudioFile = null;    
            }

            if (AudioOut != null)
            {
                AudioOut.Dispose();
                AudioOut = null;
            }

            this.FadeTarget = Fade.Disabled;
        }

    }
}
