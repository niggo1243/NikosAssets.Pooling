using NikosAssets.Helpers;

namespace NikosAssets.Pooling
{
    public interface IPoolItemOnClock : IPoolItem
    {
        /// <summary>
        /// Make sure to set, init this and additionally reset on pooling reset!
        /// </summary>
        public TimingHelper PoolCooldownToDeactivate { get; }
    }
}
