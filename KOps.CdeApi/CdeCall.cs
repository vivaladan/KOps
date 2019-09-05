using System;
using System.Collections.Generic;
using System.Text;
using Com.Apdcomms.CdeApi.Calling;

namespace KOps.CdeApi
{
    public class CdeCall
    {
        public CdeCallStatusChangedEventArgs CallStatusChanged { get; internal set; }

        public CdeCallEventArgs IncomingCall { get; internal set; }

        public CdeCallInfoEventArgs CallInfo { get; internal set; }

        public bool Connected => 
            CallStatusChanged?.CallStatus == CdeCallStatus.Connected || 
            CallStatusChanged?.CallStatus == CdeCallStatus.FloorChange;

        public uint CallId => 
            IncomingCall?.CallId ?? 
            CallStatusChanged?.CallId ?? 
            CallInfo?.CallId ?? 
            0;
    }
}
