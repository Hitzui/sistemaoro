using System;
using System.Collections.Generic;
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

namespace SistemaOro.Forms.ViewModels.Compras;

public class ComprasViewModel : BaseViewModel
{
    private readonly ICompraRepository _compraRepository;
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();

    public ComprasViewModel()
    {
        var unitOfWork = VariablesGlobales.Instance.UnityContainer.Resolve<IUnitOfWork>();
        _compraRepository = unitOfWork.CompraRepository;
        Title = "Compras";
        FetchPageCommand  = new DelegateCommand<FetchPageAsyncArgs>(FetchPage);
        GetTotalSummariesCommand = new DelegateCommand<GetSummariesAsyncArgs>(GetTotalSummaries);
        PagedAsyncSource = new PagedAsyncSource
        {
            ElementType = typeof(ComprasCliente),
            FetchPageCommand = FetchPageCommand,
            GetTotalSummariesCommand = GetTotalSummariesCommand,
            PageNavigationMode = PageNavigationMode.ArbitraryWithTotalPageCount
        };
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
        VariablesGlobalesForm.Instance.MainViewModel.formComprasPage = frmCompra;
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

    private ICommand<FetchPageAsyncArgs> FetchPageCommand { get; }

    private ICommand<GetSummariesAsyncArgs> GetTotalSummariesCommand { get; }

    private void FetchPage(FetchPageAsyncArgs args)
    {
        const int pageTakeCount = 10;
        args.Result = Task.Run<FetchRowsResult>(() =>
        {
            var context = new DataContext();
            var queryable = context.ComprasClientes.AsNoTracking()
                .SortBy(args.SortOrder, defaultUniqueSortPropertyName: nameof(ComprasCliente.Numcompra))
                .Where(cliente => cliente.Codestado == EstadoCompra.Vigente || cliente.Codestado == EstadoCompra.Cerrada)
                .Where(MakeFullFilterExpression((CriteriaOperator)args.Filter, SearchText))
                .OrderByDescending(c=> c.Fecha);
            return queryable.Skip(args.Skip).Take(args.Take * pageTakeCount).ToArray();
        });
    }

    private void GetTotalSummaries(GetSummariesAsyncArgs args)
    {
        args.Result = Task.Run(() =>
        {
            var context = new DataContext();
            var queryable = context.ComprasClientes.Where(MakeFullFilterExpression((CriteriaOperator)args.Filter, SearchText))
                .Where(cliente => cliente.Codestado == EstadoCompra.Vigente 
                                  || cliente.Codestado == EstadoCompra.Cerrada);
            return queryable.GetSummaries(args.Summaries);
        });
    }

    [Command]
    public void SearchCompra()
    {
        PagedAsyncSource.RefreshRows();
    }
}