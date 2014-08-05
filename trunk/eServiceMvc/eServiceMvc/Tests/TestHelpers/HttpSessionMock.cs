namespace Uma.Eservices.TestHelpers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;

    /// <summary>
    /// Http Session Mock class
    /// </summary>
  [ExcludeFromCodeCoverage]
    public class HttpSessionMock : HttpSessionStateBase
    {
        /// <summary>
        /// Allows to handle mocked variable setters/getters in tests
        /// </summary>
        private readonly Dictionary<string, object> keyValues = new Dictionary<string, object>();

        /// <summary>
        /// When overridden in a derived class, gets or sets a session value by using the specified name.
        /// </summary>
        /// <param name="name">The name/key.</param>
        public override object this[string name]
        {
            get
            {
                return this.keyValues.ContainsKey(name) ? this.keyValues[name] : null;
            }

            set
            {
                this.keyValues[name] = value;
            }
        }

        public override int CodePage
        {
            get
            {
                return RandomData.GetInteger(1100, 1299);
            }
            
            set
            {
            }
        }

        public override HttpSessionStateBase Contents
        {
            get
            {
                return new HttpSessionMock();
            }
        }

        public override HttpCookieMode CookieMode
        {
            get
            {
                return HttpCookieMode.UseCookies;
            }
        }

        public override int Count
        {
            get
            {
                return this.keyValues.Count;
            }
        }

        public new IEnumerable<string> Keys
        {
            get
            {
                return this.keyValues.Keys;
            }
        }

        public Dictionary<string, object> UnderlyingStore
        {
            get
            {
                return this.keyValues;
            }
        }

        public override void Abandon()
        {
            this.keyValues.Clear();
        }

        public override void Add(string name, object value)
        {
            this.keyValues.Add(name, value);
        }

        public override void Clear()
        {
            this.keyValues.Clear();
        }

        public override void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            return this.keyValues.Equals(obj);
        }

        public override IEnumerator GetEnumerator()
        {
            return this.keyValues.GetEnumerator();
        }

        public override int GetHashCode()
        {
            return this.keyValues != null ? this.keyValues.GetHashCode() : 0;
        }

        public override void Remove(string name)
        {
            this.keyValues.Remove(name);
        }

        public override void RemoveAll()
        {
            this.keyValues.Clear();
        }

        public override void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return this.keyValues.ToString();
        }

        public bool Equals(HttpSessionMock other)
        {
            if (HttpSessionMock.ReferenceEquals(null, other))
            {
                return false;
            }

            return HttpSessionMock.ReferenceEquals(this, other) || Equals(other.keyValues, this.keyValues);
        }
    }
}
