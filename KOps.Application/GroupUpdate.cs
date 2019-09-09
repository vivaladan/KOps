using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace KOps.Application
{
    public class GroupUpdate : INotification
    {
        public string Uri { get; set; }

        public string Name { get; set; }

        public IEnumerable<GroupMemberUpdate> Members { get; set; }
    }
}
