using System;
using System.Collections.Generic;

namespace Sedio.Core.Networking
{
    public readonly struct WellKnownProtocol : IEquatable<WellKnownProtocol>
    {
        public WellKnownProtocol(string id, int port)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(id));
            Id = id;
            Port = port;
        }

        public string Id { get; }
        
        public int Port { get; }

        public bool Equals(WellKnownProtocol other)
        {
            return string.Equals(Id, other.Id, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is WellKnownProtocol && Equals((WellKnownProtocol) obj);
        }

        public override int GetHashCode()
        {
            return StringComparer.InvariantCultureIgnoreCase.GetHashCode(Id);
        }

        public static bool operator ==(WellKnownProtocol left, WellKnownProtocol right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(WellKnownProtocol left, WellKnownProtocol right)
        {
            return !left.Equals(right);
        }
    }

    public static class WellKnownProtocols
    {
        private static readonly Dictionary<string, WellKnownProtocol> protocolsById
            = new Dictionary<string, WellKnownProtocol>(StringComparer.InvariantCultureIgnoreCase);

        private static readonly Dictionary<int,WellKnownProtocol> protocolsByPort
            = new Dictionary<int, WellKnownProtocol>();    
        
        static WellKnownProtocols()
        {
            Define("http",80);
            Define("https",443);
        }

        public static IReadOnlyCollection<WellKnownProtocol> All => protocolsById.Values;

        public static bool TryGetProtocolByPort(int port, out WellKnownProtocol protocol)
        {
            return protocolsByPort.TryGetValue(port, out protocol);
        }
        
        public static bool TryGetProtocolById(string id, out WellKnownProtocol protocol)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(id));
            
            return protocolsById.TryGetValue(id, out protocol);
        }

        public static string ResolveProtocolId(int? port,string protocolId)
        {
            if (protocolId != null)
            {
                return protocolId;
            }

            if (port != null && TryGetProtocolByPort(port.Value, out var protocol))
            {
                return protocol.Id;
            }

            return null;
        }

        public static int? ResolveProtocolPort(string protocolId,int? port)
        {
            if (string.IsNullOrWhiteSpace(protocolId))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(protocolId));
            
            if (port != null)
            {
                return port;
            }

            if (TryGetProtocolById(protocolId, out var protocol))
            {
                return protocol.Port;
            }

            return null;
        }
        
        private static void Define(string id, int port)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(id));
            
            protocolsById[id] = new WellKnownProtocol(id,port);
            protocolsByPort[port] = new WellKnownProtocol(id,port);
        }
    }
}