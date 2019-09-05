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
            calls.TryGetValue(callId, out var call);

            if (call == null)
            {
                call = new CdeCall();
                calls.Add(callId, call);
            }

            return call;
        }
    }
}
