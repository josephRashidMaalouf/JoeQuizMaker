using Common.Models;

namespace JoeQuizApp.Managers;

public static class QuizManager
{
    public static event Action QuizChanged;

    public static event Action QuestionsChanged;

    public static event Action CategoresChanged;

    public static event Action<QuestionModel> QuestionFound;

    public static async Task OnQuestionFound(QuestionModel question)
    {
        QuestionFound?.Invoke(question);
    }

    public static async Task OnQuizzesChanged()
    {
        QuizChanged?.Invoke();
    }

    public static async Task OnQuestionsChanged()
    {
        QuestionsChanged?.Invoke();
    }

    public static async Task OnCategoriesChanged()
    {
        CategoresChanged?.Invoke();
    }
}