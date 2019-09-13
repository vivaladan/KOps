using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KOps.Application
{
    public class TalkgroupHandler
    {
        private List<string> selected = new List<string>();
        private readonly ICdeApi cdeApi;

        public TalkgroupHandler(ICdeApi cdeApi)
        {
            this.cdeApi = cdeApi;
        }

        public async Task AcquireFloor()
        {
            await cdeApi.PttGroups(selected);
        }

        public async Task ReleaseFloor()
        {
            await cdeApi.ReleaseFloor();
        }

        internal void Select(string uri)
        {
            if (!selected.Contains(uri))
            {
                selected.Add(uri);
            }
        }

        internal void Deselect(string uri)
        {
            if (selected.Contains(uri))
            {
                selected.Remove(uri);
            }
        }
    }
}
