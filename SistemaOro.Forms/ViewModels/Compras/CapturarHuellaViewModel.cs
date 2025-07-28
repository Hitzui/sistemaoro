using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.Native;
using DevExpress.XtraEditors;
using DevExpress.XtraSpreadsheet.UI;
using NLog;
using SecuGen.FDxSDKPro.Windows;
using SistemaOro.Forms.Services.Helpers;
using Color = System.Windows.Media.Color;

namespace SistemaOro.Forms.ViewModels.Compras;

public class CapturarHuellaViewModel : BaseViewModel
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    public readonly SGFingerPrintManager MFpm;
    public byte[] FpImage;
    private int _mImageWidth;
    private int _mImageHeight;
    private SGFPMDeviceList? _dispositivoPrincipal;
    private SGFPMDeviceList[]? _mDevList;

    public CapturarHuellaViewModel()
    {
        MFpm = new SGFingerPrintManager();
        _listaDispositivos = new();
    }

    private ImageSource? _imageSource;

    public ImageSource? ImageSource
    {
        get => _imageSource;
        set => SetValue(ref _imageSource, value);
    }

    private int _selectedDispositio;

    public int SelectedDispositivo
    {
        get => _selectedDispositio;
        set => SetValue(ref _selectedDispositio, value);
    }

    private DXObservableCollection<string> _listaDispositivos;

    public DXObservableCollection<string> ListaDispositivos
    {
        get => _listaDispositivos;
        set => SetValue(ref _listaDispositivos, value);
    }

    private string? _statusBar;

    public string? StatusBar
    {
        get => _statusBar;
        set => SetValue(ref _statusBar, value);
    }

    private void LoadDevices()
    {
        try
        {
            Logger.Info($"Cargando dispositivos... {MFpm}");
            var iError = MFpm.EnumerateDevice();
            Logger.Info($"Números de dispositivos: {iError} - {MFpm.NumberOfDevice}");
            ListaDispositivos = new();
            _mDevList = new SGFPMDeviceList[MFpm.NumberOfDevice];
            for (var i = 0; i < MFpm.NumberOfDevice; i++)
            {
                var sgfpmDeviceList = new SGFPMDeviceList();
                MFpm.GetEnumDeviceInfo(i, sgfpmDeviceList);
                var enumDevice = sgfpmDeviceList.DevName + " : " + sgfpmDeviceList.DevID;
                ListaDispositivos.Add(enumDevice);
                _mDevList[i] = sgfpmDeviceList;
                Logger.Info($"Dispositivo encontrado: {enumDevice}");
                if (i == 0)
                {
                    _dispositivoPrincipal = sgfpmDeviceList;
                }
            }

            if (ListaDispositivos.Count > 0)
            {
                SelectedDispositivo = 0;
            }
        }
        catch (Exception e)
        {
            Logger.Error(e, "Error al cargar los dispositivos");
        }
    }

    [Command]
    public void CerrarDispositivo()
    {
        MFpm.CloseDevice();
    }

    [Command]
    public void AbrirDispositivo()
    {
        try
        {
            if (MFpm.NumberOfDevice == 0)
                return;

            SGFPMDeviceName deviceName;
            int deviceId;
            if (_dispositivoPrincipal is not null)
            {
                deviceName = _dispositivoPrincipal.DevName;
                deviceId = _dispositivoPrincipal.DevID;
            }
            else
            {
                deviceName = _mDevList[SelectedDispositivo].DevName;
                deviceId = _mDevList[SelectedDispositivo].DevID;
            }

            var iError = OpenDevice(deviceName, deviceId);

            if (iError == (Int32)SGFPMError.ERROR_NONE)
            {
                StatusBar = "Dispositivo abierto correctamente";
            }
        }
        catch (Exception e)
        {
            Logger.Error(e, "Error al abrir el dispositivo");
        }
    }

    [Command]
    public void CapturarHuella()
    {
        if (MFpm is null)
        {
            return;
        }

        try
        {
            int elapTime;

            Cursor.Current = Cursors.WaitCursor;

            var pInfo = new SGFPMDeviceInfoParam();
            var iError = MFpm.GetDeviceInfo(pInfo);

            if (iError == (int)SGFPMError.ERROR_NONE)
            {
                _mImageWidth = pInfo.ImageWidth;
                _mImageHeight = pInfo.ImageHeight;
            }

            elapTime = Environment.TickCount;
            FpImage = new byte[_mImageWidth * _mImageHeight];

            iError = MFpm.GetImage(FpImage);

            if (iError == (int)SGFPMError.ERROR_NONE)
            {
                ImageSource = null;
                elapTime = Environment.TickCount - elapTime;
                ImageSource= HelpersMethods.ConvertBitmapToImageSource(FpImage, _mImageWidth, _mImageHeight);
                StatusBar = "Capture Time : " + elapTime + " ms";
            }
            else
            {
                HelpersMessage.MensajeErroResult("Error", "No fue posible capturar la imagen");
            }


            Cursor.Current = Cursors.Default;
        }
        catch (Exception e)
        {
            Logger.Error(e, "Error al capturar la huella");
            HelpersMessage.MensajeErroResult("Error", $"Error: {e.Message}");
        }
    }

    private int OpenDevice(SGFPMDeviceName deviceName, int deviceId)
    {
        var iError = MFpm.Init(deviceName);
        iError = MFpm.OpenDevice(deviceId);

        if (iError == (int)SGFPMError.ERROR_NONE)
        {
            SGFPMDeviceInfoParam pInfo = new SGFPMDeviceInfoParam();
            iError = MFpm.GetDeviceInfo(pInfo);
            _mImageWidth = pInfo.ImageWidth;
            _mImageHeight = pInfo.ImageHeight;
            StatusBar = "Initialization Success";
        }
        else
            HelpersMessage.MensajeErroResult("Error", "Error al abrir el dispositivo");

        return iError;
    }


    public void OnLoad()
    {
        LoadDevices();
    }
}