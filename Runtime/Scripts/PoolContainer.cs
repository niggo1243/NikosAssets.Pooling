using System;
using System.Collections.Generic;
using UnityEngine;

namespace NikosAssets.Pooling
{
    /// <summary>
    /// Basic implementation of the <typeparamref name="IPoolContainer"/>
    /// </summary>
    /// <typeparam name="TPoolItem"></typeparam>
    public class PoolContainer<TPoolItem> : IPoolContainer
    where TPoolItem : Component, IPoolItem
    {
        protected List<TPoolItem> _poolList = new List<TPoolItem>();
        public List<TPoolItem> PoolList => _poolList;
        
        /// <summary>
        /// The prefab to spawn if no poolable (free) item exists
        /// </summary>
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

        /// <summary>
        /// Pool or instantiate a new poolitem depending if any are free to use.
        /// Does not add the poolitem automatically to this container!
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="parent"></param>
        /// <param name="match">
        /// Optional function to check for more conditions on poolable items
        /// </param>
        /// <returns>
        /// A poolitem where the PoolingReset() function has already been called on a pooled one only 
        /// </returns>
        public TPoolItem ReuseOrSpawnPoolItem(TPoolItem prefab, Transform parent, Predicate<TPoolItem> match = null)
        {
            TPoolItem poolItem = null;
            if (TryReusePoolItem(out poolItem, match))
                return poolItem;

            poolItem = Component.Instantiate(prefab, parent);

            return poolItem;
        }
        
        /// <summary>
        /// Pool or instantiate a new poolitem depending if any are free to use
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="match">
        /// Optional function to check for more conditions on poolable items
        /// </param>
        /// <returns>
        /// A poolitem where the PoolingReset() function has already been called on a pooled one only 
        /// </returns>
        public TPoolItem ReuseOrSpawnPoolItem(Transform parent, Predicate<TPoolItem> match = null)
        {
            return ReuseOrSpawnPoolItem(Prefab, parent, match);
        }

        /// <summary>
        /// Gets a poolitem if any free are available
        /// </summary>
        /// <param name="poolItem">
        /// null if no poolable item has been found
        /// </param>
        /// <param name="match">
        /// Optional function to check for more conditions on poolable items
        /// </param>
        /// <returns>
        /// true if a poolable item has been found and pooled out
        /// </returns>
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

        /// <summary>
        /// Get as many pooled items that can be used as possible (does not instantiate new ones)
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Add a poolitem to this container (will automatically be removed when the item gets destroyed)
        /// </summary>
        /// <param name="item"></param>
        public virtual void AddItem(TPoolItem item)
        {
            _poolList.Add(item);

            item.OnDestroyed += OnPoolItemDestroyed;
        }

        /// <summary>
        /// Remove the poolitem from this container
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual bool RemoveItem(TPoolItem item)
        {
            item.OnDestroyed -= OnPoolItemDestroyed;
            return _poolList.Remove(item);
        }

        /// <summary>
        /// Remove the poolitem from this container by index
        /// </summary>
        /// <param name="index"></param>
        public void RemoveItem(int index)
        {
            _poolList[index].OnDestroyed -= OnPoolItemDestroyed;
            _poolList.RemoveAt(index);
        }
    }
}
