using System;
using System.Collections.Concurrent;
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
    public class CdeCallsThreadSafe : ICdeCalls
    {
        private BlockingCollection<CdeCall> cdeEvents =
            new BlockingCollection<CdeCall>(
                new ConcurrentQueue<CdeCall>());

        private readonly CdeCallStore callStore;
        private readonly ILogger logger;
        private readonly ICde cde;
        private readonly IMediator mediator;

        public CdeCallsThreadSafe(ILogger logger, ICde cde, IMediator mediator)
        {
            callStore = new CdeCallStore();

            cde.Calling.CallStatusChanged += (sender, e) =>
            {
                logger.LogInformation("[{EventName}] {@EventArgs}", "CallStatusChanged", e);

                cdeEvents.Add(new CdeCall { CallStatusChanged = e });
            };

            cde.Calling.IncomingCall += (sender, e) =>
            {
                logger.LogInformation("[{EventName}] {@EventArgs}", "IncomingCall", e);

                cdeEvents.Add(new CdeCall { IncomingCall = e });
            };

            cde.Calling.CallInfo += (sender, e) =>
            {
                logger.LogInformation("[{EventName}] {@EventArgs}", "CallInfo", e);

                cdeEvents.Add(new CdeCall { CallInfo = e });
            };
            this.logger = logger;
            this.cde = cde;
            this.mediator = mediator;

            Task.Run(StartEventProcessing);
        }

        public void StartEventProcessing()
        {
            while (!cdeEvents.IsCompleted)
            {
                var cdeEvent = cdeEvents.Take();
                ProcessCdeEvent(cdeEvent);
            }
        }

        private void ProcessCdeEvent(CdeCall cdeEvent)
        {
            var call = callStore.Get(cdeEvent.CallId);

            if (cdeEvent.IncomingCall != null)
            {
                call.IncomingCall = cdeEvent.IncomingCall;

                if (call.IncomingCall != null && call.CallInfo != null)
                {
                    Publish(call);
                }
            }

            if (cdeEvent.CallStatusChanged != null)
            {
                call.CallStatusChanged = cdeEvent.CallStatusChanged;

                if (call.IncomingCall != null) //  && call.CallInfo != null
                {
                    Publish(call);
                }
            }

            if (cdeEvent.CallInfo != null)
            {
                call.CallInfo = cdeEvent.CallInfo;

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

            if (call.CallStatusChanged.FloorStatus == CdeFloorStatus.Taken)
            {
                callUpdate.FloorStatus = FloorStatus.Taken;

                if (call.CallInfo != null)
                {
                    callUpdate.Talker = call.CallInfo.TalkerInfo.MemberIdentity.Name;
                }
            }
            else if (call.CallStatusChanged.FloorStatus == CdeFloorStatus.Granted)
            {
                callUpdate.FloorStatus = FloorStatus.Granted;
                callUpdate.Talker = "You";
            }
            else
            {
                callUpdate.FloorStatus = FloorStatus.Idle;
                callUpdate.Talker = null;
            }

            logger.LogInformation("[{EventName}] {@EventArgs}", "CallUpdate", callUpdate);
            mediator.Publish(callUpdate);
        }

        public async Task MakeGroupCallAsync(string groupId)
        {
            var result = await cde.Calling.MakeCall(
                new CdeGroupIdentity(groupId));

            cdeEvents.Add(
                new CdeCall
                {
                    IncomingCall = new CdeCallEventArgs(
                        result.CallID, 
                        result.ChannelID, 
                        new CdeCallInfo(
                            null, 
                            CdeCallDirection.Outgoing, 
                            CdeCallType.PreArrangedGroup, 
                            groupId))
                });
        }

        public async Task AcquireFloor()
        {
            foreach (var call in callStore.Connected)
            {
                await cde.Calling.AcquireFloor(call.CallId);
            }
        }

        public async Task ReleaseFloor()
        {
            foreach (var call in callStore.Connected)
            {
                await cde.Calling.ReleaseFloor(call.CallId);
            }
        }

        public async Task PttGroupsAsync(IEnumerable<string> groups)
        {
            foreach (var groupId in groups)
            {
                var call = callStore.Get(groupId);

                if (call == null)
                {
                    await MakeGroupCallAsync(groupId);
                }
                else
                {
                    await cde.Calling.AcquireFloor(call.CallId);
                }
            }
        }
    }
}
