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
            Content = new TextBlock { Text = message}
        };
        return dialog;
    }

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
    public static MessageBoxResult MensajeInformacionResult(string title, string message)
    {
        return ThemedMessageBox.Show(
            title: title,
            text: message,
            messageBoxButtons: MessageBoxButton.OK,
            defaultButton: MessageBoxResult.OK,
            icon: MessageBoxImage.Information
        );
    }
    public static MessageBoxResult MensajeErroResult(string title, string message)
    {
        return ThemedMessageBox.Show(
            title: title,
            text: message,
            messageBoxButtons: MessageBoxButton.OK,
            defaultButton: MessageBoxResult.OK,
            icon: MessageBoxImage.Error
        );
    }
}