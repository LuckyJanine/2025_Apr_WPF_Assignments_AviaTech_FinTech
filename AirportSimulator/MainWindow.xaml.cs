using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AirportSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }

    public class DoubleToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double flightDuration && flightDuration != 0.0)
            {
                return flightDuration.ToString(culture);
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string txtFlightDuration = value as string;
            txtFlightDuration = txtFlightDuration.Replace(',', '.');
            if (double.TryParse(txtFlightDuration, NumberStyles.Any, CultureInfo.InvariantCulture, out double flightDuration))
            {
                return flightDuration;
            }
            // tells WPF binding engine a conversion failuer for it to create a ValidationError object
            return DependencyProperty.UnsetValue;
            // DON'T use binding.donothing
        }
    }
}