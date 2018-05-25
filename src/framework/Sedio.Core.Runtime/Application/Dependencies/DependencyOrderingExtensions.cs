using System;
using System.Collections.Generic;
using System.Linq;
using Sedio.Core.Algorithms;

namespace Sedio.Core.Runtime.Application.Dependencies
{
    public static class DependencyOrderingExtensions
    {
        public static IEnumerable<T> OrderByDependencies<T>(this IEnumerable<T> items)
            where T : class
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            var nodes = items.Distinct().ToHashSet();
            var edges = items.SelectMany(GetEdges).Select(item => ToEdge(item.Item1, item.Item2, items)).ToHashSet();

            return TopologicalSort.DestructiveOptimized(nodes, edges);
        }

        private static IEnumerable<(T, DependentOnAttribute)> GetEdges<T>(T source)
        {
            var dependencyOnAttributes = Attribute.GetCustomAttributes(source.GetType(), typeof(DependentOnAttribute));

            if (dependencyOnAttributes != null)
            {
                return dependencyOnAttributes.Cast<DependentOnAttribute>().Select(d => (source, d));
            }

            return Enumerable.Empty<(T, DependentOnAttribute)>();
        }

        private static (T, T) ToEdge<T>(T source, DependentOnAttribute sourceAttribute, IEnumerable<T> allItems)
        {
            var target = allItems.FirstOrDefault(item => item.GetType() == sourceAttribute.DependencyType);

            if (target == null)
            {
                throw new DependencyException($"Dependency not found: {sourceAttribute.DependencyType.Name}");
            }

            return (source, target);
        }
    }
}