using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Turtlesort.Glam.Core;

namespace GlamPlayer
{
    [ComVisibleAttribute(true)]
    public class GlamFrame : WebBrowser
    {
        private readonly string EndedState = "0";

        private List<MusicTrack> Tracklist;
        private int CurrentTrackIndex;
        public bool VolumeFadeEnabled { get; set; }// if true, volume will fade out on calls to StopPlayback() or fade in on calls to StartPlayback(), ResumePlayback()

        public void Initialize(string videoPlayerLocation)
        {
            this.Navigate(videoPlayerLocation);
            this.ObjectForScripting = this;
            this.VolumeFadeEnabled = false;
        }

        public void SetMusicTracks(List<MusicTrack> tracks)
        {
            this.Tracklist = tracks;
            this.CurrentTrackIndex = 0;
        }

        public void StartPlayback()
        {
            if (this.Tracklist.Count > 0)
            {
                this.CurrentTrackIndex = 0;
                string trackId = this.Tracklist[this.CurrentTrackIndex].Id;

                if (this.VolumeFadeEnabled)
                {
                    this.FadeInPlay(trackId);
                }
                else
                {
                    this.Play(trackId);
                }
            }
        }

        public void ResumePlayback()
        {
            if (this.Tracklist.Count > 0) { 
                if (this.VolumeFadeEnabled)
                {
                    this.FadeInPlay();
                }
                else
                {
                    this.Play();
                }
            }
        }

        public void StopPlayback()
        {
            if (this.VolumeFadeEnabled)
            {
                this.FadeOutStop();
            }
            else
            {
                this.StopPlay();
            }
        }

        public void SetFrameSize(int width, int height)
        {
            this.ThreadAwareInvocation(delegate { this.Document.InvokeScript("setFrameSize", new object[] { width, height }); });
        }

        public void OnStateChange(string state)
        {
            if (state == EndedState)
            {
                if (this.Tracklist.Count == 1)
                {
                    string trackId = this.Tracklist[this.CurrentTrackIndex].Id;
                    this.FadeInPlay(trackId); // ie. loop
                }
                else // move to the next track, we use modulus to move back to the first track if we reach the end of the tracklist
                {
                    this.CurrentTrackIndex = (this.CurrentTrackIndex + 1) % this.Tracklist.Count;

                    string trackId = this.Tracklist[this.CurrentTrackIndex].Id;
                    this.FadeInPlay(trackId);
                }
            }
        }





        private void Play()
        {
            this.ThreadAwareInvocation(delegate { this.Document.InvokeScript("start"); });
        }


        private void Play(string id)
        {
            this.ThreadAwareInvocation(delegate { this.Document.InvokeScript("start", new object[] { id }); });
        }

        private void FadeInPlay()
        {
            this.ThreadAwareInvocation(delegate { this.Document.InvokeScript("fadeStart"); });
        }

        private void FadeInPlay(string id)
        {
            this.ThreadAwareInvocation(delegate { this.Document.InvokeScript("fadeStart", new object[] { id }); });
        }

        private void StopPlay()
        {
            this.ThreadAwareInvocation(delegate { this.Document.InvokeScript("stop"); });
        }

        private void FadeOutStop()
        {
            this.ThreadAwareInvocation(delegate { this.Document.InvokeScript("fadeStop"); });
        }

        private void ThreadAwareInvocation(Action action)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(action));
            }
            else
            {
                action();
            }
        }

    }
}
