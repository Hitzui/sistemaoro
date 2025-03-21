﻿using DevExpress.Mvvm;
using SistemaOro.Data.Entities;
using SistemaOro.Data.Libraries;
using SistemaOro.Forms.Services;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.Xpf;
using System;
using System.Diagnostics;
using System.Linq.Expressions;
using DevExpress.Data.Filtering;
using DevExpress.Xpf.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using SistemaOro.Data.Repositories;
using Unity;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SistemaOro.Forms.ViewModels.Clientes;

public class ClientesViewModels : BaseViewModel
{
    private readonly IClienteRepository _clienteRepository = VariablesGlobales.Instance.UnityContainer.Resolve<IClienteRepository>();


    private Cliente _selectedItem = new();

    public Cliente SelectedItem
    {
        get => _selectedItem;
        set
        {
            if (SetValue(ref _selectedItem, value))
            {
                VariablesGlobalesForm.Instance.SelectedCliente = _selectedItem;
            }
        }
    }

    Expression<Func<Cliente, bool>> MakeFilterExpression(CriteriaOperator filter)
    {
        var converter = new GridFilterCriteriaToExpressionConverter<Cliente>();
        return converter.Convert(filter);
    }

    [AsyncCommand]
    public void FetchPage(FetchPageAsyncArgs args)
    {
        const int pageTakeCount = 5;
        args.Result = Task.Run<FetchRowsResult>(() => _clienteRepository.FetchPage(args.Skip, args.Take * pageTakeCount)
            .SortBy(args.SortOrder, defaultUniqueSortPropertyName: nameof(Cliente.Codcliente))
            .Where(MakeFilterExpression((CriteriaOperator)args.Filter)).ToArray());
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
    DataContext _Context;
    IList<Cliente> _ItemsSource;
    public IList<Cliente> ItemsSource
    {
        get
        {
            if (_ItemsSource == null && !DevExpress.Mvvm.ViewModelBase.IsInDesignMode)
            {
                _Context = new DataContext();
                _ItemsSource = _Context.Clientes.AsNoTracking().ToList();
            }
            return _ItemsSource;
        }
    }
    [Command]
    public void DataSourceRefresh(DataSourceRefreshArgs args)
    {
        _ItemsSource = null;
        _Context = null;
        RaisePropertyChanged(nameof(ItemsSource));
    }
}