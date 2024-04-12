
using System.ComponentModel;

namespace Editorjs.ViewModels
{
    public interface IEditorViewModel: INotifyPropertyChanged
    {
        Command Submit { get;  }
        string Content { get;  }

        Func<Task<string>> OnSubmitting { get; set; }
        Action<string> OnInited { get; set; }
        Action OnFocus { get; set; }

        NoteSegmentState NoteSegmentState { get;  }
    }
}