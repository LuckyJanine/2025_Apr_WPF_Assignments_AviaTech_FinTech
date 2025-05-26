using System.Windows;
using System.Windows.Controls;

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

        private void tbcCategoryType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((DataContext is MainViewModel mv) && (sender is TabControl))
            {
                if (tbcCategoryType.SelectedItem is TabItem selectedTab)
                {
                    string header = selectedTab.Header?.ToString() ?? string.Empty;
                    switch (header)
                    {
                        case "Expense":
                            mv.SelectedTransactionType = Enums.TransactionType.Spend;
                            break;
                        case "Income":
                            mv.SelectedTransactionType = Enums.TransactionType.Revenue;
                            break;
                    }
                }
            }
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
                return new ValidationResult(false, "* Required");
            }
        }
    }
}