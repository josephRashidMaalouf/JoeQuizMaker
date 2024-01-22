using JoeQuizApp.Pages;

namespace JoeQuizApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(CreateQuizPage), typeof(CreateQuizPage));
            Routing.RegisterRoute(nameof(QuizEditorPage), typeof(QuizEditorPage));
            Routing.RegisterRoute(nameof(EditQuestion), typeof(EditQuestion));
            Routing.RegisterRoute(nameof(QuestionManager), typeof(QuestionManager));
            Routing.RegisterRoute(nameof(PlayQuizPage), typeof(PlayQuizPage));
        }
    }
}
