using System.Collections;
using System.Collections.Generic;
using SistemaOro.Data.Dto;
using SistemaOro.Data.Libraries;
using SistemaOro.Data.Repositories;
using Unity;

namespace SistemaOro.Forms.ViewModels;

public class ComprasViewModel : BaseViewModel
{
    private readonly ICompraRepository _compraRepository;
    public ComprasViewModel()
    {
        _compraRepository = VariablesGlobales.Instance.UnityContainer.Resolve<ICompraRepository>();
        Title = "Compras";
    }

    public IList<DtoComprasClientes> ComprasClientesList
    {
        get=>GetValue<List<DtoComprasClientes>>(); 
        set=>SetValue(value);
    }

    public async void Load()
    {
        ComprasClientesList = await _compraRepository.FindComprasClientes();
    }
}