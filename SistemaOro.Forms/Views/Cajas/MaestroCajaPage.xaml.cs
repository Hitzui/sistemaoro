﻿using System;
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
using SistemaOro.Forms.ViewModels.Cajas;

namespace SistemaOro.Forms.Views.Cajas
{
    /// <summary>
    /// Interaction logic for MaestroCajaPage.xaml
    /// </summary>
    public partial class MaestroCajaPage : Page
    {
        public MaestroCajaPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ((MaestroCajaViewModel)DataContext).Load();
        }
    }
}
