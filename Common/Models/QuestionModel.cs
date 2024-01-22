using System.Security.AccessControl;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Common.Models;

public partial class QuestionModel : ObservableObject
{
    [ObservableProperty]
    private string id;

    [ObservableProperty]
    private string questionText;

    [ObservableProperty]
    private (string answerText, bool isCorrect) firstAlternative;

    [ObservableProperty]
    private (string answerText, bool isCorrect) secondAlternative;

    [ObservableProperty]
    private (string answerText, bool isCorrect) thirdAlternative;

    [ObservableProperty]
    private List<CategoryModel> categories = new();

    public override string ToString() => QuestionText;

    #region PlayQuizProps

    public string FirstAlternativeText
    {
        get => FirstAlternative.answerText;
    }
    public string SecondAlternativeText
    {
        get => SecondAlternative.answerText;
    }
    public string ThirdAlternativeText
    {
        get => ThirdAlternative.answerText;
    }

    [ObservableProperty]
    private bool firstAlternativeGuess;

    [ObservableProperty]
    private bool secondAlternativeGuess;

    [ObservableProperty]
    private bool thirdAlternativeGuess;


    #endregion

}