using Common.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using DataAccess;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;

namespace JoeQuizApp.ViewModels;

[QueryProperty("SelectedQuizId", "SelectedQuizId")]
public partial class PlayQuizViewModel : ObservableObject
{
    private string _selectedQuizId;
    public string SelectedQuizId
    {
        get => _selectedQuizId;
        set
        {
            _selectedQuizId = value;
            LoadQuiz(value);
        }
    }

    [ObservableProperty] 
    private QuizModel selectedQuiz;





    [RelayCommand]
    private void CorrectQuiz()
    {
        var score = 0;
        foreach (var question in SelectedQuiz.Questions)
        {
            if (question is { FirstAlternativeGuess: true, FirstAlternative.isCorrect: true })
            {
                score++;
            }
            else if (question is { SecondAlternativeGuess: true, SecondAlternative.isCorrect: true })
            {
                score++;
            }
            else if (question is { ThirdAlternativeGuess: true, ThirdAlternative.isCorrect: true })
            {
                score++;
            }

        }

        Shell.Current.DisplayAlert("Score",
            $"You got {score} points out of a maximum of {SelectedQuiz.Questions.Count} points", "Ok");
    }
    
    private async Task LoadQuiz(string id)
    {
        var quiz = await Db.QuizRepo.GetQuizById(id);
        SelectedQuiz = quiz;
    }
}