using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace IoTSuiteMonitoring.Tools
{
    public class LEDColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var color = Colors.Black;
            switch((LEDColor)value)
            {
                case LEDColor.Blue:
                    color = Colors.Blue;
                    break;
                case LEDColor.Green:
                    color = Colors.Green;
                    break;
                case LEDColor.Red:
                    color = Colors.Red;
                    break;
            }
            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
