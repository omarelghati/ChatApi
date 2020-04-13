using ChatApi.Uitilities;
using System.Collections.Generic;

namespace ChatApi.Hubs
{
    public class ConnectionMapping<T>
    {
        private readonly Dictionary<T, string> _connections =
            new Dictionary<T, string>();

        public int Count
        {
            get
            {
                return _connections.Count;
            }
        }

        public void Add(T key, string connectionId)
        {
            if (!_connections.TryGetValue(key, out connectionId))
            {
                _connections.Add(key, connectionId);
            }
        }

        public string GetConnections(T key)
        {
            string connections;
            lock (_connections)
            {
                if (_connections.TryGetValue(key, out connections))
                {
                    return connections;
                }
            }
            return null;
        }

        public void Remove(T key, string connectionId)
        {
            if (key.NotNull() && _connections.ContainsKey(key))
            {
                _connections.Remove(key);
            }
        }
    }
}