using JoeQuizApp.ViewModels;

namespace JoeQuizApp.Pages;

public partial class EditQuestion : ContentPage
{
	public EditQuestion()
	{
		InitializeComponent();
        BindingContext = new EditQuestionViewModel();
    }
}