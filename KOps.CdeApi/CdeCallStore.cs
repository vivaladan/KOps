using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KOps.CdeApi
{
    public class CdeCallStore
    {
        private Dictionary<uint, CdeCall> calls;

        public CdeCallStore()
        {
            calls = new Dictionary<uint, CdeCall>();
        }

        public IEnumerable<CdeCall> Connected =>
            calls.Values.Where(call => call.Connected);

        public CdeCall Get(uint callId)
        {
            if (!calls.TryGetValue(callId, out var call))
            {
                call = new CdeCall();
                calls.Add(callId, call);
            }

            return call;
        }

        public CdeCall Get(string groupId)
        {
            foreach (var call in calls)
            {
                if (call.Value.IncomingCall.Info.CalledPartyId == groupId)
                {
                    if (call.Value.Connected)
                    {
                        return call.Value;
                    }
                }
            }

            return null;
        }
    }
}
