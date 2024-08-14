using System;
using System.Windows.Controls;
using DevExpress.Data.Filtering;
using DevExpress.Xpf.Data;
using DevExpress.Xpf.Grid;
using SistemaOro.Data.Entities;

namespace SistemaOro.Forms.Views.Clientes
{
    /// <summary>
    /// Interaction logic for Listado.xaml
    /// </summary>
    public partial class Listado : Page
    {
        public Listado()
        {
            InitializeComponent(); 
        }

        private void OnSearchStringToFilterCriteria(object sender, SearchStringToFilterCriteriaEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.SearchString))
            {
                var filter = e.SearchString.ToLower();
                e.Filter = CriteriaOperator.FromLambda<Cliente>(cliente => cliente.Nombres.Contains(filter)
                                                                           || cliente.Apellidos!.Contains(filter)
                                                                           || cliente.Numcedula.Contains(filter));
            }
            e.ApplyToColumnsFilter = true;
        }
    }
}
