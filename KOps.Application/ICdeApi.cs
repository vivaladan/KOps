using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KOps.Application
{
    public interface ICdeApi
    {
        Task AcquireFloor();

        Task LoginAsync(string id);

        Task MakeGroupCallAsync(string groupId);
        Task PttGroups(IEnumerable<string> groups);
        Task ReleaseFloor();
    }
}
