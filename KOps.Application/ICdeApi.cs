using System;
using System.Collections.Generic;
using System.Text;

namespace KOps.Application
{
    public interface ICdeApi
    {
        ICdeCallsApi Calls { get; }

        System.Threading.Tasks.Task LoginAsync(string id);
    }
}
