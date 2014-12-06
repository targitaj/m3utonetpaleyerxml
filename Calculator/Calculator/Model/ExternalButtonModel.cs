using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator.Commands;

namespace Calculator.Model
{
    public class ExternalButtonModel
    {
        public DelegateCommand ExternalButtonCommand { get; set; }

        public string Name { get; set; }
    }
}
