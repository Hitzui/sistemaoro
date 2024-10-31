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
using SistemaOro.Forms.ViewModels.Usuarios;


namespace SistemaOro.Forms.Views.Usuarios
{
    /// <summary>
    /// Interaction logic for UsuarioEditWindow.xaml
    /// </summary>
    public partial class UsuarioEditWindow : ThemedWindow
    {
        public UsuarioEditWindow()
        {
            InitializeComponent();
            ((UsuarioEditViewModel)DataContext).CloseAction = Close;
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void ThemedWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ((UsuarioEditViewModel)DataContext).Load();
            TxtCodigo.Focus();
        }
    }
}
