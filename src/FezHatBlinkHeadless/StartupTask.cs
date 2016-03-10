using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using GHIElectronics.UWP.Shields;
using Windows.System.Threading;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace FezHatBlinkHeadless
{
    public sealed class StartupTask : IBackgroundTask
    {
        private FEZHAT _hat;
        private bool _LedSwitch = false;
        BackgroundTaskDeferral _deferral;
        private ThreadPoolTimer _timer;
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();
            _hat = await FEZHAT.CreateAsync();
            _timer = ThreadPoolTimer.CreatePeriodicTimer(Timer_Tick, TimeSpan.FromMilliseconds(1000));
        }
        private void Timer_Tick(ThreadPoolTimer timer)
        {
            _LedSwitch = !_LedSwitch;
            _hat.D2.Color = _LedSwitch ? FEZHAT.Color.Green : FEZHAT.Color.Black;
            _hat.D3.Color = _LedSwitch ? FEZHAT.Color.Black : FEZHAT.Color.Blue;
        }
    }
}
