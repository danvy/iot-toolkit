namespace Danvy.Services
{
    public interface ISettingsService
    {
        T Read<T>(string key);
        T Read<T>(string key, T defaultValue);
        void Write<T>(string key, T value);
    }
}
