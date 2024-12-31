namespace DevExpress.UITemplates.Collection.Components {
    using DevExpress.Utils;
    using DevExpress.Utils.Html;
    using DevExpress.Utils.Html.Internal;

    public sealed class CheckItemBase : DxHtmlComponent {
        public static void Register() {
            DxHtmlDocument.Define<CheckItemBase>("check-item");
        }
        //
        public enum Size {
            Small = -1,
            Default = 0,
            Large = 1,
        }
        public string Template {
            get { return GetAttribute(nameof(Template)) as string ?? string.Empty; }
        }
        public Size? ItemSize {
            get {
                object value = GetAttribute(nameof(ItemSize));
                if(value is int)
                    return new Size?((Size)(int)value);
                if(value is string) {
                    if(string.Equals((string)value, nameof(Size.Small), System.StringComparison.OrdinalIgnoreCase))
                        return Size.Small;
                    if(string.Equals((string)value, nameof(Size.Default), System.StringComparison.OrdinalIgnoreCase))
                        return Size.Default;
                    if(string.Equals((string)value, nameof(Size.Large), System.StringComparison.OrdinalIgnoreCase))
                        return Size.Large;
                }
                return value is Size ? new Size?((Size)value) : null;
            }
        }
        public override void AttributeChangedCallback(string attributeName) {
            base.AttributeChangedCallback(attributeName);
            if(string.Equals(attributeName, nameof(Template), System.StringComparison.OrdinalIgnoreCase)) {
                Reset();
                return;
            }
            if(string.Equals(attributeName, nameof(ItemSize), System.StringComparison.OrdinalIgnoreCase)) {
                EnsureItemSize(originalClassName);
                return;
            }
            if(string.Equals(attributeName, nameof(IsChecked), System.StringComparison.OrdinalIgnoreCase)) {
                if(lockCheckedChanged == 0)
                    EnsureCheckedState();
                return;
            }
        }
        ElementInternals _internals;
        string originalClassName;
        public override void ConnectedCallback() {
            base.ConnectedCallback();
            _internals = AttachInternals();
            AddEventListener("mouseup", OnItemMouseUp);
            SetAttribute("self", this);
            EnsureTemplate();
            EnsureCheckedState();
            EnsureItemSize(originalClassName = Element.ClassName);
        }
        public override void DisconnectedCallback() {
            RemoveEventListener("mouseup", OnItemMouseUp);
            DetachInternals();
            _internals = null;
            base.DisconnectedCallback();
        }
        public void Reset() {
            if(_internals == null)
                return;
            ClearChildren();
            EnsureTemplate();
            EnsureCheckedState();
            EnsureItemSize(originalClassName);
        }
        void EnsureTemplate() {
            string templateId = GetAttribute(nameof(Template)) as string;
            if(!string.IsNullOrEmpty(templateId)) {
                var template = Element.RootElement.FindTemplate(templateId);
                if(template != null)
                    AppendChild(template.CloneNode(true));
            }
        }
        void EnsureCheckedState() {
            bool value = GetCheckedState();
            if(isCheckedCore != value)
                UpdateStates(isCheckedCore = value);
        }
        bool GetCheckedState() {
            object isCheckedState = GetAttribute(nameof(IsChecked));
            if(isCheckedState is bool)
                return (bool)isCheckedState;
            if(isCheckedState is string)
                return string.Equals((string)isCheckedState, "true", System.StringComparison.OrdinalIgnoreCase);
            return isCheckedCore;
        }
        void EnsureItemSize(string className) {
            Element.ClassName = CalcClassName(className, ItemSize);
        }
        static string CalcClassName(string className, Size? size) {
            if(string.IsNullOrEmpty(className))
                className = "check-item";
            return className + " " + size.GetValueOrDefault().ToString().ToLowerInvariant();
        }
        bool isCheckedCore;
        public bool IsChecked {
            get { return isCheckedCore; }
        }
        int lockCheckedChanged;
        void OnItemMouseUp(DxHtmlElementEventArgs args) {
            var mouseArgs = args as DxHtmlElementMouseEventArgs;
            if(mouseArgs != null && mouseArgs.IsClick) {
                var meArgs = mouseArgs.MouseArgs as DXMouseEventArgs;
                if(meArgs != null && meArgs.Handled)
                    return;
                OnItemCheck(args);
            }
        }
        void OnItemCheck(DxHtmlElementEventArgs args) {
            lockCheckedChanged++;
            UpdateStates(isCheckedCore = !isCheckedCore);
            SetAttribute(nameof(IsChecked), IsChecked);
            DispatchEvent("checked-changed", args);
            lockCheckedChanged--;
            args.CancelBubble = true;
            args.SuppressOwnerEvent = true;
        }
        void UpdateStates(bool isChecked) {
            if(_internals == null)
                return;
            if(isChecked)
                _internals.States.Add("--checked");
            else
                _internals.States.Delete("--checked");
        }
    }
}
