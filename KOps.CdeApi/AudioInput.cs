using System;
using System.Collections.Generic;
using System.Text;
using NAudio.Wave;

namespace KOps.CdeApi
{
    internal class AudioInput
    {
        private readonly WaveInEvent waveSource;

        public AudioInput()
        {
            var waveFormat = new WaveFormat(48000, 16, 2);            
            waveSource = new WaveInEvent();

            waveSource.WaveFormat = waveFormat;
            waveSource.BufferMilliseconds = 20;

            waveSource.DataAvailable += WaveSource_DataAvailable;
        }

        private void WaveSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            DataAvailable?.Invoke(this, e.Buffer);
        }

        public void Start()
        {
            waveSource.StartRecording();
        }

        public void Stop()
        {
            waveSource.StopRecording();
        }

        public event EventHandler<byte[]> DataAvailable;
    }
}
