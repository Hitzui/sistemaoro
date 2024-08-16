using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using SistemaOro.Forms.ViewModels.Cajas;

namespace SistemaOro.Forms.Views.Cajas
{
    /// <summary>
    /// Interaction logic for RealizarMovimientoCajaPage.xaml
    /// </summary>
    public partial class RealizarMovimientoCajaPage : ThemedWindow
    {
        public RealizarMovimientoCajaPage()
        {
            InitializeComponent();
            ((RealizarMovimientoCajaViewModel)DataContext).CloseAction = Close;
            TxtReferencia.Focus();
        }

        private void ThemedWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ((RealizarMovimientoCajaViewModel)DataContext).Load();
        }

        private void SimpleButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TextEdit_Validate(object sender, ValidationEventArgs e)
        {
            // e.Value - the processed input value
            if (e.Value is null) return;
            var value = e.Value as string;
            if (value!.Length > 4) return;
            // Set the e.IsValid property to 'false' if the input value is invalid
            e.IsValid = false;
            // Specifies the error icon type
            e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
            // Specifies the error text
            e.ErrorContent = "Debe ingresar una referencia o concepto para continuar.";
        }
    }
}
