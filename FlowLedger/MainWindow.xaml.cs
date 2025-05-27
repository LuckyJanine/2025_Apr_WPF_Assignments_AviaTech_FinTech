using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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
                        txtCategoryName.Text = string.Empty;
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
                switch (mv.SelectedTransactionType)
                {
                    case Enums.TransactionType.Revenue:
                        txtIncomeAmount.Text = string.Empty;
                        break;
                    case Enums.TransactionType.Spend:
                        txtExpenseAmount.Text = string.Empty;
                        break;
                }
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
}