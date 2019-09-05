using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace KOps.Application
{
    //public class CallUpdateHandler : AsyncRequestHandler<CallUpdate>
    //{
    //    private readonly ILogger<CallUpdateHandler> logger;

    //    public CallUpdateHandler(ILogger<CallUpdateHandler> logger)
    //    {
    //        this.logger = logger;
    //    }

    //    protected override Task Handle(CallUpdate callUpdate, CancellationToken cancellationToken)
    //    {
    //        logger.LogInformation("[{EventName}] {@EventArgs}", "CallUpdateHandler", callUpdate);

    //        return Task.CompletedTask;
    //    }
    //}
}
