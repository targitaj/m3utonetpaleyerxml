using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Mvvm;

namespace BreakingBet
{
    public class MainWindowModel : BindableBase
    {
        private string _percentageOfProfitabilityFrom = "7";
        private string _percentageOfProfitabilityTo = "40";
        private string _logText = string.Empty;

        public string PercentageOfProfitabilityFrom
        {
            get => _percentageOfProfitabilityFrom;
            set => SetProperty(ref _percentageOfProfitabilityFrom, value);
        }

        public string PercentageOfProfitabilityTo
        {
            get => _percentageOfProfitabilityTo;
            set => SetProperty(ref _percentageOfProfitabilityTo, value);
        }

        public string LogText
        {
            get => _logText;
            set => SetProperty(ref _logText, value);
        }
    }
}
