using System;
using System.Collections.Generic;
using System.Linq;

namespace Sedio.Core.Algorithms
{
    public static class TopologicalSort
    {
        public static List<T>  DestructiveOptimized<T>(HashSet<T> nodes, HashSet<(T, T)> edges) where T : IEquatable<T> 
        {
            if (nodes == null) throw new ArgumentNullException(nameof(nodes));
            if (edges == null) throw new ArgumentNullException(nameof(edges));

            var result = new List<T>();
            var rootNodes = new HashSet<T>(nodes.Where(n => edges.All(e => e.Item2.Equals(n) == false)));

            while (rootNodes.Any()) 
            {
                var n = rootNodes.First();
                rootNodes.Remove(n);
                result.Add(n);

                foreach (var e in edges.Where(e => e.Item1.Equals(n)).ToList()) 
                {                 
                    var m = e.Item2;
                    edges.Remove(e);

                    if (edges.All(me => me.Item2.Equals(m) == false)) 
                    {
                        rootNodes.Add(m);
                    }
                }
            }
       
            return edges.Any() ? null : result;
        }
    }
}