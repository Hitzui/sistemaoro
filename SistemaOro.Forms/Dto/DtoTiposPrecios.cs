using System.ComponentModel;
using DevExpress.Mvvm;
using SistemaOro.Data.Entities;
using SistemaOro.Forms.Services;
using SistemaOro.Forms.Services.Mensajes;

namespace SistemaOro.Forms.Dto;

public class DtoTiposPrecios :  ViewModelBase, IDataErrorInfo
{
    public TipoPrecio TipoPrecio()
    {
        var mapper = MapperConfig.InitializeAutomapper();
        return mapper.Map<TipoPrecio>(this);
    }

    public DtoTiposPrecios GetDtoTiposPrecios(TipoPrecio tipoPrecio)
    {
        var mapper = MapperConfig.InitializeAutomapper();
        return mapper.Map<DtoTiposPrecios>(tipoPrecio);
    }

    public int IdTipoPrecio { get; set; }
    public string? Descripcion { get; set; }
    public decimal? Precio { get; set; }
    public string Error { get; set; } = "";

    public string this[string columnName]
    {
        get
        {
            switch (columnName)
            {
                case nameof(Descripcion):
                    if (string.IsNullOrWhiteSpace(Descripcion))
                    {
                        return MensajeTiposPrecios.DescripcionVacia;
                    }
                    break;
                case nameof(Precio):
                    if (Precio is null)
                    {
                        return MensajeTiposPrecios.PrecioVacio;
                    }

                    if (decimal.Compare(Precio.Value, decimal.Zero)<=0)
                    {
                        return MensajeTiposPrecios.PrecioMayorZero;
                    }
                    break;
            }

            return "";
        }
    }
    
}