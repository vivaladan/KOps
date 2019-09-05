using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KOps.Application;
using MediatR;

namespace KOps.Gui
{
    public class CallUpdateHandler : INotificationHandler<CallUpdate>
    {
        private readonly CallsListViewModel callsListViewModel;

        public CallUpdateHandler(CallsListViewModel callsListViewModel)
        {
            this.callsListViewModel = callsListViewModel;
        }

        public Task Handle(CallUpdate callUpdate, CancellationToken cancellationToken)
        {
            this.callsListViewModel.CallUpdate(callUpdate);
            return Task.CompletedTask;
        }
    }
}
