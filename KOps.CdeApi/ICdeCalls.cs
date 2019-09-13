using System.Threading.Tasks;

namespace KOps.CdeApi
{
    public interface ICdeCalls
    {
        Task AcquireFloor();
        Task MakeGroupCallAsync(string groupId);
        Task PttGroupsAsync(System.Collections.Generic.IEnumerable<string> groups);
        Task ReleaseFloor();
    }
}