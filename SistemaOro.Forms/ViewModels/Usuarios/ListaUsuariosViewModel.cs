using DevExpress.Mvvm;
using SistemaOro.Data.Entities;
using DevExpress.Mvvm.DataAnnotations;
using System.Linq;
using System.Collections.Generic;
using DevExpress.Mvvm.Xpf;
using Microsoft.EntityFrameworkCore;
using SistemaOro.Forms.Services;

namespace SistemaOro.Forms.ViewModels.Usuarios
{
    public class ListaUsuariosViewModel : BaseViewModel
    {
        public ListaUsuariosViewModel()
        {
            Title = "Usuarios En Sistema";
        }

        private Usuario? _selectedUsuario;

        public Usuario? SelectedUsuario
        {
            get=>_selectedUsuario;
            set
            {
                SetValue(ref _selectedUsuario, value);
                if (_selectedUsuario is not null)
                {
                    VariablesGlobalesForm.Instance.SelectedUsuario = _selectedUsuario;
                }
            }
        }

        DataContext? _context;
        private IList<Usuario> _itemsSource = new List<Usuario>();
        public IList<Usuario> ItemsSource
        {
            get
            {
                if (_itemsSource.Count<=0 && !IsInDesignMode)
                {
                    _context = new DataContext();
                    _itemsSource = _context.Usuarios.AsNoTracking().ToList();
                }
                return _itemsSource;
            }
        }
        [Command]
        public void DataSourceRefresh(DataSourceRefreshArgs args)
        {
            _itemsSource.Clear();
            _context = null;
            RaisePropertyChanged(nameof(ItemsSource));
        }
    }
}
