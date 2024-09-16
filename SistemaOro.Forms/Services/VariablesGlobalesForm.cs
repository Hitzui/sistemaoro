using System.IO;
using System.Windows.Controls;
using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using Microsoft.Extensions.Configuration;
using SistemaOro.Data.Dto;
using SistemaOro.Data.Entities;
using SistemaOro.Forms.Repository;

namespace SistemaOro.Forms.Services
{
    public class VariablesGlobalesForm : ViewModelBase
    {
        private static string? _appSettingsPath;
        private static VariablesGlobalesForm? _instance;

        // Objeto para asegurar la sincronización en caso de acceso multi-hilo.
        private static readonly object Padlock = new object();

        public static VariablesGlobalesForm Instance
        {
            get
            {
                // Se utiliza el bloque de sincronización para asegurar que
                // solo un hilo a la vez pueda acceder a esta sección de código.
                lock (Padlock)
                {
                    return _instance ??= new VariablesGlobalesForm();
                }
            }
        }

        // Constructor privado para evitar la creación de instancias adicionales.
        private VariablesGlobalesForm()
        {
            _agenciasCollection = new DXObservableCollection<Agencia>();
            _movimientosCajasCollection = new DXObservableCollection<MovCajasDto>();
        }

        public IConfigurationSection VariablesGlobales => ConfigurationBuilder.GetSection("globals");

        private static IConfigurationRoot ConfigurationBuilder
        {
            get
            {
                //var configurationBuilder = new ConfigurationBuilder();
                _appSettingsPath = "appsettings.json";
                var configurationBuilder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(_appSettingsPath, optional: false, reloadOnChange: true);

                return configurationBuilder.Build();
            }
        }

        public IDtoTipoPrecioRepository DtoTipoPrecioRepository => new DtoTipoPrecioRepository();
        private Cliente? _selectedCliente;

        public Cliente? SelectedCliente
        {
            get => _selectedCliente;
            set => SetValue(ref _selectedCliente, value);
        }

        private Usuario? _usuario;

        public Usuario? Usuario
        {
            get => _usuario;
            set => SetValue(ref _usuario, value);
        }

        private Agencia? _agencia;

        /// <summary>
        /// Esta es la agencia que se selecciona para editar sus datos
        /// </summary>
        public Agencia? SelectedAgencia
        {
            get => _agencia;
            set => SetValue(ref _agencia, value);
        }

        private Agencia? _agenciaInicial;

        /// <summary>
        /// Esta es la agencia que se selecciona al iniciar sesion
        /// </summary>
        public Agencia? Agencia
        {
            get => _agenciaInicial;
            set => SetValue(ref _agenciaInicial, value);
        }

        public MovCajasDto? MovCajasDtoSelected
        {
            get => GetValue<MovCajasDto>();
            set => SetValue(value);
        }

        private Mcaja? _maestroCaja;
        public Mcaja? MaestroCaja
        {
            get => _maestroCaja;
            set => SetValue(ref _maestroCaja,value);
        }

        private DXObservableCollection<Agencia> _agenciasCollection;

        public  DXObservableCollection<Agencia> AgenciasCollection
        {
            get => _agenciasCollection;
            set => SetValue(ref _agenciasCollection, value);
        }

        private DXObservableCollection<MovCajasDto> _movimientosCajasCollection;

        public DXObservableCollection<MovCajasDto> MovimientosCajaCollection
        {
            get => _movimientosCajasCollection;
            set => SetValue(ref _movimientosCajasCollection, value);
        }

        public Id? Parametros
        {
            get => GetValue<Id>();
            set => SetValue(value);
        }

        public Frame? MainFrame
        {
            get => GetValue<Frame>();
            set => SetValue(value);
        }
    }
}