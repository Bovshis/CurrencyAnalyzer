using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void GetStatistic(DateTime start, DateTime end, string currency)
        {

        }
    }
}
