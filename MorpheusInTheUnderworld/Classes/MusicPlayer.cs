using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace MorpheusInTheUnderworld.Classes
{
    /// <summary>
    /// This is the game Special Music Player
    /// </summary>
    public class MusicPlayer
    {
        public delegate void EventHandler();
        private float masterVolume;
        private float musicVolume;
        private float sfxVolume;

        private FMOD.RESULT result;

        public FMOD.System system;
        public int PlayingSongID;
        public List<string> PlayList;
        private FMOD.Sound CurrentSong;

        private bool FFTEnabled { get; set; }
        private FMOD.DSP_PARAMETER_FFT fftParameter;
        public float higher_freq;
        public float[] spectrum;
        public float[] previousSpectrum;
        private FMOD.ChannelGroup ChannelGroup = new FMOD.ChannelGroup(IntPtr.Zero);

        private FMOD.Channel Channel = new FMOD.Channel(IntPtr.Zero);

        public FMOD.DSP MyDSP;
        public float MasterVolume { get { return masterVolume; } set { masterVolume = value; } }
        public float MusicVolume { get { return musicVolume; } set { musicVolume = value; } }
        public float SFXVolume { get { return sfxVolume; } set { sfxVolume = value; } }
        public int NUM_SONGS { get { if (PlayList != null) return PlayList.Count; return 0; } }

        public event EventHandler NextSongEvent;

        private float bpm_elapsed;
        private bool gotBeat;

        public MusicPlayer()
        {
            masterVolume = 1f;
            musicVolume = 1f;
            sfxVolume = 1f;
        }

        public void Initialize()
        {
            if (system == null)
            {
                result = FMOD.Factory.System_Create(out system);
                result = system.init(512, FMOD.INITFLAGS.NORMAL, IntPtr.Zero);
            }
            PlayList = new List<string>();
            ChannelGroup = new FMOD.ChannelGroup(IntPtr.Zero);
            Channel = new FMOD.Channel(IntPtr.Zero);

            spectrum = new float[512];
            previousSpectrum = spectrum;
            EnableFFT();
        }
        // Is optional to Update the Music Player
        // But doing so will enable features as BPM Detection
        public void Update(GameTime gameTime)
        {

            system.update();
            if (IsSongPlaying())
            {
                uint length = 0;
                uint position = 0;
                CurrentSong.getLength(out length, FMOD.TIMEUNIT.MS);
                Channel.getPosition(out position, FMOD.TIMEUNIT.MS);
                if (position == length)
                    NextSongEvent.Invoke();
            }


            // BPM Method 1 (More research)
            #region Update System & FFT Algorithm

            if (FFTEnabled)
            {
                // Perform a Fast Fourier Transform
                
                int windowSize = spectrum.Length; //Samples.
                IntPtr data;
                uint length;
                //hanning bpm
                MyDSP.setParameterInt((int)FMOD.DSP_FFT.WINDOWTYPE, (int)FMOD.DSP_FFT_WINDOW.HAMMING);
                MyDSP.setParameterInt((int)FMOD.DSP_FFT.WINDOWSIZE, windowSize);
                MyDSP.getParameterData((int)FMOD.DSP_FFT.SPECTRUMDATA, out data, out length);
                MyDSP.getParameterFloat((int)FMOD.DSP_FFT.DOMINANT_FREQ, out higher_freq);
                fftParameter = (FMOD.DSP_PARAMETER_FFT)Marshal.PtrToStructure(data, typeof(FMOD.DSP_PARAMETER_FFT));

                #region BPMDetection

                if (fftParameter.spectrum.Length != 0)
                {
                    for (int i = 0; i < windowSize; i++)
                    {
                        if (fftParameter.spectrum.Length == 2)
                            spectrum[i] = (fftParameter.spectrum[0][i] + fftParameter.spectrum[1][i]) / 2f;
                    }
                }
                previousSpectrum = spectrum;
                #endregion
            }
            #endregion

            // BPM Method 2 (Working! but with small sync bugs)

            float bpm = 117;
            float bps = (60f / bpm);
            bpm_elapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (bpm_elapsed > bps)
            {
                gotBeat = true;
                bpm_elapsed = 0f;
            }
            else
                gotBeat = false;
        }

        public void NextSong(bool loop)
        {
            Pause();
            if (PlayingSongID != PlayList.Count)
                LoadSong(PlayingSongID + 1, loop);
            else
                LoadSong(PlayingSongID = 0, loop);
            Play();
        }
        public void Play()
        {
            if (CurrentSong != null && !IsSongPlaying())
                Channel.setPaused(false);
        }
        public void Pause()
        {
            if (CurrentSong != null)
                Channel.setPaused(true);
        }
        public void Reset()
        {
            Channel.setPosition(0, FMOD.TIMEUNIT.MS);
        }

        public void Stop()
        {
            Reset();
            Pause();
        }
        public void PreviousSong(bool loop)
        {
            Pause();
            if (PlayingSongID != 0)
                LoadSong(PlayingSongID - 1, loop);
            else
                LoadSong(PlayingSongID = PlayList.Count-1, loop);
        }
        public void AddSong(string stream)
        {
            PlayList.Add(stream);
        }
        public void LoadSong(int id, bool loop)
        {
            PlayingSongID = id;
            if (CurrentSong != null)
            {
                CurrentSong.release();
            }
            FMOD.MODE mode = (loop) ? FMOD.MODE.LOOP_NORMAL : FMOD.MODE.DEFAULT;
            system.createStream(PlayList[id], mode, out CurrentSong);
            system.playSound(CurrentSong, ChannelGroup, false, out Channel);
        }

        public void EnableFFT()
        {
            FFTEnabled = true;
            system.getMasterChannelGroup(out ChannelGroup);
            system.createDSPByType(FMOD.DSP_TYPE.FFT, out MyDSP);
            ChannelGroup.addDSP(1, MyDSP);

            MyDSP.setActive(true);
        }

        public bool IsSongPlaying()
        {
            bool isPlaying = false;
            Channel.getPaused(out isPlaying);
            return !isPlaying;
        }

        public string FormatSongPosition() 
        {
            return FormatSongPosition(GetSongPosition());
        }
        public string FormatSongPosition(uint position)
        {
            string formattedPosition = String.Empty;
            TimeSpan time = TimeSpan.FromMilliseconds(position);
            formattedPosition = time.ToString(@"mm\:ss");
            return formattedPosition;

        }
        public uint GetSongPosition()
        {
            uint position = 0;
            Channel.getPosition(out position, FMOD.TIMEUNIT.MS);
            return position;
        }

        public void SetSongPosition(uint position)
        {
            Channel.setPosition(position, FMOD.TIMEUNIT.MS);
        }

        public uint GetSongLength()
        {
            uint position = 0;
            CurrentSong.getLength(out position, FMOD.TIMEUNIT.MS);
            return position;
        }
        public bool GotBeat()
        {
            return gotBeat;
        }
    }
}
