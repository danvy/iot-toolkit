using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IoTSuiteLib
{
    public class DeviceCommand
    {
        private Dictionary<string, string> _parameterStore;

        public DeviceCommand()
        {
            Parameters = new List<DeviceCommandParameter>();
        }
        public string Name { get; set; }
        public string MessageId { get; set; }
        public string CreatedTime { get; set; }
        [JsonProperty(PropertyName="Parameters")]
        public Dictionary<string, string> ParametersStore
        {
            get
            {
                return _parameterStore;
            }
            set
            {
                if (value == _parameterStore)
                    return;
                _parameterStore = value;
                if (_parameterStore.Count > 0)
                {
                    var parameters = new List<DeviceCommandParameter>();
                    foreach (var parameter in _parameterStore)
                    {
                        parameters.Add(new DeviceCommandParameter() { Name = parameter.Key, Value = parameter.Value });
                    }
                    Parameters.AddRange(parameters);
                }
            }
        }
        [JsonIgnore]
        public List<DeviceCommandParameter> Parameters { get; set; }
    }
}
