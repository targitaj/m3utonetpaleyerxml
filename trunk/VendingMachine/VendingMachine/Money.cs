namespace VendingMachine.Logic
{
    /// <summary>
    /// Represents structure for money storing
    /// </summary>
    public struct Money
    {
        /// <summary>
        /// Get or sets euros
        /// </summary>
        public int Euros { get; set; }

        /// <summary>
        /// Get or sets cents
        /// </summary>
        public int Cents { get; set; }

        /// <summary>
        /// Equal operator ovveriding
        /// </summary>
        /// <param name="obj1">Fist object to compare</param>
        /// <param name="obj2">Second object to compare</param>
        /// <returns>Comparison result</returns>
        public static bool operator ==(Money obj1, Money obj2)
        {
            return obj1.Cents == obj2.Cents && obj1.Euros == obj2.Euros;
        }

        /// <summary>
        /// Not equal operator ovveriding
        /// </summary>
        /// <param name="obj1">Fist object to compare</param>
        /// <param name="obj2">Second object to compare</param>
        /// <returns>Comparison result</returns>
        public static bool operator !=(Money obj1, Money obj2)
        {
            return obj1.Cents != obj2.Cents && obj1.Euros != obj2.Euros;
        }

        public override bool Equals(object obj)
        {
            return this == (Money)obj;
        }

        //TODO: Add GetHashCode method
    }
}
