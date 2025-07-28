using System.Windows;
using System.Windows.Controls;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;

namespace SistemaOro.Forms.Services.Helpers;

public class HelpersMessage
{
    public static WinUIDialogWindow DialogWindow(string title, string message, MessageBoxButton? button = null)
    {
        button ??= MessageBoxButton.OK;
        var dialog = new WinUIDialogWindow(title, button.Value)
        {
            Content = new TextBlock { Text = message},
            WindowStartupLocation = WindowStartupLocation.CenterScreen
        };
        return dialog;
    }

    /// <summary>
    /// Mensaje de confirmación con titulo y mensaje, los botones son OK y Cancel
    /// </summary>
    /// <param name="title">Titulo</param>
    /// <param name="message">Cuerpo</param>
    /// <returns>MessageBoxResult</returns>
    public static MessageBoxResult MensajeConfirmacionResult(string title, string message)
    {
        return ThemedMessageBox.Show(
            title: title,
            text: message,
            messageBoxButtons: MessageBoxButton.OKCancel,
            defaultButton: MessageBoxResult.OK,
            icon: MessageBoxImage.Question
        );
    }
    public static void MensajeInformacionResult(string title, string message)
    {
        ThemedMessageBox.Show(
            title: title,
            text: message,
            messageBoxButtons: MessageBoxButton.OK,
            defaultButton: MessageBoxResult.OK,
            icon: MessageBoxImage.Information
        );
    }
    public static void MensajeErroResult(string title, string message)
    {
        ThemedMessageBox.Show(
            title: title,
            text: message,
            messageBoxButtons: MessageBoxButton.OK,
            defaultButton: MessageBoxResult.OK,
            icon: MessageBoxImage.Error
        );
    }
}