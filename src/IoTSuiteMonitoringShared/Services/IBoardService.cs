using IoTSuiteMonitoring.Tools;

namespace IoTSuiteMonitoring.Services
{
    public interface IBoardService
    {
        double Temperature { get; }
        double Humidity { get; }
        double? ExternalTemperature { get; }
        bool ButtonPressed { get; }
        LEDColor LightColor { get; set; }
    }
}
