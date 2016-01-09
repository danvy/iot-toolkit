using System;
using Windows.Foundation;
using Windows.UI.Core;

namespace Danvy.Services
{
    public class CoreDispatcherService : IDispatcherService
    {
        public static CoreDispatcher _dispatcher;
        public IAsyncAction RunAsync(Action action)
        {
            if (_dispatcher == null)
                _dispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;
            return _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
        }
    }
}
