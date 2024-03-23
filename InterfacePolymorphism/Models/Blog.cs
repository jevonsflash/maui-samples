using System.ComponentModel;

namespace Lession1.Models
{
    public enum BlogState
    {
        Edit,
        PreView
    }
    public class Blog : INotifyPropertyChanged
    {
        public Blog()
        {
            PostTime = DateTime.Now;
            State = BlogState.Edit;
            SwitchState = new Command(SwitchStateAction);
        }

        private void SwitchStateAction(object obj)
        {
            this.State = this.State == BlogState.Edit ? BlogState.PreView : BlogState.Edit;
            NotifyPropertyChanged(nameof(State));
        }

        public Guid NoteId { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public BlogState State { get; set; }
        public string Content { get; set; }
        public List<string> Images { get; set; }
        public DateTime PostTime { get; set; }

        public bool IsHidden { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public Command SwitchState { get; set; }

    }
}
