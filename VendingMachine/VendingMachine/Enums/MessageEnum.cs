namespace VendingMachine.Logic.Enums
{
    /// <summary>
    /// Enum for message displaying about current process
    /// </summary>
    public enum MessageEnum
    {
        /// <summary>
        /// Wrong coins inserted
        /// </summary>
        WrongCoinInserted,

        /// <summary>
        /// Insert coins or select product
        /// </summary>
        InsertCoinsOrSelectProduct,

        /// <summary>
        /// Not enought money to buy selected product
        /// </summary>
        NotEnoughtMoneyToBuySelectedProduct,

        /// <summary>
        /// Product buyed with no change
        /// </summary>
        ProductBuyedWithNoChange,

        /// <summary>
        /// Product buyed with change
        /// </summary>
        ProductBuyedWithChange,

        /// <summary>
        /// No money return possible becouse no money insetrted
        /// </summary>
        NoMoneyToReturn,

        /// <summary>
        /// Inserted money returned
        /// </summary>
        TakeReturnedMoney,

        /// <summary>
        /// No such product or it is finished
        /// </summary>
        SelectCorrectProduct
    }
}
