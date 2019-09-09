using System;
using System.Collections.Generic;
using System.Text;

namespace KOps.CdeApi
{
    public class CdeGroupStore
    {
        private Dictionary<string, CdeGroup> groups;

        public CdeGroupStore()
        {
            groups = new Dictionary<string, CdeGroup>();
        }

        public CdeGroup Get(string groupId)
        {
            if (!groups.TryGetValue(groupId, out var group))
            {
                group = new CdeGroup();
                groups.Add(groupId, group);
            }

            return group;
        }
    }
}
