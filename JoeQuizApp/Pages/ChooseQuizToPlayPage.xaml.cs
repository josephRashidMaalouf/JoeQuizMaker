using JoeQuizApp.ViewModels;

namespace JoeQuizApp.Pages;

public partial class ChooseQuizToPlayPage : ContentPage
{
	public ChooseQuizToPlayPage()
	{
		InitializeComponent();

        BindingContext = new ChooseQuizViewModel();
    }
}