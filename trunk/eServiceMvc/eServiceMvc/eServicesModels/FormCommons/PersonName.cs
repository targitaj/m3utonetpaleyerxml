namespace Uma.Eservices.Models.FormCommons
{
    using System.Diagnostics;

    using Uma.Eservices.Models.Shared;

    /// <summary>
    /// Model to hold person name(s) information
    /// </summary>
    [DebuggerDisplay("Id: {Id}  fName: {FirstName}")]
    public class PersonName : BaseModel
    {
        /// <summary>
        /// Unique Object key
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// First name(s) of a person
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name(s) of a person
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Returns True, if one of names is null or empty
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return string.IsNullOrEmpty(this.FirstName) || string.IsNullOrEmpty(this.LastName);
            }
        }

        /// <summary>
        /// Determined whether two objects are equal - same properties
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            PersonName p = obj as PersonName;

            if (p == null)
            {
                return false;
            }

            return (this.FirstName == p.FirstName) &&
                   (this.LastName == p.LastName);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}