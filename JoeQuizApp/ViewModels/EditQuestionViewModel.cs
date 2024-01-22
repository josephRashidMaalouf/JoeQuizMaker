using Common.Models;
using DataAccess;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JoeQuizApp.Comparer;
using JoeQuizApp.Managers;

namespace JoeQuizApp.ViewModels;

[QueryProperty("SelectedQuestionId", "SelectedQuestionId")]

public partial class EditQuestionViewModel : ObservableObject
{
    private string _selectedQuestionId;
    public string SelectedQuestionId
    {
        get => _selectedQuestionId;
        set
        { 
            _selectedQuestionId = value;
            LoadQuestion(_selectedQuestionId);
        }
    }


    #region QuestionProps

    [ObservableProperty]
    private QuestionModel selectedQuestion = new();

    [ObservableProperty]
    private string questionFirstAlternativeText;
    [ObservableProperty]
    private string questionSecondAlternativeText;
    [ObservableProperty]
    private string questionThirdAlternativeText;

    [ObservableProperty]
    private string editQuestionText;

    [ObservableProperty]
    private string editAlternativeOneText;
    [ObservableProperty]
    private string editAlternativeTwoText;
    [ObservableProperty]
    private string editAlternativeThreeText;

    [ObservableProperty]
    private bool editAlternativeOneIsCorrect;
    [ObservableProperty]
    private bool editAlternativeTwoIsCorrect;
    [ObservableProperty]
    private bool editAlternativeThreeIsCorrect;

    #endregion

    #region CategoryProps

    [ObservableProperty]
    private ObservableCollection<CategoryModel> categoryBank = new();

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddCategoryToQuestionFromBankCommand))]
    private CategoryModel? selectedAddFromBankCategory = new();

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddNewCategoriesToQuestionCommand))]
    private string categoryEntries = string.Empty;

    [ObservableProperty]
    private ObservableCollection<CategoryModel> updateQuestionCategories = new();

    [ObservableProperty]
    private CategoryModel? removeThisCategory = new();

    


    #endregion

    public EditQuestionViewModel()
    {
        LoadCategoryBank();
    }


    #region QuestionRelays

    [RelayCommand]
    private async Task UpdateQuestion()
    {
        if (!string.IsNullOrEmpty(EditQuestionText))
        {
            SelectedQuestion.QuestionText = EditQuestionText;
        }
        if (!string.IsNullOrEmpty(EditAlternativeOneText))
        {
            SelectedQuestion.FirstAlternative = (EditAlternativeOneText, EditAlternativeOneIsCorrect);
            QuestionFirstAlternativeText = EditAlternativeOneText;
        }
        if (!string.IsNullOrEmpty(EditAlternativeTwoText))
        {
            SelectedQuestion.SecondAlternative = (EditAlternativeTwoText, EditAlternativeTwoIsCorrect);
            QuestionSecondAlternativeText = EditAlternativeTwoText;
        }
        if (!string.IsNullOrEmpty(EditAlternativeThreeText))
        {
            SelectedQuestion.ThirdAlternative = (EditAlternativeThreeText, EditAlternativeThreeIsCorrect);
            QuestionThirdAlternativeText = EditAlternativeThreeText;
        }
            

        SelectedQuestion.Categories = UpdateQuestionCategories.ToList();

        await Db.QuestionRepo.UpdateQuestion(SelectedQuestion);

        EditQuestionText = string.Empty;
        EditAlternativeOneText = string.Empty;
        EditAlternativeTwoText = string.Empty;
        EditAlternativeThreeText = string.Empty;

        await Shell.Current.DisplayAlert("Question updated", "Your question has been successfully updated", "Cool beans");
    }

    [RelayCommand]
    private async Task DeleteQuestion()
    {
        var confirmDelete = await Shell.Current.DisplayAlert("Delete question", "Are you sure you want to delete this question?", "Yes, I am sure", "No, I don't want to delete it");

        if (confirmDelete == false)
            return;

        await Db.QuestionRepo.DeleteQuestion(SelectedQuestionId);
        await QuizManager.OnQuestionsChanged();
        await Shell.Current.GoToAsync("..");
    }

    #endregion

    #region CategoryRelays

    [RelayCommand(CanExecute = nameof(AddCategoryFromBankCanExecute))]
    private void AddCategoryToQuestionFromBank()
    {
        UpdateQuestionCategories.Add(SelectedAddFromBankCategory);
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
            UpdateQuestionCategories.Add(categoryModel);
        }

        

        CategoryEntries = string.Empty;

        await QuizManager.OnCategoriesChanged();

    }

    [RelayCommand]
    private async Task RemoveCategory()
    {
        UpdateQuestionCategories.Remove(RemoveThisCategory);
    }
    #endregion

    #region CanExecutes

    private bool AddNewCategoryCanExecute() => !string.IsNullOrWhiteSpace(CategoryEntries);

    private bool AddCategoryFromBankCanExecute() => SelectedAddFromBankCategory is not null;

    #endregion


    #region Init

    private async Task LoadQuestion(string id)
    {
        var question = await Db.QuestionRepo.GetQuestionById(SelectedQuestionId); 
        SelectedQuestion = question;
        UpdateQuestionCategories = new ObservableCollection<CategoryModel>(SelectedQuestion.Categories);

        QuestionFirstAlternativeText = SelectedQuestion.FirstAlternative.answerText;
        QuestionSecondAlternativeText = SelectedQuestion.SecondAlternative.answerText;
        QuestionThirdAlternativeText = SelectedQuestion.ThirdAlternative.answerText;

        EditAlternativeOneIsCorrect = SelectedQuestion.FirstAlternative.isCorrect;
        EditAlternativeTwoIsCorrect = SelectedQuestion.SecondAlternative.isCorrect;
        EditAlternativeThreeIsCorrect = SelectedQuestion.ThirdAlternative.isCorrect;
    }

    private async Task LoadCategoryBank()
    {
        var categories = await Db.CategoryRepo.GetAllCategories();
        CategoryBank = new ObservableCollection<CategoryModel>(categories);
    }

    #endregion
}