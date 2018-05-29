using System;
using System.Collections.Generic;
using System.Linq;

namespace Sedio.Core.Algorithms
{
    public static class TopologicalSort
    {
        public static List<T>  DestructiveOptimized<T>(HashSet<T> nodes, HashSet<(T, T)> edges)
        {
            if (nodes == null) throw new ArgumentNullException(nameof(nodes));
            if (edges == null) throw new ArgumentNullException(nameof(edges));

            var result = new List<T>();
            var rootNodes = new HashSet<T>(nodes.Where(n => edges.All(e => nodes.Comparer.Equals(e.Item2,n) == false)));

            while (rootNodes.Any()) 
            {
                var n = rootNodes.First();
                rootNodes.Remove(n);
                result.Add(n);

                // TODO: Get rid of allocationsh
                foreach (var e in edges.Where(e => nodes.Comparer.Equals(e.Item1,n)).ToList()) 
                {                 
                    var m = e.Item2;
                    edges.Remove(e);

                    if (edges.All(me => nodes.Comparer.Equals(me.Item2,m) == false)) 
                    {
                        rootNodes.Add(m);
                    }
                }
            }
       
            return edges.Any() ? throw new CircularPathException() : result;
        }
    }
}