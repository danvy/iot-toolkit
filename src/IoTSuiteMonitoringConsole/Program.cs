using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Danvy.Tools;
using IoTSuiteLib;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

namespace IoTSuiteMonitoring
{
    class Program
    {
        private static bool _cancel;
        private static DeviceClient _client;
        private static string _deviceId;
        private static bool _telemetry = true;
        private static double _humidity = 50;
        private static double _temperature = 20;
        private static double? _externalTemperature = null;
        private static Timer _timer;
        private static string _lightColor;

        static void Main(string[] args)
        {
            Console.CancelKeyPress += Console_CancelKeyPress;
            Init();
            _timer = new Timer(Callback, null, TimeSpan.FromMilliseconds(0), TimeSpan.FromMilliseconds(250));
            while (!_cancel)
            {
                Thread.Sleep(100);
            }
        }

        private static async void Callback(object state)
        {
            var keyDown = false;
            if (Keyboard.IsKeyDown(VirtualKey.Numpad4) || Keyboard.IsKeyDown(VirtualKey.Down) || Keyboard.IsKeyDown(VirtualKey.Add))
            {
                _temperature--;
                keyDown = true;
            }
            if (Keyboard.IsKeyDown(VirtualKey.Numpad7) || Keyboard.IsKeyDown(VirtualKey.Up) || Keyboard.IsKeyDown(VirtualKey.Subtract))
            {
                _temperature++;
                keyDown = true;
            }
            else if (Keyboard.IsKeyDown(VirtualKey.Numpad5))
            {
                _humidity--;
                keyDown = true;
            }
            else if (Keyboard.IsKeyDown(VirtualKey.Numpad8))
            {
                _humidity++;
                keyDown = true;
            }
            else if (Keyboard.IsKeyDown(VirtualKey.Numpad6))
            {
                _externalTemperature--;
                keyDown = true;
            }
            else if (Keyboard.IsKeyDown(VirtualKey.Numpad9))
            {
                _externalTemperature++;
                keyDown = true;
            }
            if (keyDown)
                Console.WriteLine("Temperature={0}, Humidity={1}, External temperature={2}", _temperature, _humidity, _externalTemperature);
            if (DateTime.Now.Second % 5 != 0)
                return;
            if (_client == null)
                return;
            //Send message
            if (_telemetry)
            {
                var data = new DeviceMonitoringData();
                data.DeviceId = _deviceId;
                data.Humidity = _humidity;
                data.Temperature = _temperature;
                data.ExternalTemperature = _externalTemperature;
                var content = JsonConvert.SerializeObject(data);
                var msg = new Message(Encoding.UTF8.GetBytes(content));
                //await _client.SendEventAsync(message);
                await _client.SendEventAsync(msg);
            }
            //receive messages
            Message message;
            while ((message = await _client.ReceiveAsync()) != null)
            {
                var content = Encoding.ASCII.GetString(message.GetBytes());
                Console.WriteLine("{0}> Received message: {1}", DateTime.Now.ToLocalTime(), content);
                var command = JsonConvert.DeserializeObject<DeviceCommand>(content);
                if (command != null)
                {
                    if (command.Name == "SwitchLight")
                    {
                        SwitchLight();
                        await _client.CompleteAsync(message);
                    }
                    else if (command.Name == "LightColor")
                    {
                        if (command.Parameters.Count == 1)
                        {
                            _lightColor = command.Parameters[0].Value;
                            await _client.CompleteAsync(message);
                        }
                    }
                    else if (command.Name == "PingDevice")
                    {
                        Console.WriteLine("Ping");
                        await _client.CompleteAsync(message);
                    }
                    else if (command.Name == "StartTelemetry")
                    {
                        Console.WriteLine("Start telemetry");
                        _telemetry = true;
                        await _client.CompleteAsync(message);
                    }
                    else if (command.Name == "StopTelemetry")
                    {
                        Console.WriteLine("Stop telemetry");
                        _telemetry = false;
                        await _client.CompleteAsync(message);
                    }
                    else
                    {
                        await _client.RejectAsync(message);
                    }
                }
            }
        }

        private static void SwitchLight()
        {
            Console.WriteLine("Switch light");
        }
        private static void Init()
        {
            _deviceId = ConfigurationManager.AppSettings["DeviceId"];
            var host = ConfigurationManager.AppSettings["Host"];
            var key = ConfigurationManager.AppSettings["Key"];
            var connectionString = string.Format("HostName={0}.azure-devices.net;DeviceId={1};SharedAccessKey={2}",
                host, _deviceId, key);
            _client = DeviceClient.CreateFromConnectionString(connectionString, TransportType.Http1);
            var data = new DeviceMetaData();
            data.Version = "1.0";
            data.IsSimulatedDevice = false;
            data.Properties.DeviceID = _deviceId;
            data.Properties.FirmwareVersion = "1.42";
            data.Properties.HubEnabledState = true;
            data.Properties.Processor = "ARM";
            data.Properties.Platform = "UWP";
            data.Properties.SerialNumber = "1234567890";
            data.Properties.InstalledRAM = "1024 MB";
            data.Properties.ModelNumber = "007-BOND";
            data.Properties.Manufacturer = "Raspberry";
            //data.Properties.UpdatedTime = DateTime.UtcNow;
            data.Properties.DeviceState = DeviceState.Normal;
            data.Commands.Add(new DeviceCommandDefinition("SwitchLight"));
            data.Commands.Add(new DeviceCommandDefinition("LightColor",
                new DeviceCommandParameterDefinition[] {
                    new DeviceCommandParameterDefinition("Color", DeviceCommandParameterType.String),
                    new DeviceCommandParameterDefinition("Color2", DeviceCommandParameterType.String)
                }));
            data.Commands.Add(new DeviceCommandDefinition("PingDevice"));
            data.Commands.Add(new DeviceCommandDefinition("StartTelemetry"));
            data.Commands.Add(new DeviceCommandDefinition("StopTelemetry"));
            var content = JsonConvert.SerializeObject(data);
            _client.SendEventAsync(new Message(Encoding.UTF8.GetBytes(content)));
        }
        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            _cancel = true;
        }
    }
}
