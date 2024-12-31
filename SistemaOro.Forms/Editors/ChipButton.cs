namespace DevExpress.UITemplates.Collection.Editors {
    using System.ComponentModel;
    using System.Drawing;
    using SistemaOro.Forms.Assets.Toolbox; 
    using DevExpress.UITemplates.Collection.Components;
    using DevExpress.UITemplates.Collection.Utilities;
    using DevExpress.Utils;
    using DevExpress.Utils.Drawing;
    using DevExpress.Utils.Html;

    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(ToolboxBitmapRoot), "ChipButton.bmp")]
    [Description("Chips are compact but highly functional visual elements that allow users to make selections, filter content, or trigger actions.")]
    public class ChipButton : CheckButtonBase {
        #region Properties
        bool showCheckCore = true;
        [DefaultValue(true)]
        [System.ComponentModel.Category("Appearance")]
        [Utils.Design.SmartTagProperty("ShowCheck", "", 2, Utils.Design.SmartTagActionType.RefreshBoundsAfterExecute | Utils.Design.SmartTagActionType.RefreshContentAfterExecute)]
        public bool ShowCheck {
            get { return showCheckCore; }
            set {
                if(showCheckCore == value) return;
                showCheckCore = value;
                OnShowCheckChanged();
            }
        }
        protected virtual void OnShowCheckChanged() {
            OnPropertiesChanged();
            EnsureButtonContentTemplate();
            if(Checked && AutoSize) AdjustSize();
        }
        protected override void OnCheckedChanged() {
            base.OnCheckedChanged();
            if(ShowCheck && AutoSize) AdjustSize();
        }
        protected override void OnIconImageOptionsChanged(object sender, ImageOptionsChangedEventArgs args) {
            base.OnIconImageOptionsChanged(sender, args);
            UpdatePartVisibility(icon);
        }
        #endregion
        #region Parts
        protected override object GetPartImage(string partName, ObjectState state, DxHtmlElementBase element) {
            if(partName == "Check")
                return DevExpress.UITemplates.Collection.Images.UIElements.Check;
            return base.GetPartImage(partName, state, element);
        }
        #endregion Parts
        #region Theme
        protected override string GetButtonContentTemplateId() {
            return ShowCheck ? "chip-button-check-template" : "chip-button-template";
        }
        protected override string LoadDefaultTemplate() {
            return ChipButtonHtmlCssAsset.Default.Html;
        }
        protected override string LoadDefaultStyles() {
            return ChipButtonHtmlCssAsset.Default.Css;
        }
        sealed class ChipButtonHtmlCssAsset : HtmlCssAsset {
            static ChipButtonHtmlCssAsset() {
                CheckItemBase.Register();
            }
            public static readonly HtmlCssAsset Default = new ChipButtonHtmlCssAsset();
        }
        #endregion Theme
        protected override ICustomDxHtmlPreview CreateHtmlEditorPreview() {
            var previewControl = new ChipButton();
            previewControl.Text = string.IsNullOrEmpty(Text) ? "{Text}" : Text;
            previewControl.IconImageOptions.Assign(IconImageOptions);
            return previewControl;
        }
    }
}
