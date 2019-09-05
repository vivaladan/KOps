using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace KOps.Application
{
    public class CallUpdate : INotification
    {
        public uint CallId { get; set; }
        public uint ChannelId { get; set; }
        public string GroupId { get; set; }
        public string CallStatus { get; set; }
        public FloorStatus FloorStatus { get; set; }
        public string Talker { get; set; }
    }
}
