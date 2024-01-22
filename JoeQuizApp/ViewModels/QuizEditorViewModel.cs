using System.Collections.ObjectModel;
using Common.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataAccess;
using JoeQuizApp.Managers;
using JoeQuizApp.Pages;

namespace JoeQuizApp.ViewModels;

[QueryProperty("SelectedQuizId", "SelectedQuizId")]
public partial class QuizEditorViewModel : ObservableObject
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

    #region QuizProps

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(UpdateQuizCommand))]
    private string editTitle;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(UpdateQuizCommand))]
    private string editDescription;


    [ObservableProperty]
    private QuizModel selectedQuiz = new();

    #endregion

    #region QuestionProps
    [ObservableProperty]
    private string searchQuery;

    [ObservableProperty]
    private ObservableCollection<QuestionModel> allQuestions = new();

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddFromQuestionBankCommand))]
    private QuestionModel? selectedAddQuestion = new QuestionModel();

    [ObservableProperty]
    private ObservableCollection<QuestionModel> quizQuestions = new();

    [ObservableProperty]
    private QuestionModel? removeThisQuestion;

    #endregion

    #region CategoryProps

    [ObservableProperty]
    private ObservableCollection<CategoryModel> categories = new();

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(FilterQuestionsCommand))]
    private CategoryModel? selectedCategory;

    #endregion


    public QuizEditorViewModel()
    { 
        LoadQuestions();
        LoadCategories();

        QuizManager.QuestionsChanged += QuizManager_QuestionsChanged;
        QuizManager.QuestionFound += QuizManager_QuestionFound;
        QuizManager.CategoresChanged += QuizManager_CategoresChanged;
    }

    private async void QuizManager_CategoresChanged()
    {
       await LoadCategories();
    }

    private void QuizManager_QuestionFound(QuestionModel obj)
    {
        QuizQuestions.Add(obj);
    }

    private async void QuizManager_QuestionsChanged()
    {
        await LoadQuestions();
    }

    #region QuizRelays

    [RelayCommand]
    private async Task DeleteQuiz()
    {
        var confirmDelete = await Shell.Current.DisplayAlert("Delete quiz", "Are you sure you want to delete this quiz?", "Yes, I am sure", "No, I don't want to delete it");

        if (confirmDelete == false)
            return;

        await Db.QuizRepo.DeleteQuiz(SelectedQuiz);
        await Shell.Current.GoToAsync("..");

        await QuizManager.OnQuizzesChanged();

    }
    [RelayCommand]
    private async Task UpdateQuiz()
    {
        if (!string.IsNullOrWhiteSpace(EditTitle))
            SelectedQuiz.Title = EditTitle;
        
        if (!string.IsNullOrEmpty(EditDescription))
            SelectedQuiz.Description = EditDescription;

        await Db.QuizRepo.UpdateQuiz(SelectedQuiz);

        EditTitle = string.Empty;
        EditDescription = string.Empty;

        await QuizManager.OnQuizzesChanged();

        await Shell.Current.DisplayAlert("Changes saved", "Your quiz has been successfully updated", "Cool beans");

    }

    #endregion

    #region QuestionRelays

    [RelayCommand(CanExecute = nameof(AddFromQuestionBankCanExecute))]
    private void AddFromQuestionBank()
    {
        SelectedQuiz.Questions.Add(SelectedAddQuestion);
        QuizQuestions.Add(SelectedAddQuestion);
    }

    [RelayCommand]
    private void RemoveQuestionFromQuiz()
    {
        SelectedQuiz.Questions.Remove(RemoveThisQuestion);
        QuizQuestions.Remove(RemoveThisQuestion);
    }


    #endregion

    #region SearchAndFilterRelays

    [RelayCommand]
    private async void SearchForQuestion()
    {
        var questions = await Db.QuestionRepo.GetAllQuestions();

        var result = questions
            .Where(question => question.QuestionText.ToLower().Contains(SearchQuery.ToLower()))
            .ToList<QuestionModel>();
        AllQuestions = new ObservableCollection<QuestionModel>(result);
    }

    [RelayCommand(CanExecute = nameof(FilterQuestionsCanExecute))]
    private async void FilterQuestions()
    {
        var questions = await Db.QuestionRepo.GetAllQuestions();

        var result = questions
            .Where(q => q.Categories
                .Any(c => c.Name == SelectedCategory!.Name));
        AllQuestions = new ObservableCollection<QuestionModel>(result);
    }
    [RelayCommand]
    private async void ShowAllQuestions()
    {
        await LoadQuestions();
    }

    #endregion

    [RelayCommand]
    private async Task NavToQuestionManager()
    {
        await Shell.Current.GoToAsync("QuestionManager");
    }

    #region CanExecutes

    private bool FilterQuestionsCanExecute() => SelectedCategory is not null;
    private bool AddFromQuestionBankCanExecute() => SelectedAddQuestion is not null;

    #endregion

    #region InitMethods

    private async Task LoadQuiz(string id)
    {
        var quiz = await Db.QuizRepo.GetQuizById(id);
        SelectedQuiz = quiz;
        QuizQuestions = new ObservableCollection<QuestionModel>(SelectedQuiz.Questions);
    }

    private async Task LoadQuestions()
    {
        var questions = await Db.QuestionRepo.GetAllQuestions();
        AllQuestions = new ObservableCollection<QuestionModel>(questions);
    }

    private async Task LoadCategories()
    {
        var categoriesFromDb = await Db.CategoryRepo.GetAllCategories();
        Categories = new ObservableCollection<CategoryModel>(categoriesFromDb);
    }

    #endregion
}