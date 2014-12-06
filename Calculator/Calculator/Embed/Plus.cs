using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginShared;

namespace Calculator.Embed
{
    public class Plus : IPlugin
    {
        public string Name { get { return "+"; } }

        public bool NeedTwoNumbers { get { return true; } }

        public double Calculate(double firstNumber, double? secondNumber)
        {
            return (secondNumber ?? 0) + firstNumber;
        }
    }
}
