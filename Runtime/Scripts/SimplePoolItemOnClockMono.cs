using NaughtyAttributes;
using NikosAssets.Helpers;
using UnityEngine;

namespace NikosAssets.Pooling
{
    public class SimplePoolItemOnClockMono : SimplePoolItemMono, IPoolItemOnClock
    {
        [SerializeField]
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        protected TimingHelper _poolCooldownToDeactivate =
            new TimingHelper(TimingHelper.TimerType.Seconds, Vector2.one * 3);

        public TimingHelper PoolCooldownToDeactivate => _poolCooldownToDeactivate;

        protected override void Awake()
        {
            base.Awake();
            PoolCooldownToDeactivate.ResetRunningTime();
            PoolCooldownToDeactivate.Init();
        }

        public override void PoolingReset()
        {
            PoolCooldownToDeactivate.ResetRunningTime();
            base.PoolingReset();
        }
    }
}
