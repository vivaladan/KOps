using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace KOps.Application
{
    public class GroupSelectionChangedHandler : INotificationHandler<GroupSelectionChanged>
    {
        private readonly TalkgroupHandler talkgroupHandler;

        public GroupSelectionChangedHandler(TalkgroupHandler talkgroupHandler)
        {
            this.talkgroupHandler = talkgroupHandler;
        }

        public Task Handle(
            GroupSelectionChanged notification,
            CancellationToken cancellationToken)
        {
            if (notification.Selected)
            {
                talkgroupHandler.Select(notification.Uri);
            }
            else
            {
                talkgroupHandler.Deselect(notification.Uri);
            }

            return Task.CompletedTask;
        }
    }
}
