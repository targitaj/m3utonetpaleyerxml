using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginShared
{
    public interface IPlugin
    {
        string Name { get; }

        bool NeedTwoNumbers { get; }

        double Calculate(double firstNumber, double? secondNumber);
    }
}
