using System.Net.Http;
using System.Windows;
using Client.Models;
using Client.Services;
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
            InitializeDateContext();
        }

        private void InitializeDateContext()
        {
            var currencyRateProvider = new CurrencyRateProvider(new HttpClient());
            var currencyAnalyzerModel = new CurrencyAnalyzerModel(currencyRateProvider);
            var currencyAnalyzerViewModel = new CurrencyAnalyzerViewModel(currencyAnalyzerModel);
            DataContext = currencyAnalyzerViewModel;
        }
    }
}
