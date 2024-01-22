using System.Collections.ObjectModel;
using Common.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataAccess;
using JoeQuizApp.Managers;
using JoeQuizApp.Pages;

namespace JoeQuizApp.ViewModels;

public partial class ChooseQuizViewModel : ObservableObject
{
    [ObservableProperty]
    private QuizModel? selectedQuiz;

    [ObservableProperty]
    private ObservableCollection<QuizModel> quizzes = new();

    public ChooseQuizViewModel()
    {
        LoadQuizzes();

        QuizManager.QuizChanged += UpdateQuizList;
    }

    private async void UpdateQuizList()
    {
        await LoadQuizzes();
    }


    [RelayCommand]
    private async Task NavToPlayQuiz()
    {
        var selectedQuizId = SelectedQuiz.Id;
        SelectedQuiz = null;
        await Shell.Current.GoToAsync($"{nameof(PlayQuizPage)}?SelectedQuizId={selectedQuizId}");
    }



    private async Task LoadQuizzes()
    {
        var quizCollection = await Db.QuizRepo.GetAllQuizzes();

        Quizzes = new ObservableCollection<QuizModel>(quizCollection);
    }
}