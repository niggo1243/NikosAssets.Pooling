using System.Collections.Generic;

namespace NikosAssets.Pooling
{
    /// <summary>
    /// Manages a bunch of pool containers separated by pool keys 
    /// </summary>
    public class PoolManager
    {
        private Dictionary<string, IPoolContainer> _poolContainersMap = new Dictionary<string, IPoolContainer>();

        public bool HasPoolContainer(string key) => _poolContainersMap.ContainsKey(key);
        
        public bool TryRegisterPoolContainer(string key, IPoolContainer poolContainer)
        {
            if (poolContainer == default)
                return false;

            if (string.IsNullOrEmpty(key))
                return false;
            
            if (_poolContainersMap.ContainsKey(key))
                return false;

            RegisterPoolContainer(key, poolContainer);
            return true;
        }
        
        public void RegisterPoolContainer(string key, IPoolContainer poolContainer)
        {
            _poolContainersMap.Add(key, poolContainer);
        }

        public bool TryUnregisterPoolContainer(string key)
        {
            if (!_poolContainersMap.ContainsKey(key))
                return false;
            
            UnregisterPoolContainer(key);
            return true;
        }
        
        public void UnregisterPoolContainer(string key)
        {
            _poolContainersMap.Remove(key);
        }

        public bool TryGetPoolContainer<TPoolContainer>(string key, out TPoolContainer poolContainer)
            where TPoolContainer : IPoolContainer
        {
            if (_poolContainersMap.TryGetValue(key, out IPoolContainer pc))
            {
                poolContainer = (TPoolContainer) pc;
                return true;
            }

            poolContainer = default(TPoolContainer);
            return false;
        }
    }
}
