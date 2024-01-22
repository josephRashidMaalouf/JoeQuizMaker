using JoeQuizApp.ViewModels;

namespace JoeQuizApp.Pages;

public partial class PlayQuizPage : ContentPage
{
	public PlayQuizPage()
	{
		InitializeComponent();
        BindingContext = new PlayQuizViewModel();
    }
}