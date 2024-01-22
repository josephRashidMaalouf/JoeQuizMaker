using JoeQuizApp.ViewModels;

namespace JoeQuizApp.Pages;

public partial class QuestionManager : ContentPage
{
	public QuestionManager()
	{
		InitializeComponent();

        BindingContext = new QuestionManagerViewModel();
    }
}