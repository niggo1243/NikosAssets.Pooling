using System;

namespace NikosAssets.Pooling
{
    public interface IPoolItem
    {
        /// <summary>
        /// Make sure to call this!
        /// </summary>
        public event Action<IPoolItem> OnDestroyed;
        
        public bool IsOccupied { get; }
        
        public bool IsUsedByMarkers { get; set; }

        public void Deactivate();
        
        public void PoolingReset();
    }
}
