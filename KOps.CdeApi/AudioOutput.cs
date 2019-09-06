using System;
using NAudio.Wave;

namespace KOps.CdeApi
{
    internal class AudioOutput
    {
        private readonly BufferedWaveProvider waveStream;        
        private readonly WaveOutEvent outputDevice;

        public AudioOutput()
        {
            var waveFormat = new WaveFormat(48000, 16, 2);
            waveStream = new BufferedWaveProvider(waveFormat);
            
            outputDevice = new WaveOutEvent();
            outputDevice.Init(waveStream);
        }

        internal void Append(byte[] data)
        {
            waveStream.AddSamples(data, 0, data.Length);
            outputDevice.Play();
        }
    }
}