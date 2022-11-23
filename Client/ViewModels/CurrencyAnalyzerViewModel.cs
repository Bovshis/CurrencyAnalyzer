using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Models;
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
    }
}
