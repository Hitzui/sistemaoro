using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using SistemaOro.Data.Repositories;
using SistemaOro.Forms.Models;

namespace SistemaOro.Forms.Repository;

public interface IDtoTipoPrecioRepository 
{
    Task<IList<DtoTiposPrecios>> FindAll();

    Task<bool> SaveTask(DtoTiposPrecios dtoTiposPrecios);

    Task<bool> DeleteTask(int id);
}