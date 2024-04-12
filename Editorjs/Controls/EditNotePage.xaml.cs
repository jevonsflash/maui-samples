using Editorjs.ViewModels;
using Microsoft.Maui.Handlers;
using System.Collections.Specialized;

namespace Editorjs.Controls;

public partial class EditNotePage : ContentView 
{
    private EditNotePageViewModel editNotePageViewModel => this.BindingContext as EditNotePageViewModel;

    public EditNotePage()
    {
        EditNotePageViewModel editNotePageViewModel=new EditNotePageViewModel();
        InitializeComponent();
        this.BindingContext = editNotePageViewModel;


        rootComponent.Parameters =
            new Dictionary<string, object>
            {
                { "EditNotePageViewModel", editNotePageViewModel }
            };
    }


   

    private void OnFavoriteSwipeItemInvoked(object sender, EventArgs e)
    {

    }

    private void OnDeleteSwipeItemInvoked(object sender, EventArgs e)
    {

    }

    private void DragGestureRecognizer_DragStarting_Collection(object sender, DragStartingEventArgs e)
    {

    }

    private void Button_Clicked(object sender, EventArgs e)
    {

    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {

    }
}