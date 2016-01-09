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
    public class DeviceMetaData
    {
        public DeviceMetaData()
        {
            Properties = new DeviceProperties();
            Commands = new List<DeviceCommandDefinition>();
            //CommandHistory = new List<DeviceCommandHistory>();
        }
        [JsonConverter(typeof(StringEnumConverter))]
        public DeviceMetaDataObjectType ObjectType { get; set; }
        public string Version { get; set; }
        [JsonProperty(PropertyName = "DeviceProperties")]
        public DeviceProperties Properties { get; set; }
        public List<DeviceCommandDefinition> Commands { get; set; }
        //public List<DeviceCommandHistory> CommandHistory { get; set; }
        public bool IsSimulatedDevice { get; set; }
    }
}
