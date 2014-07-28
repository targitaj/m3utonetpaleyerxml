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
        /// Store initial products in vending machine
        /// </summary>
        private Product[] products;

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

            this.products = products;

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

            if (!ACCEPTABLE_COINS.Contains(amount))
            {
                if (MessageChanged != null)
                {
                    MessageChanged(this, MessageEnum.WrongCoinInserted);
                    res = amount;
                }
            }

            return res;
        }

        /// <summary>Returns all inserted coins back to user.</summary>
        public Money ReturnMoney()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>Buys product from list of product.</summary>
        /// <param name="productNumber">Product number in vending machine product list.</param>>
        public Product Buy(int productNumber)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Used for initial product list validation
        /// </summary>
        /// <returns>Error message or null</returns>
        private string ValidateProducts()
        {
            foreach (var prod in this.products)
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

            if (this.products.Any(a => this.products.Count(c => c.ProductNumber == a.ProductNumber) > 1))
            {
                return "Product number must be unique";
            }

            return null;
        }

        #endregion
    }
}
