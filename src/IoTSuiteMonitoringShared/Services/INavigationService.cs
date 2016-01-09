namespace Danvy.Services
{
    public interface INavigationService
    {
        void Navigate<T>();
        void Navigate<T>(object parameter);
        void GoBack();
        bool CanGoBack();
    }
}
