using System;
using System.Collections.Generic;
using System.Linq;

namespace Sedio.Core.Collections.Immutable
{
    /// <summary>Extension methods providing basic operations on a list.</summary>
    public static class ImList
    {
        /// <summary>This a basically a Fold function, to address needs in Map, Filter, Reduce.</summary>
        /// <typeparam name="T">Type of list item.</typeparam>
        /// <typeparam name="R">Type of result.</typeparam>
        /// <param name="source">List to fold.</param>
        /// <param name="initialValue">From were to start.</param>
        /// <param name="collect">Collects list item into result</param>
        /// <returns>Return result or <paramref name="initialValue"/> for empty list.</returns>
        public static R To<T, R>(this ImList<T> source, R initialValue, Func<T, R, R> collect)
        {
            if (source.IsEmpty)
                return initialValue;
            var value = initialValue;
            for (; !source.IsEmpty; source = source.Tail)
                value = collect(source.Head, value);
            return value;
        }

        /// <summary>Form of fold function with element index for convenience.</summary>
        /// <typeparam name="T">Type of list item.</typeparam>
        /// <typeparam name="R">Type of result.</typeparam>
        /// <param name="source">List to fold.</param>
        /// <param name="initialValue">From were to start.</param>
        /// <param name="collect">Collects list item into result</param>
        /// <returns>Return result or <paramref name="initialValue"/> for empty list.</returns>
        public static R To<T, R>(this ImList<T> source, R initialValue, Func<T, int, R, R> collect)
        {
            if (source.IsEmpty)
                return initialValue;
            var value = initialValue;
            for (var i = 0; !source.IsEmpty; source = source.Tail)
                value = collect(source.Head, i++, value);
            return value;
        }

        /// <summary>Returns new list in reverse order.</summary>
        /// <typeparam name="T">List item type</typeparam> <param name="source">List to reverse.</param>
        /// <returns>New list. If list consist on single element, then the same list.</returns>
        public static ImList<T> Reverse<T>(this ImList<T> source)
        {
            if (source.IsEmpty || source.Tail.IsEmpty)
                return source;
            return source.To(ImList<T>.Empty, (it, _) => _.Prep(it));
        }

        /// <summary>Maps the items from the first list to the result list.</summary>
        /// <typeparam name="T">source item type.</typeparam> 
        /// <typeparam name="R">result item type.</typeparam>
        /// <param name="source">input list.</param> <param name="map">converter func.</param>
        /// <returns>result list.</returns>
        public static ImList<R> Map<T, R>(this ImList<T> source, Func<T, R> map)
        {
            return source.To(ImList<R>.Empty, (it, _) => _.Prep(map(it))).Reverse();
        }

        /// <summary>Maps the items from the first list to the result list with item index.</summary>
        /// <typeparam name="T">source item type.</typeparam> 
        /// <typeparam name="R">result item type.</typeparam>
        /// <param name="source">input list.</param> <param name="map">converter func.</param>
        /// <returns>result list.</returns>
        public static ImList<R> Map<T, R>(this ImList<T> source, Func<T, int, R> map)
        {
            return source.To(ImList<R>.Empty, (it, i, _) => _.Prep(map(it, i))).Reverse();
        }

        /// <summary>Copies list to array.</summary> 
        /// <param name="source">list to convert.</param> 
        /// <returns>Array with list items.</returns>
        public static T[] ToArray<T>(this ImList<T> source)
        {
            if (source.IsEmpty)
                return new T[0];
            if (source.Tail.IsEmpty)
                return new[] { source.Head };
            return source.Enumerate().ToArray();
        }
    }

    /// <summary>Immutable list - simplest linked list with Head and Rest.</summary>
    /// <typeparam name="T">Type of the item.</typeparam>
    public sealed class ImList<T>
    {
        /// <summary>Empty list to Push to.</summary>
        public static readonly ImList<T> Empty = new ImList<T>();

        /// <summary>True for empty list.</summary>
        public bool IsEmpty => Tail == null;

        /// <summary>First value in a list.</summary>
        public readonly T Head;

        /// <summary>The rest of values or Empty if list has a single value.</summary>
        public readonly ImList<T> Tail;

        /// <summary>Prepends new value and returns new list.</summary>
        /// <param name="head">New first value.</param>
        /// <returns>List with the new head.</returns>
        public ImList<T> Prep(T head) => new ImList<T>(head, this);

        /// <summary>Enumerates the list.</summary>
        /// <returns>Each item in turn.</returns>
        public IEnumerable<T> Enumerate()
        {
            if (IsEmpty)
                yield break;
            for (var list = this; !list.IsEmpty; list = list.Tail)
                yield return list.Head;
        }

        #region Implementation

        private ImList() { }

        private ImList(T head, ImList<T> tail)
        {
            Head = head;
            Tail = tail;
        }

        #endregion
    }
}