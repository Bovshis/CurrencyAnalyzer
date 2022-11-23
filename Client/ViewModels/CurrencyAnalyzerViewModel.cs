using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Client.Models;
using Client.Resources.Commands;
using Client.Resources.Enums;
using OxyPlot;

namespace Client.ViewModels
{
    public class CurrencyAnalyzerViewModel
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

        public ICommand GetCurrencyRates => new RelayCommand((_) =>
        {
            _model.GetCurrencyRates(StartDate, EndDate, SelectedCurrency);
            var a = 1;
        });
    }
}
