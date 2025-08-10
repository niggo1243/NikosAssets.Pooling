using NaughtyAttributes;
using NikosAssets.Helpers;

namespace NikosAssets.Pooling.Marker
{
    /// <summary>
    /// Used by the <typeparamref name="EditorPoolMarkerBaker"/> for automated purposes only (not needed at runtime or in the build!)
    /// </summary>
    public abstract class BasePoolItemPreview : BaseNotesMono
    {
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        public string poolContainerKey;

        protected virtual void Start()
        {
            //not needed at runtime!
            Destroy(this);
        }
    }
}
