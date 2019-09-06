using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Com.Apdcomms.CdeApi;
using Com.Apdcomms.CdeApi.Calling;
using KOps.Application;
using MediatR;
using Microsoft.Extensions.Logging;

namespace KOps.CdeApi
{
    public class CdeCalls
    {
        private readonly ILogger<CdeApi> logger;
        private readonly ICde cde;
        private readonly IMediator mediator;
        private readonly CdeCallStore calls;

        static readonly object _object = new object();

        public CdeCalls(ILogger<CdeApi> logger, ICde cde, IMediator mediator)
        {
            this.logger = logger;
            this.cde = cde;
            this.mediator = mediator;

            calls = new CdeCallStore();

            cde.Calling.CallStatusChanged += Calling_CallStatusChanged;
            cde.Calling.IncomingCall += Calling_IncomingCall;
            cde.Calling.CallInfo += Calling_CallInfo;
        }

        public async Task MakeGroupCallAsync(string groupId)
        {
            var result = await cde.Calling.MakeCall(new CdeGroupIdentity(groupId));
            var events = calls.Get(result.CallID);

            events.IncomingCall = new CdeCallEventArgs(
                result.CallID,
                result.ChannelID,
                new CdeCallInfo(
                    null,
                    CdeCallDirection.Outgoing,
                    CdeCallType.PreArrangedGroup,
                    groupId));

            Publish(events);
        }

        internal async Task AcquireFloor()
        {
            foreach (var call in calls.Connected)
            {
                await cde.Calling.AcquireFloor(call.CallId);
            }
        }

        internal async Task ReleaseFloor()
        {
            foreach (var call in calls.Connected)
            {
                await cde.Calling.ReleaseFloor(call.CallId);
            }
        }

        private void Calling_IncomingCall(object sender, CdeCallEventArgs e)
        {
            logger.LogInformation("[{EventName}] {@EventArgs}", "IncomingCall", e);

            // always first for incoming - never for outgoing

            lock (_object)
            {
                var events = calls.Get(e.CallId);

                events.IncomingCall = e;
            }
        }

        private void Calling_CallStatusChanged(object sender, CdeCallStatusChangedEventArgs e)
        {
            logger.LogInformation("[{EventName}] {@EventArgs}", "CallStatusChanged", e);

            // always get this next - both incoming and outgoing

            lock (_object)
            {
                var call = calls.Get(e.CallId);

                call.CallStatusChanged = e;

                if (call.IncomingCall != null && call.CallInfo != null)
                {
                    Publish(call);
                }
            }
        }

        private void Calling_CallInfo(object sender, CdeCallInfoEventArgs e)
        {
            logger.LogInformation("[{EventName}] {@EventArgs}", "CallInfo", e);

            // get this last - always for incoming but only for outgoing once someone else has taken the floor

            lock (_object)
            {
                var call = calls.Get(e.CallId);

                call.CallInfo = e;

                if (call.IncomingCall != null && call.CallStatusChanged != null)
                {
                    Publish(call);
                }
            }
        }

        private void Publish(CdeCall call)
        {            
            var callUpdate = new CallUpdate
            {
                CallId = call.IncomingCall.CallId,
                ChannelId = call.IncomingCall.ChannelId,
                GroupId = call.IncomingCall.Info.CalledPartyId,
                CallStatus = call.CallStatusChanged.CallStatus.ToString(),
            };

            //Requested = 0,
            //Released = 1,
            //Revoked = 2,
            //Granted = 3,
            //Denied = 4,
            //Taken = 5,
            //Idle = 6

            if (call.CallStatusChanged.FloorStatus == CdeFloorStatus.Taken)
            {
                callUpdate.Talker = call.CallInfo.TalkerInfo.MemberIdentity.Name;
                callUpdate.FloorStatus = FloorStatus.Taken;
            }
            else if (call.CallStatusChanged.FloorStatus == CdeFloorStatus.Granted)
            {                
                callUpdate.Talker = "You";
                callUpdate.FloorStatus = FloorStatus.Granted;
            }
            else
            {
                callUpdate.FloorStatus = FloorStatus.Idle;
                callUpdate.Talker = null;
            }

            logger.LogInformation("[{EventName}] {@EventArgs}", "CallUpdate", callUpdate);
            mediator.Publish(callUpdate);
        }
    }
}
