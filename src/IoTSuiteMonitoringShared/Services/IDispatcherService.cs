using System;
using Windows.Foundation;

namespace Danvy.Services
{
    public interface IDispatcherService
    {
        IAsyncAction RunAsync(Action action);
    }
}
