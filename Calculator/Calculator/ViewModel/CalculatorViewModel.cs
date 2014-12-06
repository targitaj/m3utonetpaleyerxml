using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Calculator.Commands;
using Calculator.Embed;
using Calculator.Model;
using MosalskyCalculatorPlugin;
using PluginShared;

namespace Calculator.ViewModel
{
    internal class CalculatorViewModel
    {
        #region Members

        private IPlugin currentPlugin;
        private double prevResult;
        private bool pluginChanged;
        private bool isEqualPressed;
        private double? secondNumber;

        #endregion

        #region Constructors

        public CalculatorViewModel()
        {
            Calculator = new CalculatorModel();
            LoadPlugins();
        }

        #endregion

        #region Properties

        private double Result
        {
            get
            {
                double res;
                bool stat = double.TryParse(Calculator.Result, out res);

                if (!stat)
                {
                    Calculator.Result = "0";
                }

                return res;
            }
            set { Calculator.Result = value.ToString(CultureInfo.InvariantCulture); }
        }

        private IPlugin CurrentPlugin
        {
            get { return currentPlugin; }
            set
            {
                if (!isEqualPressed && !pluginChanged)
                {
                    RunCalculation();
                }

                secondNumber = null;
                currentPlugin = value;

                if (currentPlugin != null)
                {
                    if (!currentPlugin.NeedTwoNumbers)
                    {
                        RunCalculation();
                    }
                    else
                    {
                        prevResult = Result;
                        pluginChanged = true;
                    }
                }
            }
        }

        public CalculatorModel Calculator { get; set; }

        #endregion

        #region Commands

        private DelegateCommand<object> numberCommand;
        private DelegateCommand clearCommand;
        private DelegateCommand minusCommand;
        private DelegateCommand plusCommand;
        private DelegateCommand equalCommand;

        public ICommand NumberPressedCommand
        {
            get
            {
                if (numberCommand == null)
                    numberCommand = new DelegateCommand<object>(new Action<object>(NumberPressedExecuted));

                return numberCommand;
            }
        }

        public ICommand ClearPressedCommand
        {
            get
            {
                if (clearCommand == null)
                    clearCommand = new DelegateCommand(new Action(NumberPressedExecuted));

                return clearCommand;
            }
        }

        public ICommand MinusPressedCommand
        {
            get
            {
                if (minusCommand == null)
                    minusCommand = new DelegateCommand(new Action(MinusPressedExecuted));

                return minusCommand;
            }
        }

        public ICommand PlusPressedCommand
        {
            get
            {
                if (plusCommand == null)
                    plusCommand = new DelegateCommand(new Action(PlusPressedExecuted));

                return plusCommand;
            }
        }
        
        public ICommand EqualPressedCommand
        {
            get
            {
                if (equalCommand == null)
                    equalCommand = new DelegateCommand(new Action(EqualPressedExecuted));

                return equalCommand;
            }
        }

        #endregion

        #region Methods

        private void LoadPlugins()
        {
            // TODO: read from plugin folder

            var testMult = new Multiplication();
            Calculator.ExternalButtonCommands = new List<ExternalButtonModel>()
            {
                new ExternalButtonModel()
                {
                    ExternalButtonCommand = new DelegateCommand(new Action(delegate
                    {
                        CurrentPlugin = new Multiplication();
                    })),
                    Name = testMult.Name
                }
            };
        }

        public void NumberPressedExecuted()
        {
            Result = 0;
            currentPlugin = null;
        }

        public void EqualPressedExecuted()
        {
            isEqualPressed = true;
            RunCalculation();
        }

        public void MinusPressedExecuted()
        {
            CurrentPlugin = new Minus();
        }

        public void PlusPressedExecuted()
        {
            CurrentPlugin = new Plus();
        }

        private void RunCalculation()
        {
            if (currentPlugin != null)
            {
                if (!secondNumber.HasValue)
                {
                    secondNumber = Result;
                }

                Result = CurrentPlugin.Calculate(prevResult, secondNumber);
                prevResult = Result;
            }
        }

        public void NumberPressedExecuted(object sender)
        {
            isEqualPressed = false;

            if (pluginChanged)
            {
                Calculator.Result = "0";
                pluginChanged = false;
            }

            if (Calculator.Result.Length <= 10)
            {
                if (!Calculator.Result.Contains(".") || ((Button)sender).Content.ToString() != ".")
                {
                    if (Calculator.Result != "0" || Calculator.Result.Length > 1 || ((Button)sender).Content.ToString() == ".")
                    {
                        Calculator.Result = Calculator.Result + ((Button)sender).Content;
                    }
                    else
                    {
                        Calculator.Result = ((Button)sender).Content.ToString();
                    }
                }
            }
        }

        #endregion
    }
}
