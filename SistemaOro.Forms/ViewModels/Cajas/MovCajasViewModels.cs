using System.Collections.Generic;
using System.Windows.Documents;
using DevExpress.Mvvm.Native;
using SistemaOro.Data.Dto;
using SistemaOro.Data.Entities;
using SistemaOro.Data.Libraries;
using SistemaOro.Data.Repositories;
using SistemaOro.Forms.Services;
using Unity;

namespace SistemaOro.Forms.ViewModels.Cajas;

public class MovCajasViewModels :BaseViewModel
{
    private readonly IMovimientosRepository _movimientosRepository;

    public MovCajasViewModels()
    {
        var unitOfWork = VariablesGlobales.Instance.UnityContainer.Resolve<IUnitOfWork>();
        _movimientosRepository = unitOfWork.MovimientosRepository;
        Title = "Movimientos de Caja";
        _itemSource = new DXObservableCollection<MovCajasDto>();
    }

    public MovCajasDto SelectedItem
    {
        get => GetValue<MovCajasDto>();
        set
        {
            SetValue(value);
            VariablesGlobalesForm.Instance.MovCajasDtoSelected = value;
        }
    }

    private DXObservableCollection<MovCajasDto> _itemSource;

    public DXObservableCollection<MovCajasDto> ItemSource
    {
        get => _itemSource;
        set => SetValue(ref _itemSource, value);
    }

    public async void OnLoad()
    {
        IsLoading=true;
        var findAll = await _movimientosRepository.GetMovcajasAndRubro();
        VariablesGlobalesForm.Instance.MovimientosCajaCollection = new DXObservableCollection<MovCajasDto>(findAll);
        ItemSource = VariablesGlobalesForm.Instance.MovimientosCajaCollection;
        IsLoading=false;
    }
}