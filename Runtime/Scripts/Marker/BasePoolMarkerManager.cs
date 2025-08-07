using System;
using System.Linq;
using NaughtyAttributes;
using NikosAssets.Helpers;
using UnityEngine;

namespace NikosAssets.Pooling.Marker
{
    public abstract class BasePoolMarkerManager<TPoolItem, TPoolContainer, TPoolItemMarker, TPoolMarkerManager> : BasePoolMarkerManagerWrapper<TPoolMarkerManager, TPoolItemMarker>, IDisposable
        where TPoolItem : Component, IPoolItem
        where TPoolContainer : PoolContainer<TPoolItem>, new()
        where TPoolItemMarker : BasePoolItemMarker<TPoolMarkerManager, TPoolItemMarker>
        where TPoolMarkerManager : BasePoolMarkerManager<TPoolItem, TPoolContainer, TPoolItemMarker, TPoolMarkerManager>
    {
        [Serializable]
        public class PoolItemPrefabData
        {
            public int prewarmAmount = 0;
            public TPoolItem prefab;
            public Transform parent;
            public string key;
        }

        [SerializeField]
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        protected bool _initOnAwake = true;

        [SerializeField]
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        protected PoolItemPrefabData[] _poolItemPrefabData = Array.Empty<PoolItemPrefabData>();
        
        protected PoolManager _poolManager;
        protected bool _initialized;

        protected override void Awake()
        {
            base.Awake();
            
            if (_initOnAwake)
                Init();
        }

        protected virtual void OnDestroy()
        {
            Dispose();
        }

        public virtual void Init()
        {
            if (_initialized)
                return;
            
            _poolManager = new PoolManager();

            foreach (PoolItemPrefabData data in _poolItemPrefabData)
            {
                //register pool container with the key
                TPoolContainer poolContainer = new TPoolContainer {Prefab = data.prefab};
                _poolManager.RegisterPoolContainer(data.key, poolContainer);

                //prewarm
                for (int i = 0; i < data.prewarmAmount; i++)
                {
                    TPoolItem poolItemInstance = Instantiate(data.prefab, data.parent);
                    poolItemInstance.IsUsedByMarkers = true;
                    poolItemInstance.Deactivate();
                    poolContainer.AddItem(poolItemInstance);
                }
            }

            _initialized = true;
        }
        
        public virtual void Dispose()
        {
            // if (!_initialized)
            //     return;
        }
        
        public override void PoolMarkerEnabled(TPoolItemMarker poolItemMarker)
        {
            if (!_poolManager.TryGetPoolContainer<TPoolContainer>(poolItemMarker.poolContainerKey,
                out var poolContainer))
            {
                Debug.LogError($"{poolItemMarker.poolContainerKey} was not found on poolmarker enable!");
                return;
            }

            var data = _poolItemPrefabData.First(d => d.key.Equals(poolItemMarker.poolContainerKey));
            
            var poolItem = poolContainer.ReuseOrSpawnPoolItem(data.parent);
            poolItem.IsUsedByMarkers = true;
            
            poolContainer.RemoveItem(poolItem);
            poolItemMarker.DesignatedPoolItem = poolItem;
        }
        
        public override void PoolMarkerDisabled(TPoolItemMarker poolItemMarker, IPoolItem poolItem)
        {
            if (!_poolManager.TryGetPoolContainer<TPoolContainer>(poolItemMarker.poolContainerKey, out var poolContainer))
            {
                Debug.LogError($"{poolItemMarker.poolContainerKey} was not found on poolmarker disable!");
                return;
            }            
            poolItem.Deactivate();
            poolContainer.AddItem((TPoolItem) poolItem);
            poolItemMarker.DesignatedPoolItem = null;
        }
        
        
#if UNITY_EDITOR
        
        [SerializeField]
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_EDITORONLY)]
        private int _editorSetPrewarmAmountForAll = 100;

        [Button("Set Prewarm Amount For All", EButtonEnableMode.Editor)]
        private void Button_SetPrewarmAmount()
        {
            foreach (var poolItemPrefabData in _poolItemPrefabData)
            {
                poolItemPrefabData.prewarmAmount = _editorSetPrewarmAmountForAll;
            }
            
            UnityEditor.EditorUtility.SetDirty(this);
        }
        
#endif
    }
}
