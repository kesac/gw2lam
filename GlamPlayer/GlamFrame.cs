﻿using System;
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
        public bool VolumeFadeEnabled { get; set; } // if true, volume will fade out on calls to StopPlayback() or fade in on calls to StartPlayback(), ResumePlayback()
        public event GlamFrameReadyEventHandler OnReady;
        public event CuedEventHandler OnPlaylistCued;
        private bool IsReady; // will be true once the Javascript frame has finished loading and is ready to accept commands

        public void Initialize(string videoPlayerLocation)
        {
            this.Navigate(videoPlayerLocation);
            this.ObjectForScripting = this;
            this.VolumeFadeEnabled = false;
        }

        // Called by javascript when the iframe is ready
        public void OnFrameReady()
        {
            this.IsReady = true;
            if (OnReady != null)
            {
                OnReady();
            }
        }

        /**
         * Warning: this is an asynchronous event. The playlist must finish
         * loading before it can be played. When it something is ready to be played, the 
         * OnCued event is fired.
         */
        public void SetPlaylist(List<MusicTrack> tracks)
        {
            StringBuilder ids = new StringBuilder();

            foreach (MusicTrack track in tracks)
            {
                ids.Append(track.Id);
                ids.Append(",");
            }

            ids.Remove(ids.Length - 1, 1);
            this.CallJavascript("setPlaylist", ids.ToString());
        }

        /**
         * Warning: this is an asynchronous event. The playlist must finish
         * loading before it can be played. When it something is ready to be played, the 
         * OnCued event is fired.
         */
        public void SetPlaylistById(string playlistId)
        {
            this.Log("About to call javascript function setPlaylistById()");
            this.CallJavascript("setPlaylistById", playlistId);
            this.Log("Finished calling javascript function setPlaylistById()");
        }

        /**
         * Warning: this is an asynchronous event. The playlist must finish
         * loading before it can be played. When it something is ready to be played, the 
         * OnCued event is fired.
         */
        public void SetSearchPlaylist(string searchTerms)
        {
            this.CallJavascript("setSearchPlaylist", searchTerms);
        }

        public string getCurrentVideoUrl()
        {
            object result = this.CallJavascript("getCurrentVideoUrl");

            if (result != null)
            {
                return result.ToString();
            }
            else 
            { 
                return string.Empty;
            }
        }

        public void StartPlayback()
        {
            if (this.VolumeFadeEnabled)
            {
                this.FadeInPlay();
            }
            else
            {
                this.Play();
            }

        }

        public void PausePlayback()
        {
            if (this.VolumeFadeEnabled)
            {
                this.FadeOutPause();
            }
            else
            {
                this.Pause();
            }
        }

        public void StartNextTrack()
        {
            this.CallJavascript("next");
        }

        public void StartPreviousTrack()
        {
            this.CallJavascript("previous");
        }

        public void TogglePlayPause()
        {
            this.CallJavascript("togglePlayPause");
        }


        public void IncreaseVolume()
        {
            this.CallJavascript("increaseVolume");
        }

        public void DecreaseVolume()
        {
            this.CallJavascript("decreaseVolume");
        }

        public void SetFrameSize(int width, int height)
        {
            this.CallJavascript("setFrameSize", width, height);
        }

        public void OnStateChange(string state)
        {
            this.Log("OnStateChange:\t" + state);

            if (state == "5" && this.OnPlaylistCued != null)
            {
                this.OnPlaylistCued();
            }

        }


        public void Log(string message)
        {
            System.Console.WriteLine("GLAMFRAMELOG: \t" + DateTime.Now + "\t" + message);
        }

        private void Play()
        {
            this.CallJavascript("start");
        }

        private void FadeInPlay()
        {
            this.CallJavascript("fadeStart");
        }


        private void Pause()
        {
            this.CallJavascript("pause");
        }

        private void FadeOutPause()
        {
            this.CallJavascript("fadePause");
        }

        private object CallJavascript(string functionName, params object[] args)
        {
            object returnValue = null;
            if (this.IsReady) 
            { 
                Action action = delegate
                {
                    if (args.Length > 0) { 
                        returnValue = this.Document.InvokeScript(functionName, args);
                    }
                    else
                    {
                        returnValue = this.Document.InvokeScript(functionName);
                    }
                };

                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(action));
                }
                else
                {
                    action();
                }
            }
            return returnValue;

        }

    }
}
