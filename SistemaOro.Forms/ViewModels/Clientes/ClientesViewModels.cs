using DevExpress.Mvvm;
using Microsoft.EntityFrameworkCore;
using SistemaOro.Data.Entities;
using SistemaOro.Data.Libraries;
using DevExpress.Data.Linq;
using SistemaOro.Forms.Services;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.Xpf;
using System;
using System.Linq.Expressions;
using DevExpress.Data.Filtering;
using DevExpress.Xpf.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SistemaOro.Forms.ViewModels.Clientes;

public class ClientesViewModels : ViewModelBase
{
    private Cliente _selectedItem=new();

    public Cliente SelectedItem
    {
        get => _selectedItem;
        set
        {
            SetProperty(ref _selectedItem, value,nameof(SelectedItem));
            VariablesGlobalesForm.SelectedCliente = value;
        }
    }

    Expression<Func<Cliente, bool>> MakeFilterExpression(CriteriaOperator filter)
    {
        var converter = new GridFilterCriteriaToExpressionConverter<Cliente>();
        return converter.Convert(filter);
    }
    [Command]
    public void FetchPage(FetchPageAsyncArgs args)
    {
        const int pageTakeCount = 5;
        args.Result = Task.Run<FetchRowsResult>(() =>
        {
            var context = new DataContext();
            var queryable = context.Clientes.AsNoTracking()
                .SortBy(args.SortOrder, defaultUniqueSortPropertyName: nameof(Cliente.Codcliente))
                .Where(MakeFilterExpression((CriteriaOperator)args.Filter));
            return queryable.Skip(args.Skip).Take(args.Take * pageTakeCount).ToArray();
        });
    }
    [Command]
    public void GetTotalSummaries(GetSummariesAsyncArgs args)
    {
        args.Result = Task.Run(() =>
        {
            var context = new DataContext();
            var queryable = context.Clientes.Where(MakeFilterExpression((CriteriaOperator)args.Filter));
            return queryable.GetSummaries(args.Summaries);
        });
    }
}