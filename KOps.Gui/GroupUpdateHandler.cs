using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KOps.Application;
using MediatR;

namespace KOps.Gui
{
    public class GroupUpdateHandler : INotificationHandler<GroupUpdate>
    {
        private readonly GroupsListViewModel groupsListViewModel;

        public GroupUpdateHandler(GroupsListViewModel groupsListViewModel)
        {
            this.groupsListViewModel = groupsListViewModel;
        }

        public Task Handle(GroupUpdate groupUpdate, CancellationToken cancellationToken)
        {
            this.groupsListViewModel.GroupUpdate(groupUpdate);
            return Task.CompletedTask;
        }
    }
}
