using System;
using System.Windows;
using SistemaOro.Forms.Services.Helpers;
using SistemaOro.Forms.ViewModels.Compras;

namespace SistemaOro.Forms.Views.Compras
{
    /// <summary>
    /// Lógica de interacción para FormCapturarHuella.xaml
    /// </summary>
    public partial class FormCapturarHuella : Window
    {
        private CapturarHuellaViewModel _viewModel;
        public byte[]? ImageHuella { get; set; }
        public FormCapturarHuella()
        {
            InitializeComponent();
            _viewModel = (CapturarHuellaViewModel)DataContext;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.OnLoad();
            if (ImageHuella is not null && ImageHuella.Length > 0)
            {
                ImgHuella.Source = HelpersMethods.ByteArrayToBitmapImage(ImageHuella);
            }
        }


        private void GuardarHuella_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ImgHuella.HasImage)
                {
                    ImageHuella = (byte[])ImgHuella.EditValue;
                }
                _viewModel.CerrarDispositivo();
                _viewModel.MFpm.Dispose();
            }
            catch (Exception exception)
            {
                HelpersMessage.MensajeErroResult("Error", exception.Message);
            }
            DialogResult = true;
            Close();
        }
    }
}
