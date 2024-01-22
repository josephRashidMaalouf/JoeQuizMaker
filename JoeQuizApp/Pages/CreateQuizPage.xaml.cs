using JoeQuizApp.ViewModels;

namespace JoeQuizApp.Pages;

public partial class CreateQuizPage : ContentPage
{
	public CreateQuizPage()
	{
		InitializeComponent();
        BindingContext = new CreateQuizViewModel();
    }
}