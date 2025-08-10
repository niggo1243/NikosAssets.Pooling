
namespace NikosAssets.Pooling.Marker
{
    /// <summary>
    /// A ready to use class that marks an object for a pool item
    /// </summary>
    public class SimplePoolItemMarker : BasePoolItemMarker<SimplePoolMarkerManager, SimplePoolItemMarker>
    {
        protected override void OnEnable()
        {
            SimplePoolMarkerManager.Instance.PoolMarkerEnabled(this);
        }

        protected override void OnDisable()
        {
            SimplePoolMarkerManager.Instance.PoolMarkerDisabled(this, DesignatedPoolItem);
        }
    }
}
