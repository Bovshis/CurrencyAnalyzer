using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Resources;
using Client.Resources.Enums;
using Client.Services;
using OxyPlot;
using OxyPlot.Series;

namespace Client.Models
{
    public class CurrencyAnalyzerModel
    {
        private readonly CurrencyRateProvider _currencyRateProvider;

        public CurrencyAnalyzerModel(CurrencyRateProvider _currencyRateProvider)
        {
            this._currencyRateProvider = _currencyRateProvider;
        }

        public PlotModel Plot { get; private set; } = new PlotModel();

        public async Task GetCurrencyRates(DateTime start, DateTime end, Currency currency)
        {
            var points = await _currencyRateProvider.GetCurrencyRate(start, end, currency);
            Plot = new PlotModel();
            var lineSeries = new LineSeries();
            lineSeries.Points.AddRange(points);
        }
    }
}
