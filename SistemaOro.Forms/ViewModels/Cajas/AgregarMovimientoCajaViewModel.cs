using DevExpress.Mvvm;
using SistemaOro.Data.Entities;
using SistemaOro.Data.Libraries;
using SistemaOro.Data.Repositories;
using SistemaOro.Forms.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Input;
using Unity;

namespace SistemaOro.Forms.ViewModels.Cajas;

public class AgregarMovimientoCajaViewModel : BaseViewModel
{
    private IRubroRepository _rubroRepository;
    private IMovimientosRepository _movimientosRepository;
    private bool isNew;
    public AgregarMovimientoCajaViewModel()
    {
        Title = "Agregar Movimiento Caja";
        _rubroRepository = VariablesGlobales.Instance.UnityContainer.Resolve<IRubroRepository>();
        _movimientosRepository = VariablesGlobales.Instance.UnityContainer.Resolve<IMovimientosRepository>();
        SaveCommand = new DelegateCommand(OnSaveCommand);
        Movcaja = new Movcaja();
    }

    private async void OnSaveCommand()
    {
        if (string.IsNullOrWhiteSpace(Movcaja.Descripcion))
        {
            return;
        }

        if (SelectedRubro is null)
        {
            return;
        }


        Movcaja.Codrubro = SelectedRubro.Codrubro;
        var returnTask = isNew ? _movimientosRepository.AddAsync(Movcaja).WaitAsync(CancellationToken.None) : _movimientosRepository.UpdateAsync(Movcaja).WaitAsync(CancellationToken.None);
        var task = await returnTask;
        if (task)
        {
            CloseAction?.Invoke();
        }
        else
        {
            Debug.WriteLine(_movimientosRepository.ErrorSms);
        }

    }

    public ICommand SaveCommand { get; }

    public Rubro? SelectedRubro
    {
        get => GetValue<Rubro>();
        set => SetValue(value);
    }

    public Movcaja Movcaja
    {
        get => GetValue<Movcaja>();
        set => SetValue(value);
    }

    public List<Rubro> Rubros
    {
        get => GetValue<List<Rubro>>();
        set => SetValue(value);
    }

    public async void Load()
    {
        Rubros = await _rubroRepository.FindAll();
        isNew = true;
        if (VariablesGlobalesForm.Instance.MovCajasDtoSelected is null) return;
        isNew = false;
        var findMovcaja = await _movimientosRepository.GetByIdAsync(VariablesGlobalesForm.Instance.MovCajasDtoSelected.IdMov);
        if (findMovcaja != null)
        {
            Movcaja = findMovcaja;
        }
        SelectedRubro = await _rubroRepository.FindById(VariablesGlobalesForm.Instance.MovCajasDtoSelected.Codrubro);
    }
}