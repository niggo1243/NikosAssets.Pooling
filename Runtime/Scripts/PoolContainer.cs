using System;
using System.Collections.Generic;
using UnityEngine;

namespace NikosAssets.Pooling
{
    public class PoolContainer<TPoolItem> : IPoolContainer
    where TPoolItem : Component, IPoolItem
    {
        protected List<TPoolItem> _poolList = new List<TPoolItem>();
        public List<TPoolItem> PoolList => _poolList;
        
        public TPoolItem Prefab { get; set; }

        public PoolContainer()
        {
        }
        
        public PoolContainer(TPoolItem prefab)
        {
            Prefab = prefab;
        }

        public virtual void Dispose()
        {
        }
        
        protected virtual void OnPoolItemDestroyed(IPoolItem poolItem)
        {
            RemoveItem((TPoolItem) poolItem);
        }

        public TPoolItem ReuseOrSpawnPoolItem(TPoolItem prefab, Transform parent, Predicate<TPoolItem> match = null)
        {
            TPoolItem poolItem = null;
            if (TryReusePoolItem(out poolItem, match))
                return poolItem;

            poolItem = Component.Instantiate(prefab, parent);

            return poolItem;
        }
        
        public TPoolItem ReuseOrSpawnPoolItem(Transform parent, Predicate<TPoolItem> match = null)
        {
            return ReuseOrSpawnPoolItem(Prefab, parent, match);
        }

        public bool TryReusePoolItem(out TPoolItem poolItem, Predicate<TPoolItem> match = null)
        {
            int i = _poolList.FindIndex(p => 
                !p.IsOccupied 
                && (match == null || match.Invoke(p)));

            if (i > -1)
            {
                poolItem = _poolList[i];
                poolItem.PoolingReset();
                return true;
            }

            poolItem = null;
            return false;
        }

        public List<TPoolItem> ReusePoolItems(Predicate<TPoolItem> match = null)
        {
            return _poolList.FindAll(p =>
            {
                if (!p.IsOccupied)
                    return false;

                if (match != null && !match.Invoke(p))
                    return false;
                
                p.PoolingReset();
                return true;
            });
        }

        public virtual void AddItem(TPoolItem item)
        {
            _poolList.Add(item);

            item.OnDestroyed += OnPoolItemDestroyed;
        }

        public virtual bool RemoveItem(TPoolItem item)
        {
            item.OnDestroyed -= OnPoolItemDestroyed;
            return _poolList.Remove(item);
        }

        public void RemoveItem(int index)
        {
            _poolList[index].OnDestroyed -= OnPoolItemDestroyed;
            _poolList.RemoveAt(index);
        }
    }
}
