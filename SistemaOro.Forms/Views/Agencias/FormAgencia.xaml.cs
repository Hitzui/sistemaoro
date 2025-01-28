using System.Windows;
using DevExpress.XtraEditors.DXErrorProvider;
using SistemaOro.Data.Entities;
using SistemaOro.Forms.ViewModels.Agencias;

namespace SistemaOro.Forms.Views.Agencias
{
    /// <summary>
    /// Interaction logic for FormAgencia.xaml
    /// </summary>
    public partial class FormAgencia : Window
    {
        public FormAgencia()
        {
            InitializeComponent();
            ((FormAgenciaViewModel)DataContext).CloseAction = Close;
        }

        public Agencia? SelectedAgencia { get; init; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ((FormAgenciaViewModel)DataContext).Load(SelectedAgencia);
        }

        private void SimpleButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TextEdit_Validate(object sender, DevExpress.Xpf.Editors.ValidationEventArgs e)
        {
            if (e.Value == null) return;
            if (e.Value.ToString()!.Length > 0) return;
            e.IsValid=false;
            e.ErrorType = ErrorType.Warning;
            e.ErrorContent = "Debe especificar un nombre de agencia para continuar";
        }
    }
}
