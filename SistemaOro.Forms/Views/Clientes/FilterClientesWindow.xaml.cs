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
using DevExpress.Xpf.Core;
using SistemaOro.Forms.ViewModels.Clientes;


namespace SistemaOro.Forms.Views.Clientes
{
    /// <summary>
    /// Interaction logic for FilterClientesWindow.xaml
    /// </summary>
    public partial class FilterClientesWindow : ThemedWindow
    {
        public FilterClientesWindow()
        {
            InitializeComponent();
            if (DataContext is FilterClientesViewModel viewModel)
            {
                viewModel.CloseAction = () =>
                {
                    DialogResult = true;
                    Close();
                };
            }
        }

        private void ThemedWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ((FilterClientesViewModel)DataContext).Load();
        }
    }
}
