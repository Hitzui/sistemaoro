using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SistemaOro.Data.Entities;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.Xpf;
using SistemaOro.Data.Repositories;
using SistemaOro.Data.Libraries;
using SistemaOro.Forms.Dto;
using SistemaOro.Forms.Repository;
using SistemaOro.Forms.Services;
using SistemaOro.Forms.Services.Helpers;
using Unity;

namespace SistemaOro.Forms.ViewModels.Precios
{
    internal class TiposPreciosViewModel : BaseViewModel
    {
        private readonly IDtoTipoPrecioRepository _dtoTipoPrecioRepository = VariablesGlobalesForm.Instance.DtoTipoPrecioRepository;

        public TiposPreciosViewModel()
        {
            Title = "Tipos de precios";
        }

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
        public async void ValidateRowDeletion(ValidateRowDeletionArgs args)
        {
            var result = HelpersMessage.MensajeConfirmacionResult("Eliminar", "Se ha eliminado el tipo de precio");
            if (result==MessageBoxResult.Cancel)
            {
                return;
            }
            var item = (DtoTiposPrecios)args.Items.Single();
           var delete = await _dtoTipoPrecioRepository.DeleteTask(item.IdTipoPrecio);
           if (delete)
           {
               HelpersMessage.MensajeInformacionResult("Eliminar", "Se ha eliminado el tipo de precio");
           }
           else
           {
               HelpersMessage.MensajeErroResult("Error", $"Se produjo el siguiente error: {_dtoTipoPrecioRepository.Error}");
           }
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