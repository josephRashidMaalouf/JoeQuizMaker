using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Common.Models;

public partial class QuizModel : ObservableObject
{
    [ObservableProperty] 
    private string id;

    [ObservableProperty] 
    private string title;

    [ObservableProperty] 
    private string description;

    [ObservableProperty] 
    private List<QuestionModel> questions;

}