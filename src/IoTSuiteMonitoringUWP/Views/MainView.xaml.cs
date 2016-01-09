using System;
using System.Diagnostics;
using System.Text;
using Danvy.Services;
using Danvy.Tools;
using GHIElectronics.UWP.Shields;
using IoTSuiteLib;
using Microsoft.Azure.Devices.Client;
using IoTSuiteMonitoring.Services;
using IoTSuiteMonitoring.ViewModels;
using Newtonsoft.Json;
using Windows.Foundation.Metadata;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace IoTSuiteMonitoring.Views
{
    public sealed partial class MainView : Page
    {
        private MainViewModel _viewModel;
        public MainViewModel ViewModel { get { return _viewModel ?? (_viewModel = new MainViewModel()); } }
        public MainView()
        {
            NavigationCacheMode = NavigationCacheMode.Required;
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
        }
        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.InitAsync();
        }
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (_viewModel != null)
                _viewModel.Stop();
            base.OnNavigatingFrom(e);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            if (_viewModel != null)
                _viewModel.Start();
        }
    }
}
