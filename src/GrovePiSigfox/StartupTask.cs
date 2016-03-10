using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using Windows.System.Threading;
using System.Diagnostics;
using GrovePi;
using System.IO;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace GrovePiSigfox
{
    public sealed class StartupTask : IBackgroundTask
    {
        private ThreadPoolTimer _timer;
        BackgroundTaskDeferral _deferral;
        private RPISigfox _sigfox;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            _sigfox = new RPISigfox();
            _timer = ThreadPoolTimer.CreatePeriodicTimer(Timer_Tick, TimeSpan.FromSeconds(10));
            _deferral = taskInstance.GetDeferral();
        }
        private void SwitchLed(bool on)
        {
            //DeviceFactory.Build.Led(Pin.DigitalPin3).ChangeState(on ? GrovePi.Sensors.SensorStatus.On : GrovePi.Sensors.SensorStatus.Off);
        }
        private async void Timer_Tick(ThreadPoolTimer timer)
        {
            _timer.Cancel();
            SwitchLed(true);
            try
            {
                //var tempHumid = DeviceFactory.Build.TemperatureAndHumiditySensor(Pin.DigitalPin4, GrovePi.Sensors.TemperatureAndHumiditySensorModel.DHT11).TemperatureAndHumidity();
                //Debug.WriteLine(string.Format("Temperature={0} Humidity={1}", tempHumid.Temperature, tempHumid.Humidity));
                Int16 temp = 20; // Convert.ToInt16(tempHumid.Temperature);
                byte hum = 45; // Convert.ToByte(tempHumid.Humidity);
                using (var stream = new MemoryStream())
                {
                    using (var writer = new BinaryWriter(stream))
                    {
                        writer.Write(temp);
                        writer.Write(hum);
                        writer.Flush();
                    }
                    await _sigfox.SendAsync(stream.ToArray());
                }
            }
            finally
            {
                SwitchLed(false);
            }
        }
    }
}
