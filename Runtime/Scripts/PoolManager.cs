using System.Collections.Generic;

namespace NikosAssets.Pooling
{
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
