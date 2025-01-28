using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaOro.Data.Libraries;
using SistemaOro.Data.Repositories;
using SistemaOro.Forms.Models;
using Unity;

namespace SistemaOro.Forms.Repository;

public class DtoTipoPrecioRepository : IDtoTipoPrecioRepository
{
    private readonly ITipoPrecioRepository _tipoPrecioRepotistory = VariablesGlobales.Instance.UnityContainer.Resolve<ITipoPrecioRepository>();

    public string Error { get; private set; }

    public async Task<IList<DtoTiposPrecios>> FindAll()
    {
        var findAll = await _tipoPrecioRepotistory.FindAll();
        var templist = new List<DtoTiposPrecios>();
        foreach (var item in findAll)
        {
            var dtoTipo = new DtoTiposPrecios();
            templist.Add(dtoTipo.GetDtoTiposPrecios(item));
        }

        return templist;
    }

    public async Task<bool> SaveTask(DtoTiposPrecios dtoTiposPrecios)
    {
        var tipoPrecio = dtoTiposPrecios.TipoPrecio();
        bool save;
        if (dtoTiposPrecios.IdTipoPrecio == 0)
        {
            save = await _tipoPrecioRepotistory.AddAsync(tipoPrecio);
        }
        else
        {
            var findTipoPrecio = await _tipoPrecioRepotistory.GetByIdAsync(tipoPrecio.Idtipoprecio);
            if (findTipoPrecio is null)
            {
                dtoTiposPrecios.Error = "No fue posible recuperar la entidad";
                return false;
            }

            findTipoPrecio.Precio = tipoPrecio.Precio;
            findTipoPrecio.Descripcion = tipoPrecio.Descripcion;
            save = await _tipoPrecioRepotistory.UpdateAsync(findTipoPrecio);
        }

        dtoTiposPrecios.Error = _tipoPrecioRepotistory.ErrorSms;
        return save;
    }

    public async Task<bool> DeleteTask(int id)
    {
        var tipoPrecio = await _tipoPrecioRepotistory.GetByIdAsync(id);
        if (tipoPrecio is null)
        {
            Error = $"No fue posible eliminar el tipo de precio con ID: {id}";
            return false;
        }

        var delete = await _tipoPrecioRepotistory.DeleteAsync(tipoPrecio);
        Error = _tipoPrecioRepotistory.ErrorSms;
        return delete;
    }
}