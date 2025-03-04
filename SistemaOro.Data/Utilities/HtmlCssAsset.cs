using DevExpress.Utils.Html;
using DevExpress.XtraEditors;

namespace DevExpress.UITemplates.Collection.Utilities
{
    using System;
    using System.Collections.Concurrent;
    using System.IO;

    public abstract class HtmlCssAsset {
        public static string AssetsRoot = "SistemaOro.Data.Assets."; 
        public static System.Reflection.Assembly ResourcesAssembly = typeof(HtmlCssAsset).Assembly; 
        //
        readonly string htmlName, cssName;
        protected HtmlCssAsset(string htmlName = null, string cssName = null) {
            if(string.IsNullOrEmpty(htmlName)) {
                var typeName = GetType().Name;
                this.htmlName = typeName.Substring(0, typeName.Length - nameof(HtmlCssAsset).Length);
            }
            else this.htmlName = htmlName;
            if(string.IsNullOrEmpty(cssName)) {
                var typeName = GetType().Name;
                this.cssName = typeName.Substring(0, typeName.Length - nameof(HtmlCssAsset).Length);
            }
            else this.cssName = cssName;
        }
        string htmlCore;
        public string Html {
            get { return htmlCore ?? (htmlCore = ReadText(htmlName, nameof(Html))); }
        }
        string cssCore;
        public string Css {
            get { return cssCore ?? (cssCore = ReadText(cssName, nameof(Css))); }
        }
        #region ReadText
        readonly static ConcurrentDictionary<string, string> texts =
            new ConcurrentDictionary<string, string>(StringComparer.Ordinal);
        static string ReadText(string name, string type) {
            string resourceName = AssetsRoot + $@"{type}.{name}.{type}";
            return texts.GetOrAdd(resourceName, x => {
                using(var stream = GetResourceStream(x, ResourcesAssembly)) {
                    return stream != null ? new StreamReader(stream).ReadToEnd() : string.Empty;
                }
            });
        }
        readonly static ConcurrentDictionary<string, string> mappings =
            new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        static Stream GetResourceStream(string key, System.Reflection.Assembly resourcesAssembly) {
            string resourceName;
            if(!mappings.TryGetValue(key, out resourceName)) {
                var resourceNames = resourcesAssembly.GetManifestResourceNames();
                TryAddResourceNameMappings(resourceNames, key, out resourceName);
            }
            return !string.IsNullOrEmpty(resourceName) ? resourcesAssembly.GetManifestResourceStream(resourceName) : null;
        }
        static void TryAddResourceNameMappings(string[] names, string key, out string resourceName) {
            resourceName = string.Empty;
            for(int i = 0; i < names.Length; i++) {
                if(names[i].EndsWith("." + nameof(Html), StringComparison.OrdinalIgnoreCase) ||
                    names[i].EndsWith("." + nameof(Css), StringComparison.OrdinalIgnoreCase)) {
                    if(mappings.TryAdd(names[i], names[i]) && StringComparer.OrdinalIgnoreCase.Compare(key, names[i]) == 0)
                        resourceName = names[i];
                }
            }
        }
        #endregion ReadText
        #region Apply
        public void AddTemplate(HtmlTemplateCollection templateCollection) {
            templateCollection.AddTemplate(Html, Css);
        }
        public void Apply(HtmlTemplate template) {
            template.Set(Html, Css);
        }
        public void Apply(HtmlContentControl control) {
            control.HtmlTemplate.Set(Html, Css);
        }
        public void Apply(HtmlContentPopup popup) {
            popup.HtmlTemplate.Set(Html, Css);
        }
        
        #endregion Apply
    }
}
