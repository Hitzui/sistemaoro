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
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
