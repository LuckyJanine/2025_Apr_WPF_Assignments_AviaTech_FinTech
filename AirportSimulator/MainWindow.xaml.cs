using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

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

        private void btnAddFlight_Click(object sender, RoutedEventArgs e)
        {
            if ((DataContext is MainViewModel mv) && (sender is Button))
            {
                var (ok, err) = mv.QueueAirplaneForTakeoff();
                if (!ok && !string.IsNullOrEmpty(err))
                {
                    MessageBox.Show($"Can't queue: {err}");
                } else if (!ok)
                {
                    MessageBox.Show($"Can't queue: Uncategorized error.");
                } 
            }
        }
    }

    public class ValidationsToBtnEnableConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool? canAdd = values[0] as bool?;
            bool hasError = values.Skip(1).Any(e => e is bool b && b);
            return canAdd == true && !hasError;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }


    public class ValidationRuleStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var errors = value as ReadOnlyCollection<ValidationError>;

            if(errors.Any(e => e.RuleInError != null && !(e.ErrorContent is string))) // an error from ValidationRules
            {
                return Brushes.Red;
            }

            return Brushes.DarkGray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public class ErrorContentToMessageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string message)
                return message;

            if (value is ErrorFromGeneralValidationRule error)
                return error.ErrMessage;

            return "Uncategorized validation error.";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public class ValueIsNotNullOrEmpty : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string str = value as string;

            if (!string.IsNullOrEmpty(str))
            {
                return ValidationResult.ValidResult;
            }
            else
            {
                return new ValidationResult(false, new ErrorFromGeneralValidationRule
                {
                    ErrCode = 101,
                    ErrMessage = "* Required."
                });
            }
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

    public class ErrorFromGeneralValidationRule
    {
        public int ErrCode { get; set; }
        public string ErrMessage { get; set; }
    }
}