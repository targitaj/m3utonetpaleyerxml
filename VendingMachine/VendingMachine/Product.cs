namespace VendingMachine.Logic
{
    public struct Product
    {
        /// <summary>Gets or sets number of product to user have ability select one</summary>
        public int ProductNumber { get; set; }

        ///<summary>Gets or sets the available amount of product.</summary>
        public int Available { get; set; }

        ///<summary>Gets or sets the product price.</summary>
        public Money Price { get; set; }

        ///<summary>Gets or sets the product name.</summary>
        public string Name { get; set; }

        /// <summary>
        /// Decrement operator overload
        /// </summary>
        /// <param name="obj">Object to decrement</param>
        /// <returns>Decrement result</returns>
        public Product DecrementAvailable()
        {
            return new Product()
            {
                ProductNumber = this.ProductNumber,
                Available = this.Available -1,
                Name = this.Name,
                Price = this.Price
            };
        }
    }
}
