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

        public int DependentCount { get; set; }

        public int WeekEnds { get; set; }

        public int Holidays { get; set; }

        public int Hours { get; set; }

        public int WorkingDays { get; set; }

        public bool AutoCalculateTaxSalary { get; set; } = true;

        private double _taxSalary;

        public double TaxSalary
        {
            get { return AutoCalculateTaxSalary ? MinusTax(Salary, DependentCount) : _taxSalary; }
            set { _taxSalary = value; }
        }

        public double MinusTax(double salary, int dependentCount)
        {
            var notTaxed = 75 + (165 * dependentCount);
            var obligate = (salary * 0.105);
            var livTax = (salary - obligate - notTaxed) * 0.23;

            return salary - obligate - livTax;
        }
    }
}
