namespace VendingMachine.Logic
{
    using System.Collections.Generic;
    using System.Linq;
    using Enums;
    using Interfaces;
    using System;

    /// <summary>
    /// Emulation of real vending machine work
    /// </summary>
    public class VendingMachine : IVendingMachine
    {
        #region Events

        /// <summary>
        /// Event for handling message changes
        /// </summary>
        public event EventHandler<MessageEnum> MessageChanged;

        #endregion

        #region Members

        /// <summary>
        /// Store information about acceptable coins
        /// </summary>
        private static List<Money> ACCEPTABLE_COINS = new List<Money>()
        {
            new Money()
            {
                Cents = 5
            },
            new Money()
            {
                Cents = 10
            },
            new Money()
            {
                Cents = 20
            },
            new Money()
            {
                Cents = 50
            },
            new Money()
            {
                Euros = 1
            },
            new Money()
            {
                Euros = 2
            }
        };

        #endregion

        #region Properties

        /// <summary>Vending machine manufacturer.</summary>
        public string Manufacturer { get; private set; }

        /// <summary>Amount of money inserted into vending machine. </summary>
        public Money Amount { get; private set; }

        /// <summary>Products that are sold.</summary>
        public Product[] Products { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VendingMachine"/> class.
        /// </summary>
        /// <param name="products">Products that are placed to vending machine by employee</param>
        public VendingMachine(Product[] products)
        {
            if (products == null)
            {
                throw new ArgumentNullException("products");
            }

            if (products.Length == 0)
            {
                throw new TypeInitializationException("VendingMachine.Logic.VendingMachine", new Exception("Pruducts must contain one or more elements"));
            }

            this.Amount = new Money();
            this.Products = products;

            string vald = ValidateProducts();

            if (!string.IsNullOrEmpty(vald))
            {
                throw new TypeInitializationException("VendingMachine.Logic.VendingMachine", new Exception(vald));
            }
        }

        #endregion

        #region Public methods

        /// <summary>Inserts the coin into vending machine.</summary>
        /// <param name="amount">Coin amount.</param>
        public Money InsertCoin(Money amount)
        {
            var res = new Money();

            if (!ACCEPTABLE_COINS.Any(a=>a.Cents == amount.Cents && a.Euros == amount.Euros))
            {
                if (MessageChanged != null)
                {
                    MessageChanged(this, MessageEnum.WrongCoinInserted);
                    res = amount;
                }
            }
            else
            {
                this.Amount += amount;

                if (MessageChanged != null)
                {
                    MessageChanged(this, MessageEnum.InsertCoinsOrSelectProduct);
                }
            }

            return res;
        }

        // TODO: Rewrite logic to return ACCEPTABLE_COINS array
        /// <summary>Returns all inserted coins back to user.</summary>
        public Money ReturnMoney()
        {
            var res = new Money();

            if (this.Amount == new Money())
            {
                if (MessageChanged != null)
                {
                    MessageChanged(this, MessageEnum.NoMoneyToReturn);
                }
            }
            else
            {
                if (MessageChanged != null)
                {
                    MessageChanged(this, MessageEnum.TakeReturnedMoney);
                }

                res = this.Amount;
                this.Amount -= this.Amount;
            }

            return res;
        }

        /// <summary>Buys product from list of product.</summary>
        /// <param name="productNumber">Product number in vending machine product list.</param>>
        public Product? Buy(int productNumber)
        {
            Product? res = null;

            var prod = this.Products.FirstOrDefault(p => p.ProductNumber == productNumber);

            if (prod.Available > 0)
            {
                if (prod.Price > this.Amount)
                {
                    if (MessageChanged != null)
                    {
                        MessageChanged(this, MessageEnum.NotEnoughtMoneyToBuySelectedProduct);
                    }
                }

                if (prod.Price == this.Amount)
                {
                    res = prod;

                    if (MessageChanged != null)
                    {
                        MessageChanged(this, MessageEnum.ProductBuyedWithNoChange);
                    }

                    this.Amount -= prod.Price;
                    this.Products[this.Products.ToList().IndexOf(prod)] = prod.DecrementAvailable();
                }

                if (prod.Price < this.Amount)
                {
                    res = prod;

                    if (MessageChanged != null)
                    {
                        MessageChanged(this, MessageEnum.ProductBuyedWithChange);
                    }

                    this.Amount -= prod.Price;
                    this.Products[this.Products.ToList().IndexOf(prod)] = prod.DecrementAvailable();

                    ReturnMoney();
                }
            }
            else
            {
                if (MessageChanged != null)
                {
                    MessageChanged(this, MessageEnum.SelectCorrectProduct);
                }
            }

            return res;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Used for initial product list validation
        /// </summary>
        /// <returns>Error message or null</returns>
        private string ValidateProducts()
        {
            foreach (var prod in this.Products)
            {
                if (prod.Price.Euros < 0 || prod.Price.Cents < 0)
                {
                    return "Euros and cents can't have negative value";
                }

                if (prod.Price.Euros + prod.Price.Cents <= 0)
                {
                    return "Price must be larger than zero";
                }

                if (string.IsNullOrEmpty(prod.Name))
                {
                    return "Product name must have value";
                }

                if (prod.Available < 0)
                {
                    return "Product count must not be negative";
                }
            }

            if (this.Products.Any(a => this.Products.Count(c => c.ProductNumber == a.ProductNumber) > 1))
            {
                return "Product number must be unique";
            }

            return null;
        }

        #endregion
    }
}
