using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace IoTSuiteLib
{
    public class DeviceCommandParameterDefinition
    {
        public DeviceCommandParameterDefinition()
        {

        }
        public DeviceCommandParameterDefinition(string name, DeviceCommandParameterType type)
        {
            Name = name;
            Type = type;
        }
        public string Name { get; set;  }
        [JsonConverter(typeof(StringEnumConverter))]
        public DeviceCommandParameterType Type { get; set; }
    }
}