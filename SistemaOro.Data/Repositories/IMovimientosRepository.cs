﻿using SistemaOro.Data.Dto;
using SistemaOro.Data.Entities;

namespace SistemaOro.Data.Repositories;

public interface IMovimientosRepository :ICrudRepository<Movcaja>
{
    Task<List<MovCajasDto>> GetMovcajasAndRubro();
}