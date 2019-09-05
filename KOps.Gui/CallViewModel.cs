using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using KOps.Application;

namespace KOps.Gui
{
    public class CallViewModel : ViewModel
    {
        private uint callId;
        private uint channelId;
        private string groupId;
        private string callStatus;
        private FloorStatus floorStatus;
        private string talker;
        private bool transmitting;
        private bool receiving;
        private Brush highlight;

        public uint CallId
        {
            get => callId;
            set => SetProperty(ref callId, value);
        }

        public uint ChannelId
        {
            get => channelId;
            set => SetProperty(ref channelId, value);
        }

        public string GroupId
        {
            get => groupId;
            set => SetProperty(ref groupId, value);
        }

        public string CallStatus
        {
            get => callStatus;
            set => SetProperty(ref callStatus, value);
        }

        public FloorStatus FloorStatus
        {
            get => floorStatus;
            set => SetProperty(ref floorStatus, value);
        }

        public string Talker
        {
            get => talker;
            set => SetProperty(ref talker, value);
        }

        public bool Transmitting
        {
            get => transmitting;
            set => SetProperty(ref transmitting, value);
        }

        public bool Receiving
        {
            get => receiving;
            set => SetProperty(ref receiving, value);
        }

        public Brush Highlight
        {
            get => highlight;
            set => SetProperty(ref highlight, value);
        }
    }
}
