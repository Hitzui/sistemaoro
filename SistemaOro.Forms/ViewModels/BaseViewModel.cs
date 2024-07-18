using System.Windows;
using DevExpress.Mvvm;
using SistemaOro.Forms.Views;

namespace SistemaOro.Forms.ViewModels;

public class BaseViewModel: ViewModelBase
{
    private string _title=string.Empty;

    public string Title
    {
        get => _title;
        set => SetValue(ref _title, value);
    }
    private bool _isLoading;
    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value, nameof(IsLoading));
    }

}