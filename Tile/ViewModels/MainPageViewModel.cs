using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Tile.ViewModels
{

    public class MainPageViewModel : ObservableObject, INoteSegmentServiceContainer
    {

        public MainPageViewModel()
        {
            CreateSegment = new Command(CreateSegmentAction);
            RemoveSegment = new Command(RemoveSegmentAction);
            SelectAllSegment = new Command(SelectAllSegmentAction);
            RemoveSelectedSegment = new Command(RemoveSelectedSegmentAction);

            IsConfiguratingNoteSegment = false;
            SelectedNoteSegments = new ObservableCollection<object>();

        }

        private void SelectAllSegmentAction(object obj)
        {
            foreach (var noteSegments in NoteSegments)
            {
                SelectedNoteSegments.Add(noteSegments);
            }
        }

        private void RemoveSelectedSegmentAction(object obj)
        {
            foreach (var noteSegments in SelectedNoteSegments.ToList())
            {
                NoteSegments.Remove((INoteSegmentService)noteSegments);
            }
        }






        private void RemoveSegmentAction(object obj)
        {
            NoteSegments.Remove(obj as INoteSegmentService);
        }

        private async void CreateSegmentAction(object obj)
        {
            var type = obj as string;
            NoteSegment note = default;


            var noteSegment = new NoteSegment()
            {
                Title = note.Title,
                Type = type,
                Desc = note.Desc,
                Icon=note.Icon,
            };



            var newModel = new NoteSegmentService(noteSegment);
            if (newModel != null)
            {
                newModel.Create.Execute(null);
                newModel.NoteSegmentState = IsConfiguratingNoteSegment ? NoteSegmentState.Config : NoteSegmentState.Edit;
                newModel.Container = this;
                NoteSegments.Add(newModel);
            }

        }





        private long noteId;

        public long NoteId
        {
            get { return noteId; }
            set
            {
                if (noteId != value)
                {
                    noteId = value;
                    OnPropertyChanged();
                }

            }
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





        private ObservableCollection<INoteSegmentService> _noteSegments;

        public ObservableCollection<INoteSegmentService> NoteSegments
        {
            get { return _noteSegments; }
            set
            {
                _noteSegments = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<object> _selectedNoteSegments;

        public ObservableCollection<object> SelectedNoteSegments
        {
            get { return _selectedNoteSegments; }
            set
            {
                _selectedNoteSegments = value;
                OnPropertyChanged();
            }
        }

        private INoteSegmentService _selectedNoteSegment;

        public INoteSegmentService SelectedNoteSegment
        {
            get { return _selectedNoteSegment; }
            set
            {
                _selectedNoteSegment = value;
                OnPropertyChanged();
            }
        }

        private bool _isConfiguratingNoteSegment;

        public bool IsConfiguratingNoteSegment
        {
            get { return _isConfiguratingNoteSegment; }
            set
            {
                _isConfiguratingNoteSegment = value;
                OnPropertyChanged();

            }
        }





        public Command CreateSegment { get; set; }
        public Command CreateSegmentFromStore { get; set; }
        public Command Remove { get; set; }
        public Command RemoveSegment { get; set; }
        public Command RemoveSelectedSegment { get; set; }
        public Command SelectAllSegment { get; set; }


        public Command SwitchState { get; set; }



    }
}
