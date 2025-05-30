using FlowLedger.Enums;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace FlowLedger
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

        private void btnAddCategory_Click(object sender, RoutedEventArgs e)
        {
            if ((DataContext is MainViewModel mv) && (sender is Button))
            {
                string categoryInput = txtCategoryName.Text;
                if (!string.IsNullOrWhiteSpace(categoryInput))
                {
                    string categoryToAdd = char.ToUpper(categoryInput[0]) + categoryInput.Substring(1).ToLower();
                    var (success, error) = mv.AddNewCategory(categoryToAdd);
                    if (!success)
                    {
                        MessageBox.Show($"Adding Failed.\n{error}");
                    } else
                    {
                        mv.NewCategoryToAdd = string.Empty;
                    }
                }
                else
                {
                    MessageBox.Show("Please type a new Category to add.");
                }
            }
        }

        private void btnConfirmTransaction_Click(object sender, RoutedEventArgs e)
        {
            if ((DataContext is MainViewModel mv) && (sender is Button))
            {
                mv.ConfirmTransaction();
            }
        }

        private void tbcCategoryType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((DataContext is MainViewModel mv) && (sender is TabControl))
            {
                if (tbcCategoryType.SelectedItem is TabItem selectedTab)
                {
                    string header = selectedTab.Header?.ToString() ?? string.Empty;
                    switch (header)
                    {
                        case "Income":
                            mv.SelectedTransactionType = Enums.TransactionType.Revenue;
                            break;
                        case "Expense":
                            mv.SelectedTransactionType = Enums.TransactionType.Spend;
                            break;
                    }
                }
            }
        }

        // probably better to use StyleTrigger in view rather than Message Queue (?)
        //private void cmbSelectedMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if ((DataContext is MainViewModel mv) && (sender is ComboBox))
        //    {
        //        Dispatcher.BeginInvoke(new Action(() =>
        //        {
        //            var selectedMonth = mv.SelectedMonth;
        //            if (mv.IsDeficit)
        //            {
        //                txtIsDeficit.Text = "Deficit:";
        //            } else
        //            {
        //                txtIsDeficit.Text = "Surplus:";
        //            }
        //        }), System.Windows.Threading.DispatcherPriority.Background);
        //    }
        //}
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
                return new ValidationResult(false, "* Required");
            }
        }
    }

    public class ValueWithoutSpecialChars : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string str = value as string;
            bool validInput = Regex.IsMatch(str, @"^[a-zA-Z0-9 _-]*$");

            if (!string.IsNullOrEmpty(str) && validInput)
            {
                return ValidationResult.ValidResult;
            }
            else if (string.IsNullOrEmpty(str))
            {
                return ValidationResult.ValidResult;
            }
            else
            {
                return new ValidationResult(false, "No special char.");
            }
        }
    }

    public class BoolToTextBlockVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            (bool)value ? Visibility.Visible : Visibility.Collapsed;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            (Visibility)value == Visibility.Visible;
    }

    public class TransactionTypeToColorConverter : IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TransactionType type)
            {
                if (type == TransactionType.Revenue)
                {
                    // return Brushes.Blue;
                    var style = new Style(typeof(TextBlock));
                    style.Setters.Add(new Setter(TextBlock.FontSizeProperty, 14.0));
                    style.Setters.Add(new Setter(TextBlock.ForegroundProperty, Brushes.Blue));
                    style.Setters.Add(new Setter(TextBlock.FontWeightProperty, FontWeights.SemiBold));
                    style.Setters.Add(new Setter(TextBlock.FontStyleProperty, FontStyles.Italic));
                    return style;
                }
            }
            return DependencyProperty.UnsetValue;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotSupportedException();
    }
}