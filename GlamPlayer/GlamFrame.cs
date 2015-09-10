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
        public bool VolumeFadeEnabled { get; set; } // if true, volume will fade out on calls to StopPlayback() or fade in on calls to StartPlayback(), ResumePlayback()

        public void Initialize(string videoPlayerLocation)
        {
            this.Navigate(videoPlayerLocation);
            this.ObjectForScripting = this;
            this.VolumeFadeEnabled = false;
        }

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

        public void setSearchPlaylist(string searchTerms)
        {
            this.CallJavascript("setSearchPlaylist", searchTerms);
        }

        public string getCurrentVideoUrl()
        {
            return this.CallJavascript("getCurrentVideoUrl").ToString();
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

        public void SetFrameSize(int width, int height)
        {
            this.CallJavascript("setFrameSize", width, height);
        }

        public void OnStateChange(string state)
        {
            this.Log("OnStateChange: " + state);
        }


        public void Log(string message)
        {
            //MessageBox.Show(message);
            System.Console.WriteLine(message + "\t" + DateTime.Now);
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

            return returnValue;

        }

    }
}
