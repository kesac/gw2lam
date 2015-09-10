using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace GlamPlayer
{
    [ComVisible(true)]
    public class GlamFrame : WebBrowser
    {

        public void Initialize(string videoPlayerLocation)
        {
            this.Navigate(videoPlayerLocation);
            this.ObjectForScripting = this;
        }

        public void Play(string id)
        {
            this.ThreadAwareInvocation(delegate { this.Document.InvokeScript("start", new object[] { id }); });
        }

        public void FadeInPlay(string id)
        {
            this.ThreadAwareInvocation(delegate { this.Document.InvokeScript("fadeStart", new object[] { id }); });
        }

        public void StopPlayback()
        {
            this.ThreadAwareInvocation(delegate { this.Document.InvokeScript("stop"); });
        }

        public void FadeOutStop()
        {
            this.ThreadAwareInvocation(delegate { this.Document.InvokeScript("fadeStop"); });
        }

        public void SetFrameSize(int width, int height)
        {
            this.ThreadAwareInvocation(delegate { this.Document.InvokeScript("setFrameSize", new object[] { width, height }); });
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
