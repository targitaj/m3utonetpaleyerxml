namespace Uma.Eservices.DbAccess
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Used for Entity stroing adding filtering
    /// </summary>
    /// <typeparam name="TEntity">Entity used in memory set</typeparam>
    public sealed class InMemoryDbSet<TEntity> : IDbSet<TEntity>, IOrderedQueryable<TEntity>, IListSource where TEntity : class
    {
        /// <summary>
        /// Set ot entities
        /// </summary>
        private readonly HashSet<TEntity> set;
        
        /// <summary>
        /// Creates an instance of the <see cref="InMemoryDbSet"/> class.
        /// </summary>
        public InMemoryDbSet()
        {
            this.set = new HashSet<TEntity>();
        }

        /// <summary>
        /// Adding entity
        /// </summary>
        /// <param name="entity">Entity to add</param>
        public TEntity Add(TEntity entity)
        {
            this.set.Add(entity);
            return entity;
        }

        /// <summary>
        /// Attaching entity
        /// </summary>
        /// <param name="entity">Entity to attack</param>
        public TEntity Attach(TEntity entity)
        {
            this.set.Add(entity);
            return entity;
        }

        /// <summary>
        /// Entity creation
        /// </summary>
        public TEntity Create()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Derived entity creation
        /// </summary>
        /// <typeparam name="TDerivedEntity">Derived Entity</typeparam>
        public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, TEntity
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finding of entity
        /// </summary>
        /// <param name="keyValues">Finding keys</param>
        public TEntity Find(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Remove entity from set
        /// </summary>
        /// <param name="entity">Entity to remove</param>
        public TEntity Remove(TEntity entity)
        {
            this.set.Remove(entity);
            return entity;
        }

        /// <summary>
        /// Returns the items in the local cache
        /// </summary>
        public ObservableCollection<TEntity> Local { get { return new ObservableCollection<TEntity>(this.set); } }

        /// <summary>
        /// Get enumerator
        /// </summary>
        IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator() { return this.set.GetEnumerator(); }

        /// <summary>
        /// Gets enumerator
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator() { return this.set.GetEnumerator(); }

        /// <summary>
        /// Gets type of Entity
        /// </summary>
        Type IQueryable.ElementType { get { return typeof(TEntity); } }

        /// <summary>
        /// Gets Experssion of set
        /// </summary>
        Expression IQueryable.Expression { get { return this.set.AsQueryable().Expression; } }

        /// <summary>
        /// Get Provider
        /// </summary>
        IQueryProvider IQueryable.Provider { get { return this.set.AsQueryable().Provider; } }

        /// <summary>
        /// Get ContainsListCollection
        /// </summary>
        bool IListSource.ContainsListCollection { get { return false; } }

        /// <summary>
        /// Gets List of entities
        /// </summary>
        IList IListSource.GetList() { throw new InvalidOperationException(); }
    }
}