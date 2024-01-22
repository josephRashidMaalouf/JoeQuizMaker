using CommunityToolkit.Mvvm.ComponentModel;

namespace Common.Models;

public partial class CategoryModel : ObservableObject
{
    
    [ObservableProperty]
    private string name;

    public override string ToString() => Name;
}