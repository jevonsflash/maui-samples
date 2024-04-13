
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using MauiSample.Common.Helper;
using Microsoft.Maui.Storage;
using System.Reflection;


namespace Editorjs.ViewModels
{
    public enum NoteSegmentState
    {
        Edit,
        PreView
    }
    public class EditNotePageViewModel : ObservableObject, IEditorViewModel
    {
        public Func<Task<string>> OnSubmitting { get; set; }
        public Action<string> OnInited { get; set; }
        public Action OnFocus { get; set; }

        public EditNotePageViewModel()
        {
            Submit = new Command(SubmitAction);

            NoteSegmentState=NoteSegmentState.Edit;
            var content = "";
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Editorjs.Assets.sample1.json"))
            {
                if (stream != null)
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        content = reader.ReadToEnd();                     
                    }
                }
            }
            Init(new Note()
            {
                Title = "sample",
                Content=content

            });
        }

        private void Init(Note note)
        {
            if (note != null)
            {
                Title = note.Title;
                Content = note.Content;
            }
            OnInited?.Invoke(this.Content);
        }


        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

      
        private string _content;

        public string Content
        {
            get { return _content; }
            set
            {
                _content = value;
                OnPropertyChanged();
            }
        }


        private NoteSegmentState _noteSegmentState;

        public NoteSegmentState NoteSegmentState
        {
            get { return _noteSegmentState; }
            set
            {
                _noteSegmentState = value;
                OnPropertyChanged();

            }
        }

        private async void SubmitAction(object obj)
        {
            var savedContent = await OnSubmitting?.Invoke();
            if (string.IsNullOrEmpty(savedContent))
            {
                return;
            }
            this.Content=savedContent;

            var note = new Note();
            note.Title = this.Title;
            note.Content = this.Content;
        }
        public Command Submit { get; set; }

    }
}
