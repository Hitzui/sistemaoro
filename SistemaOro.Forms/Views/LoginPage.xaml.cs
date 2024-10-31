using DevExpress.Xpf.Editors;
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
using SistemaOro.Forms.ViewModels;

namespace SistemaOro.Forms.Views
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : ThemedWindow
    {
        private readonly LoginViewModel _viewModel;
        public LoginPage()
        {
            InitializeComponent();
            _viewModel = (LoginViewModel)DataContext;
        }

        private async void SimpleButton_Click(object sender, RoutedEventArgs e)
        {
            var login = await _viewModel.OnLoginCommand();
            if (!login) return;
            var main = new MainWindow();
            main.Show();
            Close();
        }

        private void ThemedWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.OnLoad();
            TxtUsername.Focus();
        }

        private void TextEdit_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    txtPassword.Focus();
                    break;
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    cmbAgencia.Focus();
                    break;
            }
        }

        private void cmbAgencia_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    btnLogin.Focus();
                    break;
            }
        }
    }
}
