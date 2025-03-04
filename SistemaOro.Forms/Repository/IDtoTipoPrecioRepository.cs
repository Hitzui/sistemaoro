using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using SistemaOro.Data.Repositories;
using SistemaOro.Forms.Dto;

namespace SistemaOro.Forms.Repository;

public interface IDtoTipoPrecioRepository
{
    public string Error { get; }

    Task<IList<DtoTiposPrecios>> FindAll();

    Task<bool> SaveTask(DtoTiposPrecios dtoTiposPrecios);

    Task<bool> DeleteTask(int id);
}