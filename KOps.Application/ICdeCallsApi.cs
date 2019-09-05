using System;
using System.Collections.Generic;
using System.Text;

namespace KOps.Application
{
    public interface ICdeCallsApi
    {
        System.Threading.Tasks.Task AcquireFloor();
        System.Threading.Tasks.Task MakeGroupCallAsync(string groupId);
        System.Threading.Tasks.Task ReleaseFloor();
    }
}
