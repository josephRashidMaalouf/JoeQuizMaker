using JoeQuizApp.ViewModels;

namespace JoeQuizApp.Pages;

public partial class QuizListPage : ContentPage
{
	public QuizListPage()
	{
		InitializeComponent();
        BindingContext = new QuizListViewModel();
    }
}