using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools
{
    public class SalaryPerMonth
    {
        public string Month { get; set; }
        public double Salary { get; set; }

        public double TaxSalary
        {
            get
            {
                return MinusTax(Salary);
            }
        }

        public double MinusTax(double salary)
        {
            salary = salary - 75;
            return salary - ((salary / 100) * 10.5) - ((salary / 100) * 24) + 75;
        }
    }
}
