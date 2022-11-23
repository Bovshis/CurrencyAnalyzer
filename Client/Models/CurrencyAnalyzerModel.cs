using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Resources;
using Client.Resources.Enums;
using OxyPlot;
using OxyPlot.Series;

namespace Client.Models
{
    public class CurrencyAnalyzerModel
    {
        public CurrencyAnalyzerModel()
        {

        }

        public PlotModel Plot { get; private set; } = new PlotModel();

        public List<Currency> Currencies { get; set; }

        public void GetCurrencyRates(DateTime start, DateTime end, Currency currency)
        {
            Plot = new PlotModel();
            var line = new LineSeries();
            line.Points.Add(new DataPoint(1,1));
            line.Points.Add(new DataPoint(2,2));
            Plot.Series.Add(line);
        }
    }
}
