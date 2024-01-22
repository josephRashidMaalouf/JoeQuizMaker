using DataAccess.Services;

namespace DataAccess;

public static class Db
{
    public static QuizRepository QuizRepo { get; private set; } = new QuizRepository();

    public static QuestionRepository QuestionRepo { get; private set; } = new QuestionRepository();

    public static CategoryRepository CategoryRepo { get; private set; } = new CategoryRepository();
}