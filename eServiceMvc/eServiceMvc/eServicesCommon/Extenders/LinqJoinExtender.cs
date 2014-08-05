namespace Uma.Eservices.Common.Extenders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Class helps to create different kind of Join queries.
    /// For more info:
    /// http://www.codeproject.com/Articles/488643/LinQ-Extended-Joins
    /// </summary>
    public static partial class LinqJoinExtender
    {
        /// <summary>
        /// Correlates the elements of two sequences based on matching keys. The default
        /// equality comparer is used to compare keys.
        /// </summary>
        /// <typeparam name="TSource"> The type of the elements of the first sequence</typeparam>
        /// <typeparam name="TInner"> The type of the elements of the second sequence</typeparam>
        /// <typeparam name="TKey"> The type of the keys returned by the key selector functions.</typeparam>
        /// <typeparam name="TResult"> The type of the result elements.</typeparam>
        /// <param name="source"> The first sequence to join.</param>
        /// <param name="inner"> The sequence to join to the first sequence.</param>
        /// <param name="pk" > A function to extract the join key from each element of the first sequence.</param>
        /// <param name="fk"> A function to extract the join key from each element of the second sequence.</param>
        /// <param name="result"> A function to create a result element from two matching elements.</param>
        /// <returns>
        /// An System.Collections.Generic.IEnumerable that has elements of type TResult
        /// that are obtained by performing an inner join on two sequences.
        /// </returns>
        public static IEnumerable<TResult> LeftJoin<TSource, TInner, TKey, TResult>(this IEnumerable<TSource> source,
                                                 IEnumerable<TInner> inner,
                                                 Func<TSource, TKey> pk,
                                                 Func<TInner, TKey> fk,
                                                 Func<TSource, TInner, TResult> result)
        {
            IEnumerable<TResult> resultVal = Enumerable.Empty<TResult>();

            resultVal = from s in source
                        join i in inner
                        on pk(s) equals fk(i) into joinData
                        from left in joinData.DefaultIfEmpty()
                        select result(s, left);

            return resultVal;
        }

        /// <summary>
        /// Correlates the elements of two sequences based on matching keys. The default
        /// equality comparer is used to compare keys.
        /// </summary>
        /// <typeparam name="TSource"> The type of the elements of the first sequence</typeparam>
        /// <typeparam name="TInner"> The type of the elements of the second sequence</typeparam>
        /// <typeparam name="TKey"> The type of the keys returned by the key selector functions.</typeparam>
        /// <typeparam name="TResult"> The type of the result elements.</typeparam>
        /// <param name="source"> The first sequence to join.</param>
        /// <param name="inner"> The sequence to join to the first sequence.</param>
        /// <param name="pk" >A function to extract the join key from each element of the first sequence.</param>
        /// <param name="fk"> A function to extract the join key from each element of the second sequence.</param>
        /// <param name="result"> A function to create a result element from two matching elements.</param>
        /// <returns>
        /// An System.Collections.Generic.IEnumerable that has elements of type TResult
        /// that are obtained by performing an inner join on two sequences.
        /// </returns>
        public static IEnumerable<TResult> RightJoin<TSource, TInner, TKey, TResult>(this IEnumerable<TSource> source,
                                                  IEnumerable<TInner> inner,
                                                  Func<TSource, TKey> pk,
                                                  Func<TInner, TKey> fk,
                                                  Func<TSource, TInner, TResult> result)
        {
            IEnumerable<TResult> resultVal = Enumerable.Empty<TResult>();

            resultVal = from i in inner
                        join s in source
                        on fk(i) equals pk(s) into joinData
                        from right in joinData.DefaultIfEmpty()
                        select result(right, i);

            return resultVal;
        }

        /// <summary>
        /// Correlates the elements of two sequences based on matching keys. The default
        /// equality comparer is used to compare keys.
        /// </summary>
        /// <typeparam name="TSource"> The type of the elements of the first sequence</typeparam>
        /// <typeparam name="TInner"> The type of the elements of the second sequence</typeparam>
        /// <typeparam name="TKey"> The type of the keys returned by the key selector functions.</typeparam>
        /// <typeparam name="TResult"> The type of the result elements.</typeparam>
        /// <param name="source"> The first sequence to join.</param>
        /// <param name="inner"> The sequence to join to the first sequence.</param>
        /// <param name="pk" >A function to extract the join key from each element of the first sequence.</param>
        /// <param name="fk"> A function to extract the join key from each element of the second sequence.</param>
        /// <param name="result"> A function to create a result element from two matching elements.</param>
        /// <returns>
        /// An System.Collections.Generic.IEnumerable that has elements of type TResult
        /// that are obtained by performing an inner join on two sequences.
        /// </returns>
        public static IEnumerable<TResult> FullOuterJoin<TSource, TInner, TKey, TResult>(this IEnumerable<TSource> source,
                                                          IEnumerable<TInner> inner,
                                                          Func<TSource, TKey> pk,
                                                          Func<TInner, TKey> fk,
                                                          Func<TSource, TInner, TResult> result)
        {
            var left = source.LeftJoin(inner, pk, fk, result).ToList();
            var right = source.RightJoin(inner, pk, fk, result).ToList();

            return left.Union(right);
        }

        /// <summary>
        /// Correlates the elements of two sequences based on matching keys. The default
        /// equality comparer is used to compare keys.
        /// </summary>
        /// <typeparam name="TSource"> The type of the elements of the first sequence</typeparam>
        /// <typeparam name="TInner"> The type of the elements of the second sequence</typeparam>
        /// <typeparam name="TKey"> The type of the keys returned by the key selector functions.</typeparam>
        /// <typeparam name="TResult"> The type of the result elements.</typeparam>
        /// <param name="source"> The first sequence to join.</param>
        /// <param name="inner"> The sequence to join to the first sequence.</param>
        /// <param name="pk" >A function to extract the join key from each element of the first sequence.</param>
        /// <param name="fk"> A function to extract the join key from each element of the second sequence.</param>
        /// <param name="result"> A function to create a result element from two matching elements.</param>
        /// <returns>
        /// An System.Collections.Generic.IEnumerable that has elements of type TResult
        /// that are obtained by performing an inner join on two sequences.
        /// </returns>
        public static IEnumerable<TResult> LeftExcludingJoin<TSource, TInner, TKey, TResult>(this IEnumerable<TSource> source,
                                                          IEnumerable<TInner> inner,
                                                          Func<TSource, TKey> pk,
                                                          Func<TInner, TKey> fk,
                                                          Func<TSource, TInner, TResult> result)
        {
            IEnumerable<TResult> resultVal = Enumerable.Empty<TResult>();

            resultVal = from s in source
                        join i in inner
                        on pk(s) equals fk(i) into joinData
                        from left in joinData.DefaultIfEmpty()
                        where left == null
                        select result(s, left);

            return resultVal;
        }

        /// <summary>
        /// Correlates the elements of two sequences based on matching keys. The default
        /// equality comparer is used to compare keys.
        /// </summary>
        /// <typeparam name="TSource"> The type of the elements of the first sequence</typeparam>
        /// <typeparam name="TInner"> The type of the elements of the second sequence</typeparam>
        /// <typeparam name="TKey"> The type of the keys returned by the key selector functions.</typeparam>
        /// <typeparam name="TResult"> The type of the result elements.</typeparam>
        /// <param name="source"> The first sequence to join.</param>
        /// <param name="inner"> The sequence to join to the first sequence.</param>
        /// <param name="pk" >A function to extract the join key from each element of the first sequence.</param>
        /// <param name="fk"> A function to extract the join key from each element of the second sequence.</param>
        /// <param name="result"> A function to create a result element from two matching elements.</param>
        /// <returns>
        /// An System.Collections.Generic.IEnumerable that has elements of type TResult
        /// that are obtained by performing an inner join on two sequences.
        /// </returns>
        public static IEnumerable<TResult> RightExcludingJoin<TSource, TInner, TKey, TResult>(this IEnumerable<TSource> source,
                                                        IEnumerable<TInner> inner,
                                                        Func<TSource, TKey> pk,
                                                        Func<TInner, TKey> fk,
                                                        Func<TSource, TInner, TResult> result)
        {
            IEnumerable<TResult> resultVal = Enumerable.Empty<TResult>();

            resultVal = from i in inner
                        join s in source
                        on fk(i) equals pk(s) into joinData
                        from right in joinData.DefaultIfEmpty()
                        where right == null
                        select result(right, i);

            return resultVal;
        }

        /// <summary>
        /// Correlates the elements of two sequences based on matching keys. The default
        /// equality comparer is used to compare keys.
        /// </summary>
        /// <typeparam name="TSource"> The type of the elements of the first sequence</typeparam>
        /// <typeparam name="TInner"> The type of the elements of the second sequence</typeparam>
        /// <typeparam name="TKey"> The type of the keys returned by the key selector functions.</typeparam>
        /// <typeparam name="TResult"> The type of the result elements.</typeparam>
        /// <param name="source"> The first sequence to join.</param>
        /// <param name="inner"> The sequence to join to the first sequence.</param>
        /// <param name="pk" >A function to extract the join key from each element of the first sequence.</param>
        /// <param name="fk"> A function to extract the join key from each element of the second sequence.</param>
        /// <param name="result"> A function to create a result element from two matching elements.</param>
        /// <returns>
        /// An System.Collections.Generic.IEnumerable that has elements of type TResult
        /// that are obtained by performing an inner join on two sequences.
        /// </returns>
        public static IEnumerable<TResult> FulltExcludingJoin<TSource, TInner, TKey, TResult>(this IEnumerable<TSource> source,
                                                      IEnumerable<TInner> inner,
                                                      Func<TSource, TKey> pk,
                                                      Func<TInner, TKey> fk,
                                                      Func<TSource, TInner, TResult> result)
        {
            var left = source.LeftExcludingJoin(inner, pk, fk, result).ToList();
            var right = source.RightExcludingJoin(inner, pk, fk, result).ToList();

            return left.Union(right);
        }
    }
}
