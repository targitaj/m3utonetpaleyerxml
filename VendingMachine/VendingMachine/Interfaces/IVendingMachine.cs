namespace VendingMachine.Logic.Interfaces
{
    using System;
    using Enums;

    /// <summary>
    /// Emulation of real vending machine work
    /// </summary>
    public interface IVendingMachine
    {
        event EventHandler<MessageEnum> MessageChanged;

        /// <summary>Vending machine manufacturer.</summary>
        string Manufacturer { get; }

        /// <summary>Amount of money inserted into vending machine. </summary>
        Money Amount { get; }

        /// <summary>Products that are sold.</summary>
        Product[] Products { get; set; }

        /// <summary>Inserts the coin into vending machine.</summary>
        /// <param name="amount">Coin amount.</param>
        Money InsertCoin(Money amount);

        /// <summary>Returns all inserted coins back to user.</summary>
        Money ReturnMoney();

        /// <summary>Buys product from list of product.</summary>
        /// <param name="productNumber">Product number in vending machine product list.</param>>
        Product Buy(int productNumber);
    }
}