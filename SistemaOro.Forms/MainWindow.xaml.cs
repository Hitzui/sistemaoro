using DevExpress.Xpf.Core;
using SistemaOro.Forms.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SistemaOro.Forms
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ThemedWindow
    {
        private MainViewModel viewModel;
        public MainWindow()
        {
            viewModel = new MainViewModel();
            DataContext = viewModel;
            InitializeComponent();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            viewModel.SetMainFrame(MainFrame);
        }

        private void ThemedWindow_Closed(object sender, EventArgs e)
        {
            // Cerrar todas las ventanas abiertas primero
            foreach (Window window in Application.Current.Windows)
            {
                if (window != this)
                {
                    window.Close();
                }
            }
    
            // Forzar el shutdown
            Application.Current.Shutdown();
    
            // Forzar terminación del proceso si es necesario
            Environment.Exit(0);
        }
    }
}
