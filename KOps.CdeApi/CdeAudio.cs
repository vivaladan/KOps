using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Com.Apdcomms.CdeApi;
using Com.Apdcomms.CdeApi.Audio;
using MediatR;
using Microsoft.Extensions.Logging;

namespace KOps.CdeApi
{
    public class CdeAudio
    {
        private readonly ILogger<CdeApi> logger;
        private readonly ICde cde;
        private readonly IMediator mediator;
        private readonly AudioInput audioInput;
        private Dictionary<uint, AudioOutput> channels = new Dictionary<uint, AudioOutput>();

        public CdeAudio(ILogger<CdeApi> logger, ICde cde, IMediator mediator)
        {
            this.logger = logger;
            this.cde = cde;
            this.mediator = mediator;

            this.audioInput = new AudioInput();
            audioInput.DataAvailable += AudioInput_DataAvailable;

            cde.AudioPlaybackFrameAvailable += Cde_AudioPlaybackFrameAvailable;
        }

        internal void StartCapture()
        {
            audioInput.Start();
        }

        internal void StopCapture()
        {
            audioInput.Stop();
        }

        private void AudioInput_DataAvailable(object sender, byte[] e)
        {
            logger.LogInformation("Sending samples as {@Bytes}", e.Length);

            cde.CaptureAudioFrame(e);
        }

        private void Cde_AudioPlaybackFrameAvailable(object sender, AudioPlaybackEventArgs e)
        {
            logger.LogInformation("Received samples for channel {@ChannelId} as {@Bytes}", e.ChannelId, e.Data.Length);

            if (!channels.TryGetValue(e.ChannelId, out var channel))
            {
                channel = new AudioOutput();
                channels.Add(e.ChannelId, channel);
            }

            channel.Append(e.Data);
        }
    }
}
