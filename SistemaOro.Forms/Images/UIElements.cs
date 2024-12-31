namespace DevExpress.UITemplates.Collection.Images {
    using System;
    using DevExpress.Utils;
    using DevExpress.Utils.Svg;

    public static class UIElements {
        const string assetsRoot = "SistemaOro.Forms.Assets."; 
        [ThreadStatic]
        static SvgImageCollection svgImagesCore;
        public static SvgImageCollection SvgImages {
            get { return svgImagesCore ?? (svgImagesCore = SvgImageCollection.FromResources(assetsRoot + "SVG.UIElements", typeof(UIElements).Assembly)); }
        }
        #region Elements
        public static SvgImage Check {
            get { return SvgImages[nameof(Check)]; }
        }
        public static SvgImage Close {
            get { return SvgImages[nameof(Close)]; }
        }
        public static SvgImage Search {
            get { return SvgImages[nameof(Search)]; }
        }
        public static SvgImage DropDown {
            get { return SvgImages[nameof(DropDown)]; }
        }
        public static SvgImage Show {
            get { return SvgImages[nameof(Show)]; }
        }
        public static SvgImage Hide {
            get { return SvgImages[nameof(Hide)]; }
        }
        public static SvgImage Copy {
            get { return SvgImages[nameof(Copy)]; }
        }
        public static SvgImage Info {
            get { return SvgImages[nameof(Info)]; }
        }
        #endregion Elements
    }
}
