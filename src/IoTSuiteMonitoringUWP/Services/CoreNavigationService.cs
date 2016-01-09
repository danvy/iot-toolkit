using System;
using System.Linq;
using System.Reflection;
using Windows.UI.Xaml;

namespace Danvy.Services
{
    public class CoreNavigationService : INavigationService
    {
        public void Navigate<T>()
        {
            Navigate<T>(null);
        }
        public void Navigate<T>(object parameter)
        {
            var t = typeof(T);
            var assembly = t.GetTypeInfo().Assembly;
            var vmName = t.Name;
            var name = t.FullName.Replace("ViewModel", "View");
            t = Type.GetType(name);
            if (t == null)
            {
                var vName = vmName.Replace("ViewModel", "View");
                t = assembly.GetTypes().First((x) => { return x.Name == vName; });
                if (t == null)
                {
                    vName = vmName.Replace("ViewModel", "Page");
                    t = assembly.GetTypes().First((x) => { return x.Name == vName; });
                }
            }
            if (t == null)
                throw new Exception(string.Format("Can't find view for view model {0}", vmName));
            var frame = Window.Current.Content as Windows.UI.Xaml.Controls.Frame;
            if (frame != null)
                frame.Navigate(t, parameter);
        }
        public void GoBack()
        {
            var frame = Window.Current.Content as Windows.UI.Xaml.Controls.Frame;
            if (frame != null)
                frame.GoBack();
        }
        public bool CanGoBack()
        {
            var frame = Window.Current.Content as Windows.UI.Xaml.Controls.Frame;
            return frame != null ? frame.CanGoBack : false;
        }
    }
}
