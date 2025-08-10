using NikosAssets.Helpers;

namespace NikosAssets.Pooling
{
    /// <summary>
    /// A short-lived pool item that gets deactivated over some time
    /// </summary>
    public interface IPoolItemOnClock : IPoolItem
    {
        /// <summary>
        /// Make sure to set, init this and additionally reset on pooling reset!
        /// </summary>
        public TimingHelper PoolCooldownToDeactivate { get; }
    }
}
