using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SistemaOro.Forms.ViewModels;


namespace SistemaOro.Forms.Views
{
    /// <summary>
    /// Interaction logic for IngresarUsuarioModal.xaml
    /// </summary>
    public partial class IngresarUsuarioModal : DevExpress.Xpf.Core.ThemedWindow
    {
        public IngresarUsuarioModal()
        {
            InitializeComponent();
            ((IngresarUsuarioViewModel)DataContext).CerrarDialogo = result =>
            {
                DialogResult = result;
                Close();
            };
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TxtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.Enter)
            {
                TxtPassword.Focus();
            }
        }

        private void TxtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.Enter)
            {
                BtnAceptar.Focus();
            }
        }

        private void BtnAceptar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.Enter)
            {
                BtnAceptar.Command.Execute(null);
            }
        }
    }
}
