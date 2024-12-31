namespace DevExpress.UITemplates.Collection.Editors {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using DevExpress.UITemplates.Collection.Components;
    using DevExpress.Utils;
    using DevExpress.Utils.Html;

    [DefaultEvent(nameof(CheckedChanged))]
    public abstract class CheckButtonBase : ButtonBase {
        protected override void Dispose(bool disposing) {
            if(buttonElement != null)
                buttonElement.RemoveEventListener("checked-changed", OnCheckedChanged);
            base.Dispose(disposing);
        }
        protected override void SetButtonElementSize(SizeType value) {
            if(buttonElement != null)
                buttonElement.SetAttribute(nameof(CheckItemBase.ItemSize), (CheckItemBase.Size)(int)value);
        }
        #region Properties
        bool? checkedQueuedValue, designModeValue;
        [DefaultValue(false), System.ComponentModel.Category("Data")]
        [Utils.Design.SmartTagProperty("Checked", "", 1, Utils.Design.SmartTagActionType.RefreshBoundsAfterExecute | Utils.Design.SmartTagActionType.RefreshContentAfterExecute)]
        public bool Checked {
            get {
                if(DesignMode)
                    return designModeValue.GetValueOrDefault();
                if(buttonElement == null)
                    return checkedQueuedValue.GetValueOrDefault();
                var checkItem = buttonElement.GetAttribute("self") as CheckItemBase;
                return (checkItem != null) && checkItem.IsChecked;
            }
            set {
                if(DesignMode)
                    designModeValue = value;
                if(Checked == value)
                    return;
                if(buttonElement != null)
                    buttonElement.SetAttribute(nameof(CheckItemBase.IsChecked), value);
                else checkedQueuedValue = value;
                OnCheckedChanged();
            }
        }
        protected virtual void OnCheckedChanged() {
            RaiseCheckedChanged();
        }
        protected override void OnKeyClick() {
            Toggle();
            base.OnKeyClick();
        }
        public void Toggle() {
            if(GetValidationCanceled())
                return;
            Checked = !Checked;
        }
        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e) {
            var dxArgs = DXMouseEventArgs.GetMouseArgs(e);
            dxArgs.Handled = GetValidationCanceled();
            base.OnMouseUp(dxArgs);
        }
        #endregion
        #region Parts
        protected readonly static object button = new object();
        DxHtmlElement buttonElement;
        protected override void FindParts(Dictionary<object, DxHtmlElement> parts, DxHtmlRootElement root) {
            base.FindParts(parts, root);
            buttonElement = root.FindElementById(nameof(button));
            buttonElement.SetAttribute(nameof(CheckItemBase.Template), GetButtonContentTemplateId());
            if(checkedQueuedValue.GetValueOrDefault()) {
                buttonElement.SetAttribute(nameof(CheckItemBase.IsChecked), true);
                checkedQueuedValue = null;
            }
            var iSize = (CheckItemBase.Size)(int)ButtonSize;
            if(iSize != CheckItemBase.Size.Default)
                buttonElement.SetAttribute(nameof(CheckItemBase.ItemSize), iSize);
            buttonElement.AddEventListener("checked-changed", OnCheckedChanged);
            parts[icon] = buttonElement.FindElementById(nameof(icon));
            parts[button] = buttonElement;
        }
        protected override bool OnGotFocus(Dictionary<object, DxHtmlElement> parts, DxHtmlRootElement root) {
            if(buttonElement != null)
                buttonElement.Focus(true);
            return (buttonElement != null);
        }
        protected override bool OnLostFocus(Dictionary<object, DxHtmlElement> parts, DxHtmlRootElement root) {
            if(buttonElement != null)
                buttonElement.Focus(false);
            return (buttonElement != null);
        }
        #endregion Parts
        #region Events
        readonly static object checkedChanged = new object();
        //
        public event EventHandler CheckedChanged {
            add { Events.AddHandler(checkedChanged, value); }
            remove { Events.RemoveHandler(checkedChanged, value); }
        }
        protected void RaiseCheckedChanged() {
            var handler = Events[checkedChanged] as EventHandler;
            handler?.Invoke(this, EventArgs.Empty);
        }
        #endregion Events
        #region Theme
        protected abstract string GetButtonContentTemplateId();
        #endregion Theme
        #region API
        protected void EnsureButtonContentTemplate() {
            if(buttonElement != null) {
                var currentTemplateId = GetButtonContentTemplateId();
                buttonElement.SetAttribute(nameof(CheckItemBase.Template), currentTemplateId);
            }
        }
        #endregion
    }
}
