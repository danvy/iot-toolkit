using System.Windows.Input;
using Danvy.Services;
using Danvy.Tools;
using Danvy.ViewModels;

namespace IoTSuiteMonitoring.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private string _host = "DaIoTMonitoring";
        private string _deviceId = "8ca300ca-2405-4f5f-8a75-2f8ed4803a6a";
        private string _key = "fy4hWfdRitdk2vNMZFRlQw==";
        public static readonly SettingsViewModel Default = new SettingsViewModel();
        private RelayCommand _saveCommand;
        public SettingsViewModel()
        {
            Load();
        }
        public string Host
        {
            get
            {
                return _host;
            }

            set
            {
                if (value == _host)
                    return;
                _host = value;
                RaisePropertyChanged();
            }
        }

        public string DeviceId
        {
            get
            {
                return _deviceId;
            }
            set
            {
                if (value == _deviceId)
                    return;
                _deviceId = value;
                RaisePropertyChanged();
            }
        }
        public string Key
        {
            get
            {
                return _key;
            }
            set
            {
                if (value == _key)
                    return;
                _key = value;
                RaisePropertyChanged();
            }
        }
        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new RelayCommand(() =>
                {
                    Save();
                }));
            }
        }
        public void Load()
        {
            var settings = IoC.Instance.Resolve<ISettingsService>();
            DeviceId = settings.Read<string>("DeviceId", DeviceId);
            Host = settings.Read<string>("Host", Host);
            Key = settings.Read<string>("Key", Key);
        }
        public void Save()
        {
            var settings = IoC.Instance.Resolve<ISettingsService>();
            settings.Write<string>("DeviceId", DeviceId);
            settings.Write<string>("Host", Host);
            settings.Write<string>("Key", Key);
        }
    }
}
