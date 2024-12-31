namespace DevExpress.UITemplates.Collection.Editors {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using DevExpress.Utils;
    using DevExpress.Utils.Design;
    using DevExpress.Utils.Drawing;
    using DevExpress.Utils.Html;
    using DevExpress.XtraEditors.Drawing;
    using DevExpress.XtraEditors.Internal;

    public abstract class ButtonBase : HtmlButtonBase, DevExpress.Utils.MVVM.ISupportCommandBinding {
        readonly ButtonImageOptions iconImageOptionsCore;
        protected ButtonBase() {
            this.iconImageOptionsCore = CreateImageOptions();
            this.iconImageOptionsCore.Changed += OnIconImageOptionsChanged;
        }
        protected override void Dispose(bool disposing) {
            if(iconImageOptionsCore != null) {
                this.iconImageOptionsCore.Changed -= OnIconImageOptionsChanged;
                iconImageOptionsCore.Dispose();
            }
            base.Dispose(disposing);
        }
        public enum SizeType {
            Small = -1,
            Default = 0,
            Large = 1,
        }
        SizeType buttonSizeTypeCore = SizeType.Default;
        [DefaultValue(SizeType.Default), System.ComponentModel.Category("Layout")]
        public SizeType ButtonSize {
            get { return buttonSizeTypeCore; }
            set {
                if(buttonSizeTypeCore == value) return;
                buttonSizeTypeCore = value;
                OnButtonSizeChanged(value);
            }
        }
        protected virtual void OnButtonSizeChanged(SizeType value) {
            SetButtonElementSize(value);
            if(AutoSize) 
                AdjustSize();
            OnPropertiesChanged();
        }
        [SmartTagProperty("AutoSize", "", 3)]
        public override bool AutoSize {
            get { return base.AutoSize; }
            set { base.AutoSize = value; }
        }
        protected abstract void SetButtonElementSize(SizeType value);
        #region Icon
        [System.ComponentModel.Category(XtraEditors.CategoryName.Appearance), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [SmartTagProperty("Icon", "", 2, SmartTagActionType.RefreshBoundsAfterExecute | SmartTagActionType.RefreshContentAfterExecute)]
        public ButtonImageOptions IconImageOptions {
            get { return iconImageOptionsCore; }
        }
        protected virtual void OnIconImageOptionsChanged(object sender, ImageOptionsChangedEventArgs args) {
            shouldRaiseSizeableChanged = args.ImageSizeChanged;
            OnPropertiesChanged();
        }
        protected virtual ButtonImageOptions CreateImageOptions() {
            return new ButtonImageOptions();
        }
        public class ButtonImageOptions : ButtonImageOptionsBase { }
        #endregion
        protected readonly static object icon = new object();
        //
        protected override void FindParts(Dictionary<object, DxHtmlElement> parts, DxHtmlRootElement root) {
            parts[icon] = root.FindElementById(nameof(icon));
        }
        protected override void CalcPartsVisibility(Dictionary<object, DxHtmlElement> parts) {
            DxHtmlElement iconElement;
            if(parts.TryGetValue(icon, out iconElement) && iconElement != null)
                iconElement.Hidden = !IsIconVisible();
        }
        protected virtual bool IsIconVisible() {
            return IconImageOptions.HasImage;
        }
        protected override object GetPartImage(string partName, ObjectState state, DxHtmlElementBase element) {
            if(partName == "Icon")
                return iconImageOptionsCore.GetImage(LookAndFeel, state);
            return base.GetPartImage(partName, state, element);
        }
        protected override Rectangle CalcPartBounds(Dictionary<object, DxHtmlElement> parts, object partKey) {
            if(Equals(partKey, icon))
                return parts[icon].AbsoluteBounds;
            return Rectangle.Empty;
        }
        protected override void UpdateFromOwner(EditorButtonObjectInfoArgs buttonInfo) {
            base.UpdateFromOwner(buttonInfo);
            buttonInfo.Button.ImageOptions.ImageToTextAlignment = XtraEditors.ImageAlignToText.LeftCenter;
            buttonInfo.Button.ImageOptions.Assign(IconImageOptions);
        }
        bool isKeyPressedCore;
        protected override void OnKeyDown(KeyEventArgs e) {
            base.OnKeyDown(e);
            if(e.Handled)
                return;
            if(e.KeyData == Keys.Space) {
                isKeyPressedCore = true;
                UpdateViewInfoState();
            }
        }
        protected override void OnKeyUp(KeyEventArgs e) {
            base.OnKeyUp(e);
            if(e.Handled)
                return;
            if(e.KeyData == Keys.Space) {
                bool wasPressed = isKeyPressedCore;
                isKeyPressedCore = false;
                UpdateViewInfoState();
                if(wasPressed)
                    OnKeyClick();
            }
        }
        protected virtual void OnKeyClick() {
            OnClick(EventArgs.Empty);
        }
        protected override void OnLostFocus(EventArgs e) {
            base.OnLostFocus(e);
            isKeyPressedCore = false;
        }
        protected override bool GetValidationCanceled() {
            if(!base.GetValidationCanceled())
                return false;
            return !IsUnvalidatedControlIsParent(this);
        }
        #region Commands
        public virtual IDisposable BindCommand(object command, Func<object> queryCommandParameter = null,
            Action<ButtonBase, Func<bool>> updateState = null) {
            if(updateState == null)
                updateState = (button, canExecute) => button.Enabled = canExecute();
            return DevExpress.Utils.MVVM.CommandHelper.Bind(this,
                (button, execute) => button.Click += (s, e) => execute(),
                updateState,
                command, queryCommandParameter);
        }
        public virtual IDisposable BindCommand(System.Linq.Expressions.Expression<Action> commandSelector, object source,
            Action<ButtonBase, Func<bool>> updateState, Func<object> queryCommandParameter = null) {
            return BindCommand(commandSelector, source, queryCommandParameter, updateState);
        }
        public virtual IDisposable BindCommand(System.Linq.Expressions.Expression<Action> commandSelector, object source,
            Func<object> queryCommandParameter = null, Action<ButtonBase, Func<bool>> updateState = null) {
            if(updateState == null)
                updateState = (button, canExecute) => button.Enabled = canExecute();
            return DevExpress.Utils.MVVM.CommandHelper.Bind(this,
                (button, execute) => button.Click += (s, e) => execute(),
                updateState,
                commandSelector, source, queryCommandParameter);
        }
        public virtual IDisposable BindCommand<T>(System.Linq.Expressions.Expression<Action<T>> commandSelector, object source,
            Action<ButtonBase, Func<bool>> updateState, Func<T> queryCommandParameter = null) {
            return BindCommand(commandSelector, source, queryCommandParameter, updateState);
        }
        public virtual IDisposable BindCommand<T>(System.Linq.Expressions.Expression<Action<T>> commandSelector, object source,
            Func<T> queryCommandParameter = null, Action<ButtonBase, Func<bool>> updateState = null) {
            if(updateState == null)
                updateState = (button, canExecute) => button.Enabled = canExecute();
            return DevExpress.Utils.MVVM.CommandHelper.Bind(this,
                (button, execute) => button.Click += (s, e) => execute(),
                updateState,
                commandSelector, source, () => (queryCommandParameter != null) ? queryCommandParameter() : default(T));
        }
        IDisposable DevExpress.Utils.MVVM.ISupportCommandBinding.BindCommand(object command, Func<object> queryCommandParameter) {
            return BindCommand(command, queryCommandParameter);
        }
        IDisposable DevExpress.Utils.MVVM.ISupportCommandBinding.BindCommand(System.Linq.Expressions.Expression<Action> commandSelector, object source, Func<object> queryCommandParameter) {
            return BindCommand(commandSelector, source, queryCommandParameter);
        }
        IDisposable DevExpress.Utils.MVVM.ISupportCommandBinding.BindCommand<T>(System.Linq.Expressions.Expression<Action<T>> commandSelector, object source, Func<T> queryCommandParameter) {
            return BindCommand<T>(commandSelector, source, queryCommandParameter);
        }
        #endregion Commands
    }
}
