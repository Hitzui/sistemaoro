using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Navigation;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.XtraPrinting.Drawing;
using NLog;
using SistemaOro.Data.Dto;
using SistemaOro.Data.Libraries;
using SistemaOro.Data.Repositories;
using SistemaOro.Forms.Services.Helpers;
using SistemaOro.Forms.Views.Compras;
using SistemaOro.Forms.Views.Reportes.Compras;
using Unity;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using DevExpress.XtraEditors;
using SistemaOro.Data.Entities;
using SistemaOro.Forms.Services;
using DevExpress.Mvvm.Xpf;
using System.Linq.Expressions;
using DevExpress.Data.Filtering;
using DevExpress.Xpf.Data;
using System.Linq;
using System.Windows.Input;
using DevExpress.Mvvm;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using SistemaOro.Forms.Services.Enum;

namespace SistemaOro.Forms.ViewModels.Compras;

public class ComprasViewModel : BaseViewModel
{
    private readonly ICompraRepository _compraRepository;
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();

    private IList<ComprasCliente> _source;
    public InfiniteAsyncSource Source { get; set; }
    
    public ComprasViewModel()
    {
        var unitOfWork = VariablesGlobales.Instance.UnityContainer.Resolve<IUnitOfWork>();
        _compraRepository = unitOfWork.CompraRepository;
        Title = "Compras";
    }

    public PagedAsyncSource PagedAsyncSource { get;}
    private string _searchText;

    public string SearchText
    {
        get=>_searchText; 
        set=>SetValue(ref _searchText, value);
    }

    public ComprasCliente? SelectedCompra
    {
        get => GetValue<ComprasCliente>();
        set => SetValue(value);
    }

    public void EditarCompraCommand(NavigationService? navigationService)
    {
        if (SelectedCompra is null)
        {
            HelpersMessage.MensajeInformacionResult("Editar", "No ha seleccionado una compra a editar, intente nuevamente");
            return;
        }

        if (navigationService is null)
        {
            return;
        }

        VariablesGlobalesForm.Instance.MainViewModel.RbnEditarCompraVisible = true;
        var frmCompra = new FormComprasPage();
        VariablesGlobalesForm.Instance.MainViewModel.FormComprasPage = frmCompra;
        frmCompra.SetSelectedCompra(SelectedCompra);
        navigationService.Navigate(frmCompra);
    }

    public async void ImprimirCompra()
    {
        try
        {
            if (SelectedCompra is null)
            {
                HelpersMessage.MensajeErroResult("Imprimir", "No hay una compra seleccionada a imprimir");
                return;
            }

            var findCompra = await _compraRepository.DetalleCompraImprimir(SelectedCompra.Numcompra!);
            _logger.Info($"Numero de compra {SelectedCompra.Numcompra} - Cantidad de datos en la lista: {findCompra.Count}");
            if (findCompra.Count>0)
            {
                foreach (var detalleCompra in findCompra)
                {
                    _logger.Info($"Datos: {detalleCompra.Numcompra} - {detalleCompra.Nocontrato}");
                }
            }

            var compra = await _compraRepository.FindById(SelectedCompra.Numcompra ?? string.Empty);
            HelpersMethods.ImprimirReportesCompra(compra!, findCompra);
        }
        catch (Exception e)
        {
            await XtraMessageBox.ShowAsync(e.Message);
            _logger.Error(e, "Error al imprimir la compra");
        }
    }

    public void Load()
    {
        
    }

    public void ImprimirTicketCompra()
    {
        if (SelectedCompra is null)
        {
            HelpersMessage.MensajeErroResult("Imprimir", "No hay una compra seleccionada a imprimir");
            return;
        }
        var reporteCompraTicket = new ReporteCompraTicket();
        reporteCompraTicket.Parameters["parNumeroCompra"].Value = SelectedCompra.Numcompra;
        HelpersMethods.LoadReport(reporteCompraTicket);
    }

    Expression<Func<ComprasCliente, bool>> MakeFullFilterExpression(CriteriaOperator filter, string textoBusqueda)
    {
        var converter = new GridFilterCriteriaToExpressionConverter<ComprasCliente>();
        var baseFilter = converter.Convert(filter);

        Expression<Func<ComprasCliente, bool>> textoFilter = x => true;

        if (!string.IsNullOrWhiteSpace(textoBusqueda))
        {
            var texto = textoBusqueda.ToLower();
            textoFilter = x =>
                x.Numcompra.ToLower().Contains(texto) ||
                x.NombreCliente.ToLower().Contains(texto);
        }

        // Combina las dos expresiones usando Expression.AndAlso
        var parameter = Expression.Parameter(typeof(ComprasCliente), "x");
        var baseBody = baseFilter.Body;
        var textoBody = Expression.Invoke(textoFilter, parameter);
        var combinedBody = Expression.AndAlso(baseBody, textoBody);

        return Expression.Lambda<Func<ComprasCliente, bool>>(combinedBody, parameter);
    }

    Expression<Func<ComprasCliente, bool>> MakeFilterExpression(CriteriaOperator filter)
    {
        var converter = new GridFilterCriteriaToExpressionConverter<ComprasCliente>();
        return converter.Convert(filter);
    }
    
    [Command]
    public void FetchRowsBak(FetchRowsAsyncArgs args)
    {
        args.Result = Task.Run<FetchRowsResult>(() =>
        {
            var context = new DataContext();
            var queryable = context.ComprasClientes.AsNoTracking()
                .SortBy(args.SortOrder, defaultUniqueSortPropertyName: nameof(ComprasCliente.Numcompra))
                .OrderByDescending(c=> c.Fecha)
                .Where(c=>c.Codestado == EstadoCompra.Vigente || c.Codestado == EstadoCompra.Cerrada)
                .Where(MakeFilterExpression((CriteriaOperator)args.Filter));
            return queryable.Skip(args.Skip).Take(args.Take ?? 100).ToArray();
        });
    }
    
    [Command]
    public void GetTotalSummaries1(GetSummariesAsyncArgs args)
    {
        args.Result = Task.Run(() =>
        {
            var context = new DataContext();
            var queryable = context.ComprasClientes.Where(MakeFilterExpression((CriteriaOperator)args.Filter));
            return queryable.GetSummaries(args.Summaries);
        });
    }

    [Command]
    public void FetchRows(FetchRowsAsyncArgs args)
    {
        args.Result = Task.Run<FetchRowsResult>(() =>
        {
            using var context = new DataContext();
            
            // Construir la consulta base
            var query = context.ComprasClientes.AsNoTracking()
                .Where(c => c.Codestado == EstadoCompra.Vigente || 
                           c.Codestado == EstadoCompra.Cerrada);

            // Aplicar filtro si existe
            if (args.Filter != null)
            {
                var filterExpression = CriteriaToExpressionConverter.Convert((CriteriaOperator)args.Filter);
                query = query.Where(filterExpression);
            }

            // Aplicar ordenamiento
            query = ApplySorting(query, args.SortOrder);

            // Obtener el total de registros (para paginación)
            var totalCount = query.Count();

            // Aplicar paginación
            var items = query
                .Skip(args.Skip)
                .Take(args.Take ?? 100)
                .ToList();

            return new FetchRowsResult(items.ToArray(), hasMoreRows: items.Count >= (args.Take ?? 100));
        });
    }

    private IQueryable<ComprasCliente> ApplySorting(IQueryable<ComprasCliente> query, SortDefinition[] sortOrder)
    {
        if (sortOrder.Length == 0)
        {
            // Ordenamiento por defecto
            return query.OrderByDescending(x => x.Fecha)
                       .ThenByDescending(x => x.Numcompra);
        }

        IOrderedQueryable<ComprasCliente>? orderedQuery = null;
        var isFirstOrder = true;

        foreach (var sort in sortOrder)
        {
            if (isFirstOrder)
            {
                orderedQuery = sort.Direction == ListSortDirection.Ascending
                    ? query.OrderBy(GetSortExpression(sort.PropertyName))
                    : query.OrderByDescending(GetSortExpression(sort.PropertyName));
                isFirstOrder = false;
            }
            else
            {
                orderedQuery = sort.Direction == ListSortDirection.Ascending
                    ? orderedQuery!.ThenBy(GetSortExpression(sort.PropertyName))
                    : orderedQuery!.ThenByDescending(GetSortExpression(sort.PropertyName));
            }
        }

        return orderedQuery ?? query;
    }

    private Expression<Func<ComprasCliente, object>> GetSortExpression(string propertyName)
    {
        return propertyName switch
        {
            "Numcompra" => x => x.Numcompra,
            "Codcliente" => x => x.Codcliente,
            "Fecha" => x => x.Fecha,
            "Total" => x => x.Total,
            _ => x => x.Numcompra // Por defecto
        };
    }
}