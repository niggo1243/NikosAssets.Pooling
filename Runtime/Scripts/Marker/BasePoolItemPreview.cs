using NaughtyAttributes;
using NikosAssets.Helpers;

namespace NikosAssets.Pooling.Marker
{
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
