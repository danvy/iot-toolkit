using Windows.Storage;

namespace Danvy.Services
{
    public class CoreSettingsService : ISettingsService
    {
        public T Read<T>(string key)
        {
            return Read<T>(key, default(T));
        }
        public T Read<T>(string key, T defaultValue)
        {
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(key))
            {
                return (T)ApplicationData.Current.LocalSettings.Values[key];
            }
            else
                return defaultValue;
        }
        public void Write<T>(string key, T value)
        {
            ApplicationData.Current.LocalSettings.Values[key] = value;
        }
    }
}
