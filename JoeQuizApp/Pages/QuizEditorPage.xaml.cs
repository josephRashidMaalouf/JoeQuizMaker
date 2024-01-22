
using JoeQuizApp.ViewModels;

namespace JoeQuizApp.Pages;

public partial class QuizEditorPage : ContentPage
{
	public QuizEditorPage()
	{
		InitializeComponent();
        BindingContext = new QuizEditorViewModel();

    }
}