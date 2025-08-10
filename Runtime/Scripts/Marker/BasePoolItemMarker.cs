using NaughtyAttributes;
using NikosAssets.Helpers;

namespace NikosAssets.Pooling.Marker
{
    /// <summary>
    /// Use this to pool environment props or entities OnEnable and set them free again OnDisable (see the Readme.md for an example use case)
    /// </summary>
    /// <typeparam name="TPoolMarkerManager"></typeparam>
    /// <typeparam name="TPoolItemMarker"></typeparam>
    public abstract class BasePoolItemMarker<TPoolMarkerManager, TPoolItemMarker> : BaseNotesMono
        where TPoolMarkerManager : BasePoolMarkerManagerWrapper<TPoolMarkerManager, TPoolItemMarker>
        where TPoolItemMarker : BasePoolItemMarker<TPoolMarkerManager, TPoolItemMarker>
    {
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        public string poolContainerKey;

        public IPoolItem DesignatedPoolItem { get; set; }
        
        protected abstract void OnEnable();

        protected abstract void OnDisable();
    }
}
