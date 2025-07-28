

using System.Threading.Tasks;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using SistemaOro.Data.Entities;
using SistemaOro.Data.Libraries;
using SistemaOro.Data.Repositories;
using Unity;

namespace SistemaOro.Forms.ViewModels.Parametros;

public class ParametrosViewModel : BaseViewModel
{
    private readonly IParametersRepository _parametersRepository;
    public ParametrosViewModel()
    {
        Title = "Parametros Sistema";
        var unitOfWork = VariablesGlobales.Instance.UnityContainer.Resolve<IUnitOfWork>();
        _parametersRepository = unitOfWork.ParametersRepository;
        SaveCommandTask =  new AsyncCommand(SaveCommand);
    }

    public IAsyncCommand SaveCommandTask { get; }
    private Id? _parametros;

    public Id? Parametros
    {
        get=>_parametros;
        set => SetValue(ref _parametros, value);
    }

    private Task SaveCommand()
    {
        if (Parametros is null)
        {
            return Task.CompletedTask;
        }
        return _parametersRepository.ActualizarParametros(Parametros);
    }
    public async void Load()
    {
        Parametros = await _parametersRepository.RecuperarParametros();
    }
}