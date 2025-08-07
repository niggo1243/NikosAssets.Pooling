using System;
using System.Threading;
using NikosAssets.Helpers;

namespace NikosAssets.Pooling
{
    public class SimplePoolItemMono : BaseNotesMono, IPoolItem
    {
        public event Action<IPoolItem> OnDestroyed;


        public virtual bool IsOccupied => isActiveAndEnabled;
        
        public bool IsUsedByMarkers { get; set; }

        public CancellationTokenSource CancellationTokenSource { get; } = new CancellationTokenSource();
        
        protected virtual void Awake()
        {
        }

        protected virtual void OnDestroy()
        {
            //probably because the world is unloading safety check
            CancellationTokenSource.Cancel();
            
            OnDestroyed?.Invoke(this);
        }
        
        public virtual void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public virtual void PoolingReset()
        {
            gameObject.SetActive(true);
        }
    }
}