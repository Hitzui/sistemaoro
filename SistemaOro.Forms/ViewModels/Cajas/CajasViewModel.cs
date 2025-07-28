using SistemaOro.Data.Entities;
using DevExpress.Mvvm.DataAnnotations;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using DevExpress.Mvvm.Xpf;
using SistemaOro.Data.Libraries;
using SistemaOro.Data.Repositories;
using SistemaOro.Forms.Services.Helpers;
using SistemaOro.Forms.Services.Mensajes;
using Unity;

namespace SistemaOro.Forms.ViewModels.Cajas;

public class CajasViewModel : BaseViewModel
{
    private ICajaRepository _cajaRepository;
    private IAgenciaRepository _agenciaRepository;
    public CajasViewModel()
    {
        var unitOfWork = VariablesGlobales.Instance.UnityContainer.Resolve<IUnitOfWork>();
        Title = "Cajas";
        _itemsSource = new List<Caja>();
        _cajaRepository = unitOfWork.CajaRepository;
        _agenciaRepository = unitOfWork.AgenciaRepository;
        _agencias = new List<Agencia>();
    }

    private IList<Agencia> _agencias;

    public IList<Agencia> Agencias
    {
        get => _agencias;
        set => SetValue(ref _agencias, value);
    }

    private IList<Caja> _itemsSource;
    public IList<Caja> ItemsSource
    {
        get=>_itemsSource;
        set => SetValue(ref _itemsSource, value);
    }
    [Command]
    public async void ValidateRow(RowValidationArgs args)
    {
        var message = HelpersMessage.MensajeConfirmacionResult(MensajesGenericos.GuardarTitulo, MensajesCajas.GuardarCaja);
        if (message== MessageBoxResult.Cancel)
        {
            return;
        }
        var item = (Caja)args.Item;
        bool save;
        if (args.IsNewItem)
        {
            save =await _cajaRepository.AddAsync(item);
        }
        else
        {
            save = await _cajaRepository.UpdateAsync(item);
        }
        if (!save)
        {
            HelpersMessage.MensajeErroResult(MensajesGenericos.ErrorTitulo, _cajaRepository.ErrorSms);
        }
        OnLoad();
    }

    [Command]
    public async void ValidateRowDeletion(ValidateRowDeletionArgs args)
    {
        var message = HelpersMessage.MensajeConfirmacionResult(MensajesGenericos.EliminarTitulo, MensajesCajas.EliminarCaja);
        if (message== MessageBoxResult.Cancel)
        {
            return;
        }
        var item = (Caja)args.Items.Single();
        var delete = await _cajaRepository.DeleteAsync(item.Codcaja);
        if (!delete)
        {
            HelpersMessage.MensajeErroResult(MensajesGenericos.ErrorTitulo, _cajaRepository.ErrorSms);
        }
        OnLoad();
    }
    [Command]
    public void DataSourceRefresh(DataSourceRefreshArgs args)
    {
        _itemsSource.Clear();
        OnLoad();
    }

    public async void OnLoad()
    {
        IsLoading=true;
        ItemsSource = await _cajaRepository.FindAll();
        Agencias = await _agenciaRepository.FindAll();
        IsLoading = false;

    }
}