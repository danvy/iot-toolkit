using GHIElectronics.UWP.Shields;
using IoTSuiteMonitoring.Tools;

namespace IoTSuiteMonitoring.Services
{
    public class FezHatBoardService : IBoardService
    {
        private FEZHAT _hat = null;
        private FEZHAT.Color _lightColor = FEZHAT.Color.Blue;
        public FezHatBoardService()
        {
        }
        public void CheckHat()
        {
            if (_hat == null)
                _hat = FEZHAT.CreateAsync().Result;
        }
        public bool ButtonPressed
        {
            get
            {
                CheckHat();
                return _hat.IsDIO18Pressed() || _hat.IsDIO22Pressed();
            }
        }
        public double? ExternalTemperature
        {
            get
            {
                return null;
            }
        }
        public double Humidity
        {
            get
            {
                return 0;
            }
        }
        public LEDColor LightColor
        {
            get
            {
                CheckHat();
                var color = LEDColor.Black;
                if ((_hat.D2.Color.R > _hat.D2.Color.G) && (_hat.D2.Color.R > _hat.D2.Color.B))
                {
                    color = LEDColor.Red;
                }
                if ((_hat.D2.Color.G > _hat.D2.Color.R) && (_hat.D2.Color.G > _hat.D2.Color.B))
                {
                    color = LEDColor.Green;
                }
                if ((_hat.D2.Color.B > _hat.D2.Color.R) && (_hat.D2.Color.B > _hat.D2.Color.G))
                {
                    color = LEDColor.Blue;
                }
                return color;
            }
            set
            {
                CheckHat();
                FEZHAT.Color color;
                switch (value)
                {
                    case LEDColor.Green:
                        color = FEZHAT.Color.Green;
                        break;
                    case LEDColor.Red:
                        color = FEZHAT.Color.Red;
                        break;
                    case LEDColor.Blue:
                        color = FEZHAT.Color.Blue;
                        break;
                    default:
                        color = FEZHAT.Color.Black;
                        break;
                }
                _hat.D2.Color = color;
            }
        }
        public double Temperature
        {
            get
            {
                CheckHat();
                return _hat.GetTemperature();
            }
        }
    }
}
