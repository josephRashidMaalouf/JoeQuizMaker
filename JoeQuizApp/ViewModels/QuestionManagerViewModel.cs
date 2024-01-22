using System.Collections;
using System.Collections.ObjectModel;
using Common.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataAccess;
using JoeQuizApp.Comparer;
using JoeQuizApp.Managers;

namespace JoeQuizApp.ViewModels;

public partial class QuestionManagerViewModel : ObservableObject
{
    #region QuestionProps

    private QuestionModel NewQuestion { get; set; } = new();

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(CreateQuestionCommand))]
    private string editQuestionText;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(CreateQuestionCommand))]
    private string editFirstAlternativeText;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(CreateQuestionCommand))]
    private string editSecondAlternativeText;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(CreateQuestionCommand))]
    private string editThirdAlternativeText;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(CreateQuestionCommand))]
    private bool editFirstAlternativeIsCorrect = false;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(CreateQuestionCommand))]
    private bool editSecondAlternativeIsCorrect = false;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(CreateQuestionCommand))]
    private bool editThirdAlternativeIsCorrect = false;

    [ObservableProperty]
    private ObservableCollection<QuestionModel> allQuestions = new();

    [ObservableProperty]
    private QuestionModel? selectedQuestion = new QuestionModel();

    #endregion

    #region CategoryProps
    [ObservableProperty]
    private ObservableCollection<CategoryModel> categoryBank = new();

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddCategoryToQuestionFromBankCommand))]
    private CategoryModel? selectedAddFromBankCategory;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddNewCategoriesToQuestionCommand))]
    private string categoryEntries = string.Empty;

    [ObservableProperty]
    private ObservableCollection<CategoryModel> newQuestionCategories = new();

    [ObservableProperty]
    private CategoryModel? removeThisCategory = new();

    #endregion

    public QuestionManagerViewModel()
    {
        LoadQuestions();
        LoadCategories();

        QuizManager.QuestionsChanged += UpdateQuestions;
    }

    private async void UpdateQuestions()
    {
        await LoadQuestions();
    }


    #region QuestionRelays

    [RelayCommand(CanExecute = nameof(CreateNewQuestionCanExecute))]
    private async Task CreateQuestion()
    {
        NewQuestion.QuestionText = EditQuestionText;
        NewQuestion.FirstAlternative = (EditFirstAlternativeText, EditFirstAlternativeIsCorrect);
        NewQuestion.SecondAlternative = (EditSecondAlternativeText, EditSecondAlternativeIsCorrect);
        NewQuestion.ThirdAlternative = (EditThirdAlternativeText, EditThirdAlternativeIsCorrect);
        NewQuestion.Categories.AddRange(NewQuestionCategories);

        var id = await Db.QuestionRepo.CreateQuestion(NewQuestion);

        NewQuestion.Id = id;

        AllQuestions.Add(NewQuestion);

        NewQuestion = new QuestionModel();

        EditQuestionText = string.Empty;
        EditFirstAlternativeText = string.Empty;
        EditSecondAlternativeText = string.Empty;
        EditThirdAlternativeText = string.Empty;

        EditFirstAlternativeIsCorrect = false;
        EditSecondAlternativeIsCorrect = false;
        EditThirdAlternativeIsCorrect = false;

        NewQuestionCategories = new ObservableCollection<CategoryModel>();
    }


    #endregion

    #region CategoryRelays

    [RelayCommand(CanExecute = nameof(AddCategoryFromBankCanExecute))]
    private void AddCategoryToQuestionFromBank()
    {
        NewQuestionCategories.Add(SelectedAddFromBankCategory);
        SelectedAddFromBankCategory = null;
    }

    [RelayCommand(CanExecute = nameof(AddNewCategoryCanExecute))]
    private async Task AddNewCategoriesToQuestion()
    {
        var categories = CategoryEntries.Split(',')
            .Select(c => c.Trim())
            .Select(c => new CategoryModel()
            {
                Name = c
            });


        var comparer = new CategoryEqualityComparer();

        var uniqueCategories = categories.Except(CategoryBank, comparer);

        if (uniqueCategories.Any())
        {
            await Db.CategoryRepo.CreateCategories(uniqueCategories);

            foreach (var categoryModel in uniqueCategories)
            {
                CategoryBank.Add(categoryModel);
            }
        }
        

        foreach (var categoryModel in categories)
        {
            NewQuestionCategories.Add(categoryModel);
        }

        

        CategoryEntries = string.Empty;

        await QuizManager.OnCategoriesChanged();

    }

    [RelayCommand]
    private void RemoveCategory()
    {
        NewQuestionCategories.Remove(RemoveThisCategory);
    }
    #endregion

    #region CanExecutes
    
    private bool CreateNewQuestionCanExecute()
    {
        var allFieldsFilledIn = !string.IsNullOrWhiteSpace(EditQuestionText)
                                && !string.IsNullOrWhiteSpace(EditFirstAlternativeText)
                                && !string.IsNullOrWhiteSpace(EditSecondAlternativeText)
                                && !string.IsNullOrWhiteSpace(EditThirdAlternativeText);

        var correctAnswerChecked =
            EditFirstAlternativeIsCorrect == true
            || EditSecondAlternativeIsCorrect == true
            || EditThirdAlternativeIsCorrect == true;

        return allFieldsFilledIn && correctAnswerChecked;
    }

    private bool AddNewCategoryCanExecute() => !string.IsNullOrWhiteSpace(CategoryEntries);

    private bool AddCategoryFromBankCanExecute() => SelectedAddFromBankCategory is not null;

    #endregion

    [RelayCommand]
    private async Task NavToEditQuestion()
    {
        var selectedQuestionId = SelectedQuestion.Id;

        var queryInfo = new Dictionary<string, object>
        {
            { "SelectedQuestionId", selectedQuestionId }
        };

        await Shell.Current.GoToAsync("EditQuestion", queryInfo);
    }

    #region InitMethods

    private async Task LoadQuestions()
    {
        var questions = await Db.QuestionRepo.GetAllQuestions();
        AllQuestions = new ObservableCollection<QuestionModel>(questions);
    }
    private async Task LoadCategories()
    {
        var categories = await Db.CategoryRepo.GetAllCategories();
        CategoryBank = new ObservableCollection<CategoryModel>(categories);
    }

    #endregion
}