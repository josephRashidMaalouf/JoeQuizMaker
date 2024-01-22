using Common.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataAccess;
using JoeQuizApp.Managers;

namespace JoeQuizApp.ViewModels;

public partial class CreateQuizViewModel : ObservableObject
{
    [ObservableProperty] 
    private QuizModel quiz;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(CreateNewQuizCommand))]
    private string entryTitle;

    [ObservableProperty]
    private string entryDescription;

    [RelayCommand(CanExecute = nameof(CreateNewQuizCanExecute))]
    private async Task CreateNewQuiz()
    {
        var newQuiz = new QuizModel()
        {
            Title = EntryTitle,
            Description = EntryDescription,
        };

        EntryTitle = string.Empty;
        EntryDescription = string.Empty;

        await Db.QuizRepo.CreateQuiz(newQuiz);

        await QuizManager.OnQuizzesChanged();

        await Shell.Current.GoToAsync("..");
        
    }

    private bool CreateNewQuizCanExecute() => !string.IsNullOrWhiteSpace(EntryTitle) && string.IsNullOrWhiteSpace(EntryDescription);
}