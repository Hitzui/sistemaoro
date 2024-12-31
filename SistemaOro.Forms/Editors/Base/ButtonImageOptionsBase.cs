namespace DevExpress.UITemplates.Collection.Editors {
    using System.ComponentModel;
    using DevExpress.Skins;
    using DevExpress.Utils;

    public class ButtonImageOptionsBase : ImageCollectionImageOptions, ISkinElementDataProvider {
        object imageListCore;
        [DefaultValue(null), TypeConverter(typeof(DevExpress.Utils.Design.ImageCollectionImagesConverter))]
        public virtual object ImageList {
            get { return imageListCore; }
            set { SetValue(ref imageListCore, value, "ImageList"); }
        }
        protected override object GetImageCollection() {
            return imageListCore;
        }
        [Editor(typeof(DevExpress.Utils.Design.ImageIndexesEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DevExpress.Utils.ImageList("ImageList")]
        public override int ImageIndex {
            get { return base.ImageIndex; }
            set { base.ImageIndex = value; }
        }
        protected override void AssignCore(ImageOptions source) {
            base.AssignCore(source);
            var sourceOptions = source as ButtonImageOptionsBase;
            if(sourceOptions != null)
                imageListCore = sourceOptions.ImageList;
        }
        protected override void ResetCore() {
            base.ResetCore();
            imageListCore = null;
        }
        //
        SkinProductId ISkinElementDataProvider.ProductId {
            get { return SkinProductId.Common; }
        }
        string ISkinElementDataProvider.ElementName {
            get { return CommonSkins.SkinButton; }
        }
    }
}
