using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace IoTSuiteLib
{
    public class DeviceCommandDefinition
    {
        public DeviceCommandDefinition()
        {
            Parameters = new List<DeviceCommandParameterDefinition>();
        }
        public DeviceCommandDefinition(string name) : this()
        {
            Name = name;
        }
        public DeviceCommandDefinition(string name, DeviceCommandParameterDefinition[] parameters) : this(name)
        {
            foreach (var parameter in parameters)
            {
                Parameters.Add(parameter);
            }
        }
        public string Name { get; set; }
        public List<DeviceCommandParameterDefinition> Parameters { get; set; }
    }
}
