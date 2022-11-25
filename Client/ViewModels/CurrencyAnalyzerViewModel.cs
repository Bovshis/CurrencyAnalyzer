using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Client.Models;
using Client.Resources;
using Client.Resources.Commands;
using Client.Resources.Enums;
using OxyPlot;

namespace Client.ViewModels
{
    public class CurrencyAnalyzerViewModel : PropertyNotifier
    {
        private readonly CurrencyAnalyzerModel _model;

        public CurrencyAnalyzerViewModel(CurrencyAnalyzerModel model)
        {
            _model = model;
        }

        public PlotModel Plot => _model.Plot;

        public DateTime StartDate { get; set; } = DateTime.Now;

        public DateTime EndDate { get; set; } = DateTime.Now;

        public Currency SelectedCurrency { get; set; }

        public ICommand GetCurrencyRates => new RelayCommand(async (_) =>
        {
            if (StartDate > EndDate)
            {
                MessageBox.Show("Start date can't be grater than end date!", "Error");
                return;
            }
            await _model.GetCurrencyRates(StartDate, EndDate, SelectedCurrency);
            OnPropertyChanged(nameof(Plot));
        });
    }
}
