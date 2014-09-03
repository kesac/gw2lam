using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NAudio;
using NAudio.Wave;
using Newtonsoft.Json;

namespace gw2lam
{
    class MusicPlayer
    {

        private const float FADE_OUT_STEP = 0.03f;
        private enum TargetState { PLAY, FADEOUT, STOP, NOTHING }

        private IWavePlayer waveOutDevice;
        private AudioFileReader audioFileReader;
        private TargetState targetState;

        public string TargetAudioFile { get; private set; }
        public float Volume
        {
            get
            {
                if (audioFileReader != null)
                {
                    return audioFileReader.Volume;
                }
                else{
                    return 0;
                }
            }
        }

        public bool IsPlaying
        {
            get
            {
                return this.audioFileReader != null;
            }
        }

        public MusicPlayer()
        {
            this.TargetAudioFile = "";
            this.targetState = TargetState.NOTHING;
        }

        // Used for fading out
        public void Update()
        {
           if (this.targetState == TargetState.FADEOUT)
            {
                if (audioFileReader != null)
                {
                    this.audioFileReader.Volume -= FADE_OUT_STEP;
                    if (this.audioFileReader.Volume < 0)
                    {
                        this.audioFileReader.Volume = 0;
                        this.StopAudio();
                    }
                }
                else
                {
                    this.StopAudio();
                }
            }

           // No more data to read, ensure that the IsPlaying attribute will return false
           if (audioFileReader != null && !audioFileReader.HasData(1))
           {
               StopAudio();
           }
        }

        public void PlayAudio(String path)
        {
            this.TargetAudioFile = path;
            this.waveOutDevice = new WaveOut();
            this.audioFileReader = new AudioFileReader(this.TargetAudioFile);
            this.waveOutDevice.Init(audioFileReader);
            this.waveOutDevice.Play();
            this.targetState = TargetState.NOTHING;
        }

        // Only works if Update() is being called in the main loop
        public void FadeStop()
        {
            if (this.IsPlaying)
            {
                this.targetState = TargetState.FADEOUT;
            }
        }

        public void StopAudio()
        {
            TargetAudioFile = "";

            if (waveOutDevice != null)
            {
                waveOutDevice.Stop();
            }
            if (audioFileReader != null)
            {
                audioFileReader.Dispose();
                audioFileReader = null;
            }
            if (waveOutDevice != null)
            {
                waveOutDevice.Dispose();
                waveOutDevice = null;
            }
            this.targetState = TargetState.NOTHING;
        }

    }
}
