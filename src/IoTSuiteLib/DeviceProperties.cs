using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace IoTSuiteLib
{
    public class DeviceProperties
    {
        public string DeviceID { get; set; }
        [JsonConverter(typeof(BoolToIntConverter))]
        public bool HubEnabledState { get; set; }
        public DateTime CreatedTime { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public DeviceState DeviceState { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string Manufacturer { get; set; }
        public string ModelNumber { get; set; }
        public string SerialNumber { get; set; }
        public string FirmwareVersion { get; set; }
        public string Platform { get; set; }
        public string Processor { get; set; }
        public string AvailablePowerSources { get; set; }
        public string PowerSourceVoltage { get; set; }
        public string BatteryLevel { get; set; }
        public string InstalledRAM { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }
}
