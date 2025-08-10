using System;

namespace NikosAssets.Pooling
{
    public interface IPoolItem
    {
        /// <summary>
        /// Make sure to call this!
        /// </summary>
        public event Action<IPoolItem> OnDestroyed;
        
        /// <summary>
        /// Determines if this pool item can be pooled or not (occupied)
        /// </summary>
        public bool IsOccupied { get; }
        
        /// <summary>
        /// Useful if you don't accidentally want to add this pool item into your own poolcontainer!
        /// </summary>
        public bool IsUsedByMarkers { get; set; }

        /// <summary>
        /// Not occupied anymore or the time is up for clocked (short-lived) poolitems like projectiles or collectables
        /// </summary>
        public void Deactivate();
        
        /// <summary>
        /// Gets occupied and reset, ready for action!
        /// </summary>
        public void PoolingReset();
    }
}
