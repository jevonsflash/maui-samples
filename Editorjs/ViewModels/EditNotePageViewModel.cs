
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
            GoToState = new Command(GoToStateAction);

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




        private void GoToStateAction(object obj)
        {
            if (obj is NoteSegmentState)
            {

                this.NoteSegmentState=(NoteSegmentState)obj;

            }
        }



        private void Init(Note note)
        {
            if (note != null)
            {
                Title = note.Title;
                Desc = note.Desc;
                Icon = note.Icon;
                Color = note.Color;
                BackgroundColor = note.BackgroundColor;
                PreViewContent = note.PreViewContent;
                IsEditable = note.IsEditable;
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



        private string _desc;

        public string Desc
        {
            get { return _desc; }
            set
            {
                _desc = value;
                OnPropertyChanged();
            }
        }

        private string _icon;

        public string Icon
        {
            get { return _icon; }
            set
            {
                _icon = value;
                OnPropertyChanged();
            }
        }

        private string _color;

        public string Color
        {
            get { return _color; }
            set
            {
                _color = value;
                OnPropertyChanged();
            }
        }

        private string _backgroundColor;


        public string BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                _backgroundColor = value;
                OnPropertyChanged();
            }
        }


        private string _preViewContent;

        public string PreViewContent
        {
            get { return _preViewContent; }
            set
            {
                _preViewContent = value;
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

        private bool _isEditable;

        public bool IsEditable
        {
            get { return _isEditable; }
            set
            {
                _isEditable = value;
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


        private bool _popupLoading;

        public bool PopupLoading
        {
            get { return _popupLoading; }
            set
            {
                _popupLoading = value;
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
            note.Desc = this.Desc;
            note.Icon = this.Icon;
            note.Color = this.Color;
            note.BackgroundColor = this.BackgroundColor;
            note.PreViewContent = this.PreViewContent;
            note.IsEditable = this.IsEditable;
            note.Content = this.Content;
        }




        public Command Submit { get; set; }

        public Command GoToState { get; set; }



    }
}
