using System.Collections.Generic;
using System.Linq;
using NikosAssets.Helpers;
using NikosAssets.Helpers.AlarmClock;
using UnityEngine;

namespace NikosAssets.Pooling
{
    public class PoolContainerOnClock<TPoolItemOnClock> : PoolContainer<TPoolItemOnClock>
        where TPoolItemOnClock : Component, IPoolItemOnClock
    {
        private BaseAlarmClock _alarmClock;

        public PoolContainerOnClock(TimingHelper timingHelper, Transform parent)
        {
            GameObject alarmClockGo = new GameObject($"PoolClock_{typeof(TPoolItemOnClock).Name}");
            alarmClockGo.transform.SetParent(parent);
            _alarmClock = alarmClockGo.AddComponent<AlarmClockMono>();
            _alarmClock.timer = timingHelper;
        }

        public PoolContainerOnClock(BaseAlarmClock alarmClock)
        {
            _alarmClock = alarmClock;
            _alarmClock.ResetTime();
        }
        
        public void Init()
        {
            _alarmClock.OnAlarm += Tick;
        }
        
        public override void Dispose()
        {
            base.Dispose();
            
            _alarmClock.OnAlarm -= Tick;
        }
        
        public List<TPoolItemOnClock> GetValidPoolItems()
        {
            return _poolList.FindAll(p => p.IsOccupied);
        }
        
        public void Tick()
        {
            foreach (TPoolItemOnClock poolItem 
                //TODO is this faster or slower then loop all and "if"? test it!
                in _poolList.Where(poolItem => poolItem.IsOccupied && poolItem.PoolCooldownToDeactivate.CheckRunningTime()))
                poolItem.Deactivate();
        }
        
        public override void AddItem(TPoolItemOnClock item)
        {
            base.AddItem(item);
            
            //TODO already requires CooldownTiming being set/ initialized/ not null!!!
            item.PoolCooldownToDeactivate.Init();
            item.PoolCooldownToDeactivate.ResetTimers();
        }
    }
}
