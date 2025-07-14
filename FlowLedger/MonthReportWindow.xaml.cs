using FlowLedger.Enums;
using FlowLedger.Models;
using FlowLedger.Utils;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.UniversalAccessibility.Drawing;
using System.ComponentModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace FlowLedger
{
    /// <summary>
    /// Interaction logic for MonthReportWindow.xaml
    /// </summary>
    public partial class MonthReportWindow : Window, INotifyPropertyChanged
    {
        private bool _canExportPDF;

        private readonly KeyValuePair<YearMonth, MonthTransactions> _monthSummary;

        public event PropertyChangedEventHandler? PropertyChanged;

        internal MonthReportWindow(KeyValuePair<YearMonth, MonthTransactions> monthSummary)
        {
            DataContext = this;
            InitializeComponent();
            _monthSummary = monthSummary;
            LoadMonthReport();
            // has data to do file-export
            CanExportPDF = _monthSummary.Value != null && _monthSummary.Value.Transactions.Any();
        }

        public bool CanExportPDF
        {
            get => _canExportPDF;
            set
            {
                if (_canExportPDF != value) 
                {
                    _canExportPDF = value;
                    OnPropertyChanged(nameof(CanExportPDF));
                }
            }
        }

        private void SaveAsPdf_Click(object sender, RoutedEventArgs e)
        {
            var (ok, file, error) = FileHelper.SelectFileForSaving("pdf");
            if (ok && string.IsNullOrEmpty(error)) 
            {
                PdfDocument doc = new PdfDocument();
                doc.Info.Title = "Monthly Summary Report";
                PdfPage page = doc.AddPage();
                // canvas
                XGraphics xgf = XGraphics.FromPdfPage(page);
                XFont titleFont = new XFont("Arial", 14, XFontStyleEx.Bold);
                XFont paragraphFont = new XFont("Arial", 12, XFontStyleEx.Regular);
                XPen mainDividerPen = new XPen(XColors.Black, 2);
                mainDividerPen.DashStyle = XDashStyle.Solid;

                double marginLeftRight = 40;
                double marginStackYToTop = 25;
                double contentWidth = page.Width - 2 * marginLeftRight;

                // topleft - Y: 25 
                var titleRect = new XRect(marginLeftRight, marginStackYToTop, contentWidth, 40);
                xgf.DrawString(
                    $"Month Summary Report - {_monthSummary.Key.ToString()}", 
                    titleFont, 
                    XBrushes.Black,
                    titleRect, 
                    XStringFormats.Center
                    );
                xgf.DrawRectangle(XPens.Red, titleRect); // *** only for debugging purpose with visualization of the layout

                // topleft - Y: (25 + 40) + 10
                marginStackYToTop = marginStackYToTop + 50;
                var lineRect = new XRect(marginLeftRight, marginStackYToTop, contentWidth, 20);
                xgf.DrawLine(
                    mainDividerPen, 40, 85, contentWidth + marginLeftRight, 85
                    );
                xgf.DrawRectangle(XPens.Red, lineRect); // *** only for debugging purpose with visualization of the layout

                // topleft - Y: ((25 + 40) + 10) + 20 + 10 = 105
                marginStackYToTop += 30;
                var paraRect = new XRect(marginLeftRight, marginStackYToTop, contentWidth, 40);
                xgf.DrawString(
                    $"Net cash-flow: {_monthSummary.Value.MonthlyNet}",
                    paragraphFont,
                    XBrushes.Black,
                    paraRect,
                    XStringFormats.TopLeft
                    );
                xgf.DrawRectangle(XPens.Red, paraRect); // *** only for debugging purpose with visualization of the layout

                // topleft - Y: 105 + 40 + 10
                marginStackYToTop += 50;
                var categoryListParaRect = new XRect(marginLeftRight, marginStackYToTop, contentWidth, 100);

                xgf.DrawRectangle(XPens.Red, categoryListParaRect); // *** only for debugging purpose with visualization of the layout

                marginStackYToTop += 110;
                categoryListParaRect = new XRect(marginLeftRight, marginStackYToTop, contentWidth, 100);

                xgf.DrawRectangle(XPens.Red, categoryListParaRect); // *** only for debugging purpose with visualization of the layout

                doc.Save(file);
            }
        }

        private void LoadMonthReport()
        {
            var doc = new FlowDocument
            {
                FontSize = 14,
                PagePadding = new Thickness(20)
            };

            doc.Blocks.Add(new Paragraph(new Bold(new Run($"Month Summary Report - {_monthSummary.Key.ToString()}"))) { FontSize = 16 });

            if (_monthSummary.Value == null || !_monthSummary.Value.Transactions.Any())
            {
                doc.Blocks.Add(new Paragraph(new Run("No data available.")));
                ReportViewer.Document = doc;
                return;
            }

            var netSummary = new Paragraph();
            netSummary.Inlines.Add(new Run($"Net cash-flow: ") { FontWeight = FontWeights.Bold });
            netSummary.Inlines.Add(new Run($"{_monthSummary.Value.MonthlyNet}\n"));
            doc.Blocks.Add(netSummary);

            var topExpenseCategoryList = new List()
            {
                MarkerStyle = TextMarkerStyle.Disc,
                Margin = new Thickness(20, 0, 0, 10)
            };
            var expenseCategories = GetTopExpenseCategories();
            foreach (var category in expenseCategories)
            {
                string itemText = $"{category}";
                topExpenseCategoryList.ListItems.Add(new ListItem(new Paragraph(new Run(itemText))));
            }
            doc.Blocks.Add(new Paragraph(new Run($"\nTop 3 expense categories: "))
            {
                FontWeight = FontWeights.Normal,
                FontSize = 14,
                Margin = new Thickness(0, 10, 0, 5)
            });
            doc.Blocks.Add(topExpenseCategoryList);

            var topRevenueCategoryList = new List()
            {
                MarkerStyle = TextMarkerStyle.Disc,
                Margin = new Thickness(20, 0, 0, 10),
                Foreground = Brushes.DarkBlue,
                FontWeight = FontWeights.SemiBold,
            };
            var revenueCategories = GetTopRevenueCategories();
            foreach (var category in revenueCategories)
            {
                string itemText = $"{category}";
                topRevenueCategoryList.ListItems.Add(new ListItem(new Paragraph(new Run(itemText))));
            }
            doc.Blocks.Add(new Paragraph(new Run($"\nTop 3 revenue categories: "))
            {
                FontWeight = FontWeights.SemiBold,
                Foreground = Brushes.DarkBlue,
                FontSize = 14,
                Margin = new Thickness(0, 10, 0, 5)
            });
            doc.Blocks.Add(topRevenueCategoryList);

            ReportViewer.Document = doc;
        }

        private List<string> GetTopExpenseCategories()
        {
            var topExpenseCategories = _monthSummary.Value.Transactions
                .Where(t => t.Category.TransactionType == TransactionType.Spend &&
                            !string.IsNullOrEmpty(t.Category.Name))
                .GroupBy(t => t.Category.Name)
                .OrderByDescending(g => g.Count())
                .Take(3)
                .Select(g => g.Key)
                .ToList();
            return topExpenseCategories;
        }

        private List<string> GetTopRevenueCategories()
        {
            var topRevenueCategories = _monthSummary.Value.Transactions
                .Where(t => t.Category.TransactionType == TransactionType.Revenue &&
                            !string.IsNullOrEmpty(t.Category.Name))
                .GroupBy(t => t.Category.Name)
                .OrderByDescending(g => g.Count())
                .Take(3)
                .Select(g => g.Key)
                .ToList();
            return topRevenueCategories;
        }

        protected void OnPropertyChanged(string propertyName) 
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
