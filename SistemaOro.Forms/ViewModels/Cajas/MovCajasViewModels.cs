using System.Collections.Generic;
using System.Windows.Documents;
using SistemaOro.Data.Dto;
using SistemaOro.Data.Libraries;
using SistemaOro.Data.Repositories;
using Unity;

namespace SistemaOro.Forms.ViewModels.Cajas;

public class MovCajasViewModels :BaseViewModel
{
    private IMovimientosRepository MovimientosRepository => VariablesGlobales.Instance.UnityContainer.Resolve<IMovimientosRepository>();

    public MovCajasViewModels()
    {
        Title = "Movimientos de Caja";
        _itemSource = new List<MovCajasDto>();
    }

    private List<MovCajasDto> _itemSource;

    public List<MovCajasDto> ItemSource
    {
        get => _itemSource;
        set => SetValue(ref _itemSource, value);
    }

    public async void OnLoad()
    {
        IsLoading=true;
        ItemSource = await MovimientosRepository.GetMovcajasAndRubro();
        IsLoading=false;
    }
}