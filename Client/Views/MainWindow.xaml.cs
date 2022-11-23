using System.Windows;
using Client.Models;
using Client.ViewModels;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var currencyAnalyzerModel = new CurrencyAnalyzerModel();
            var currencyAnalyzerViewModel = new CurrencyAnalyzerViewModel(currencyAnalyzerModel);
            DataContext = currencyAnalyzerViewModel;
        }
    }
}
