namespace VendingMachine.Test
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Logic;
    using Logic.Enums;
    using Logic.Interfaces;

    /// <summary>
    /// Testing class for vending machine 
    /// </summary>
    [TestClass]
    public class VendingMachineTest
    {
        #region Members

        /// <summary>
        /// Instance of vending machine
        /// </summary>
        private IVendingMachine vendingMachine;

        private Product[] products;

        #endregion

        #region Initialization

        /// <summary>
        /// Initialization of all tests
        /// </summary>
        [TestInitialize]
        public void VendingMachineTestInitialize()
        {
            this.products = GenerateProducts();

            vendingMachine = new Logic.VendingMachine(products);
        }

        #endregion

        #region Iitialization test

        /// <summary>
        /// It is possible to update list of products at any time.
        /// To update products need create new instance of machine
        /// If all is correct no errors occurs
        /// </summary>
        [TestMethod]
        public void StartNewVendingMachineNoErorrs()
        {
            var newProductList = products.ToList();
            newProductList.Remove(newProductList.First());

            this.vendingMachine = new Logic.VendingMachine(newProductList.ToArray());
        }

        /// <summary>
        /// Initialization of new instance with nullable products list.
        /// <see cref="ArgumentNullException"/> error occurs.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StartNewVendingMachineWithNullableProductsArgumentNullException()
        {
            var newProductList = products.ToList();
            newProductList.Remove(newProductList.First());

            this.vendingMachine = new Logic.VendingMachine(null);
        }

        /// <summary>
        /// Initialization of new instance with nullable products list.
        /// <see cref="TypeInitializationException"/> error occurs.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TypeInitializationException))]
        public void StartNewVendingMachineWithEmptyProductsTypeInitializationException()
        {
            var newProductList = products.ToList();
            newProductList.Clear();

            this.vendingMachine = new Logic.VendingMachine(newProductList.ToArray());
        }

        /// <summary>
        /// Initialization of new instance with negative priced product.
        /// <see cref="TypeInitializationException"/> error occurs.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TypeInitializationException))]
        public void StartNewVendingMachineWithNegativePriceTypeInitializationException()
        {
            products[0].Price = new Money()
            {
                Cents = -1
            };

            this.vendingMachine = new Logic.VendingMachine(products);
        }

        /// <summary>
        /// Initialization of new instance with zero priced product.
        /// <see cref="TypeInitializationException"/> error occurs.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TypeInitializationException))]
        public void StartNewVendingMachineWithZeroPriceTypeInitializationException()
        {
            products[0].Price = new Money()
            {
                Cents = 0,
                Euros = 0
            };

            this.vendingMachine = new Logic.VendingMachine(products);
        }

        /// <summary>
        /// Initialization of new instance with empty product name.
        /// <see cref="TypeInitializationException"/> error occurs.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TypeInitializationException))]
        public void StartNewVendingMachineWithEmptyProductNameTypeInitializationException()
        {
            products[0].Name = "";

            this.vendingMachine = new Logic.VendingMachine(products);
        }

        /// <summary>
        /// Initialization of new instance with negative product number.
        /// <see cref="TypeInitializationException"/> error occurs.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TypeInitializationException))]
        public void StartNewVendingMachineWithNegativeProductNumberTypeInitializationException()
        {
            products[0].Name = "";

            this.vendingMachine = new Logic.VendingMachine(products);
        }

        /// <summary>
        /// Initialization of new instance with negative product number.
        /// <see cref="TypeInitializationException"/> error occurs.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TypeInitializationException))]
        public void StartNewVendingMachineWithIdenticalProductNumbersTypeInitializationException()
        {
            products[0].ProductNumber = products[1].ProductNumber;

            this.vendingMachine = new Logic.VendingMachine(products);
        }

        #endregion

        #region Coins inserting test

        /// <summary>
        /// Wrong coin value instrted, wrong coin value message and return wrong money back
        /// </summary>
        [TestMethod]
        public void InstertedWrongCoin()
        {
            var money = new Money() { Cents = 1 };
            var wrongCoinInsertedOccurs = false;

            vendingMachine.MessageChanged += delegate(object sender, MessageEnum e)
            {
                if (e == MessageEnum.WrongCoinInserted)
                {
                    wrongCoinInsertedOccurs = true;
                }
            };

            var backMoney = vendingMachine.InsertCoin(money);

            Assert.AreEqual(money, backMoney);
            Assert.IsTrue(wrongCoinInsertedOccurs);
        }

        /// <summary>
        /// Correct coin value instrted, InsertCoinsOrSelectProduct message occurs
        /// </summary>
        [TestMethod]
        public void InstertedCorrectCoinReturnBackZeroWithMessage()
        {
            var money = new Money() { Cents = 5 };
            var correctCoinInsertedOccurs = false;

            vendingMachine.MessageChanged += delegate(object sender, MessageEnum e)
            {
                if (e == MessageEnum.InsertCoinsOrSelectProduct)
                {
                    correctCoinInsertedOccurs = true;
                }
            };

            var backMoney = vendingMachine.InsertCoin(money);
            
            Assert.AreNotEqual(money, backMoney);
            Assert.AreEqual(money, new Money());
            Assert.IsTrue(correctCoinInsertedOccurs);
        }

        #endregion

        #region Buying test

        /// <summary>
        /// Buying product with no enought money, message occurs
        /// </summary>
        [TestMethod]
        public void BuyProductWithNotEnoughtMoneyNotEnoughtMoneyMessageShows()
        {
            var money = new Money() { Cents = 5 };
            vendingMachine.InsertCoin(money);

            var product = vendingMachine.Buy(products.First().ProductNumber);
            var notEnoughtMoneyToBuySelectedProductOccurs = false;

            vendingMachine.MessageChanged += delegate(object sender, MessageEnum e)
            {
                if (e == MessageEnum.NotEnoughtMoneyToBuySelectedProduct)
                {
                    notEnoughtMoneyToBuySelectedProductOccurs = true;
                }
            };
            
            Assert.IsTrue(notEnoughtMoneyToBuySelectedProductOccurs);
            Assert.IsNull(product);
        }

        /// <summary>
        /// Buying product with enough money with no change(money back)
        /// </summary>
        [TestMethod]
        public void BuyProductWithEnoughtMoneyAndNoChangeReturningProduct()
        {
            var money = products.First().Price;
            vendingMachine.InsertCoin(money);

            var product = vendingMachine.Buy(products.First().ProductNumber);
            var productBuiedWithNoChangeOccurs = false;

            vendingMachine.MessageChanged += delegate(object sender, MessageEnum e)
            {
                if (e == MessageEnum.ProductBuyedWithNoChange)
                {
                    productBuiedWithNoChangeOccurs = true;
                }
            };
            
            Assert.IsTrue(productBuiedWithNoChangeOccurs);
            Assert.IsNotNull(product);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Correct product list generation
        /// </summary>
        /// <returns>Correct list of products</returns>
        private Product[] GenerateProducts()
        {
            return new[]
            {
                new Product()
                {
                    ProductNumber = 1,
                    Available = 3,
                    Name = "Chocolate 'Pergale'",
                    Price = new Money()
                    {
                        Euros = 1,
                        Cents = 30
                    }
                },
                new Product()
                {
                    ProductNumber = 2,
                    Available = 5,
                    Name = "Chocolate 'Karuna'",
                    Price = new Money()
                    {
                        Euros = 1,
                        Cents = 30
                    }
                },
                new Product()
                {
                    ProductNumber = 3,
                    Available = 3,
                    Name = "Crips 'Estrella' with pizza taste",
                    Price = new Money()
                    {
                        Euros = 1,
                        Cents = 0
                    }
                },
                new Product()
                {
                    ProductNumber = 4,
                    Available = 4,
                    Name = "Crips 'Estrella' with onion taste",
                    Price = new Money()
                    {
                        Euros = 1,
                        Cents = 0
                    }
                },
                new Product()
                {
                    ProductNumber = 5,
                    Available = 6,
                    Name = "Chocolate 'Snickers'",
                    Price = new Money()
                    {
                        Euros = 0,
                        Cents = 65
                    }
                },
                new Product()
                {
                    ProductNumber = 6,
                    Available = 7,
                    Name = "Chocolate 'Mars'",
                    Price = new Money()
                    {
                        Euros = 0,
                        Cents = 65
                    }
                },
                new Product()
                {
                    ProductNumber = 7,
                    Available = 3,
                    Name = "Candy 'M&Ms'",
                    Price = new Money()
                    {
                        Euros = 0,
                        Cents = 70
                    }
                }
            };
        }

        #endregion
    }
}
