﻿#pragma checksum "..\..\..\..\..\..\Views\Cajas\RealizarMovimientoCajaPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "C36F08DA92A7C9A85C8CFE2AE7D1791B7F84D5C1"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using DevExpress.Core;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.ConditionalFormatting;
using DevExpress.Xpf.Core.DataSources;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Xpf.Core.ServerMode;
using DevExpress.Xpf.DXBinding;
using DevExpress.Xpf.Data;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.DataPager;
using DevExpress.Xpf.Editors.DateNavigator;
using DevExpress.Xpf.Editors.ExpressionEditor;
using DevExpress.Xpf.Editors.Filtering;
using DevExpress.Xpf.Editors.Flyout;
using DevExpress.Xpf.Editors.Popups;
using DevExpress.Xpf.Editors.Popups.Calendar;
using DevExpress.Xpf.Editors.RangeControl;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Editors.Settings.Extension;
using DevExpress.Xpf.Editors.Validation;
using SistemaOro.Forms.ViewModels.Cajas;
using SistemaOro.Forms.Views.Cajas;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace SistemaOro.Forms.Views.Cajas {
    
    
    /// <summary>
    /// RealizarMovimientoCajaPage
    /// </summary>
    public partial class RealizarMovimientoCajaPage : DevExpress.Xpf.Core.ThemedWindow, System.Windows.Markup.IComponentConnector {
        
        
        #line 18 "..\..\..\..\..\..\Views\Cajas\RealizarMovimientoCajaPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Editors.TextEdit TxtReferencia;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/SistemaOro.Forms;component/views/cajas/realizarmovimientocajapage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\..\Views\Cajas\RealizarMovimientoCajaPage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 12 "..\..\..\..\..\..\Views\Cajas\RealizarMovimientoCajaPage.xaml"
            ((SistemaOro.Forms.Views.Cajas.RealizarMovimientoCajaPage)(target)).Loaded += new System.Windows.RoutedEventHandler(this.ThemedWindow_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.TxtReferencia = ((DevExpress.Xpf.Editors.TextEdit)(target));
            
            #line 18 "..\..\..\..\..\..\Views\Cajas\RealizarMovimientoCajaPage.xaml"
            this.TxtReferencia.Validate += new DevExpress.Xpf.Editors.Validation.ValidateEventHandler(this.TextEdit_Validate);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 33 "..\..\..\..\..\..\Views\Cajas\RealizarMovimientoCajaPage.xaml"
            ((DevExpress.Xpf.Core.SimpleButton)(target)).Click += new System.Windows.RoutedEventHandler(this.SimpleButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

