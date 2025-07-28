using System;
using System.IO;
using System.Windows;
using NLog;

namespace SistemaOro.Forms.ViewModels.Compras
{
    /// <summary>
    /// Lógica de interacción para FormCapturarFirma.xaml
    /// </summary>
    public partial class FormCapturarFirma : Window
    {
        public string? SigString { get; set; }
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public FormCapturarFirma()
        {
            InitializeComponent();
        }
        
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SigPlusNet.SetTabletState(0);
        }

        

        private void cmdClear_Click(object? sender, EventArgs e)
        {
            SigPlusNet.ClearTablet();
        }

        private void FuncionFirmar_Click(object? sender, EventArgs e)
        {
            SigPlusNet.SetTabletState(1);
        }

        private void cmdSigString_Click(object sender, RoutedEventArgs e)
        {
            if (SigPlusNet.NumberOfTabletPoints() > 0)
            {
                SigPlusNet.SetTabletState(0);

                // Encrypt the signature.
                SigPlusNet.AutoKeyStart();
                SigPlusNet.SetAutoKeyANSIData("123");
                SigPlusNet.AutoKeyFinish();
                SigPlusNet.SetEncryptionMode(2);
                
                SigPlusNet.SetSigCompressionMode(1);
                SigString = SigPlusNet.GetSigString(); // Convertir a string
                Logger.Info(SigString);
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Please sign first.");
            }
        }
    }
}
