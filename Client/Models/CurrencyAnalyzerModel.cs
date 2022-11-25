using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Resources;
using Client.Resources.Enums;
using Client.Services;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace Client.Models
{
    public class CurrencyAnalyzerModel : PropertyNotifier
    {
        private readonly CurrencyRateProvider _currencyRateProvider;

        public CurrencyAnalyzerModel(CurrencyRateProvider _currencyRateProvider)
        {
            this._currencyRateProvider = _currencyRateProvider;
            Plot = new PlotModel();
            Plot.Axes.Add(new DateTimeAxis()
            {
                
                Position = AxisPosition.Bottom,
                StringFormat = "yyyy-MM-dd",
                MinorIntervalType = DateTimeIntervalType.Days,
                IntervalType = DateTimeIntervalType.Days,
            });
        }

        public PlotModel Plot { get; private set; }

        public async Task GetCurrencyRates(DateTime start, DateTime end, Currency currency)
        {
            var points = await _currencyRateProvider.GetCurrencyRate(start, end, currency);
            Plot = new PlotModel();
            Plot.Axes.Add(new DateTimeAxis()
            {

                Position = AxisPosition.Bottom,
                StringFormat = "yyyy-MM-dd",
                MinorIntervalType = DateTimeIntervalType.Days,
                IntervalType = DateTimeIntervalType.Days,
            });
            var lineSeries = new LineSeries();
            lineSeries.Points.AddRange(points);
            Plot.Series.Add(lineSeries);
        }
    }
}
