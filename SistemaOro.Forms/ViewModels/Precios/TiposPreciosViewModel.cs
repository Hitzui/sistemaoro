using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaOro.Data.Entities;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.Xpf;
using SistemaOro.Data.Repositories;
using SistemaOro.Data.Libraries;
using SistemaOro.Forms.Models;
using SistemaOro.Forms.Repository;
using SistemaOro.Forms.Services;
using Unity;

namespace SistemaOro.Forms.ViewModels.Precios
{
    internal class TiposPreciosViewModel : BaseViewModel
    {
        public TiposPreciosViewModel()
        {
            Title = "Tipos de precios";
        }

        private readonly IDtoTipoPrecioRepository _dtoTipoPrecioRepository = VariablesGlobalesForm.Instance.DtoTipoPrecioRepository;


        public IList<DtoTiposPrecios>? ItemsSource
        {
            get => GetValue<IList<DtoTiposPrecios>>();
            set => SetValue(value);
        }

        [Command]
        public void ValidateRow(RowValidationArgs args)
        {
            var item = (DtoTiposPrecios)args.Item;

            if (string.IsNullOrWhiteSpace(item.Descripcion))
            {
                return;
            }

            _dtoTipoPrecioRepository.SaveTask(item);
        }

        [Command]
        public void ValidateRowDeletion(ValidateRowDeletionArgs args)
        {
            var item = (DtoTiposPrecios)args.Items.Single();
            _dtoTipoPrecioRepository.DeleteTask(item.IdTipoPrecio);
        }

        [Command]
        public void DataSourceRefresh(DataSourceRefreshArgs args)
        {
            Load();
        }

        public async void Load()
        {
            ItemsSource = await _dtoTipoPrecioRepository.FindAll();
            RaisePropertyChanged(nameof(ItemsSource));
        }
    }
}