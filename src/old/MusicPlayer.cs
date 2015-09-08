using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NAudio;
using NAudio.Wave;
using NVorbis.NAudioSupport;
using Newtonsoft.Json;

namespace gw2lam
{
    public class MusicPlayer
    {

        private const float FADE_OUT_STEP = 0.03f;
        private enum TargetState { PLAY, FADEOUT, STOP, NOTHING }

        private IWavePlayer waveOutDevice;
        private AudioFileReader audioFileReader;
        private VorbisFileReader vorbisFileReader;
        private TargetState targetState;

        public List<String> Playlist { get; set; }

        public string CurrentPosition
        {
            get
            {
                if (audioFileReader != null)
                {
                    return audioFileReader.CurrentTime.ToString("m\\:ss");
                }
                else if (vorbisFileReader != null)
                {
                    return vorbisFileReader.CurrentTime.ToString("m\\:ss");
                }
                else
                {
                    return "0:00";
                }
            }
        }

        public string CurrentLength
        {
            get
            {
                if (audioFileReader != null)
                {
                    return audioFileReader.TotalTime.ToString("m\\:ss");
                }
                else if (vorbisFileReader != null)
                {
                    return vorbisFileReader.TotalTime.ToString("m\\:ss");
                }
                else
                {
                    return "0:00";
                }
            }
        }

        public string TargetAudioFile { get; private set; }
        public float Volume
        {
            get
            {
                if (audioFileReader != null)
                {
                    return audioFileReader.Volume;
                }
                else if (vorbisFileReader != null && waveOutDevice != null)
                {
                    return waveOutDevice.Volume;
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
                return this.audioFileReader != null || this.vorbisFileReader != null;
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
                else if (vorbisFileReader != null)
                {

                    if (this.waveOutDevice.Volume - FADE_OUT_STEP < 0)
                    {
                        this.waveOutDevice.Volume = 0;
                        this.StopAudio();
                    }
                    else
                    {
                        this.waveOutDevice.Volume -= FADE_OUT_STEP;
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
           else if (vorbisFileReader != null && vorbisFileReader.Position == vorbisFileReader.Length)
           {
               StopAudio();
           }
           
        }

        public void PlayRandomTrack()
        {
            Random random = new Random();
            this.TargetAudioFile = this.Playlist[random.Next(this.Playlist.Count)];

            this.waveOutDevice = new WaveOut();

            if (this.TargetAudioFile.EndsWith(".mp3") || this.TargetAudioFile.EndsWith(".wav"))
            {
                this.audioFileReader = new AudioFileReader(this.TargetAudioFile);
                this.waveOutDevice.Init(audioFileReader);
            }
            else if (this.TargetAudioFile.EndsWith(".ogg"))
            {
                this.vorbisFileReader = new VorbisFileReader(this.TargetAudioFile);
                this.waveOutDevice.Init(vorbisFileReader);
                
            }

            this.waveOutDevice.Volume = 1.0f; // Unfortunately, VorbisFileReader does not have volume control
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
            
            if (vorbisFileReader != null)
            {
                vorbisFileReader.Dispose();
                vorbisFileReader = null;
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
