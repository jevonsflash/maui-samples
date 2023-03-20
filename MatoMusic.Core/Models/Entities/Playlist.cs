namespace MatoMusic.Core.Models.Entities
{
    public class Playlist 
    {
        public Playlist()
        {

        }
        public Playlist(string name, bool isHidden, bool isRemovable)
        {
            Title = name;
            IsHidden = isHidden;
            IsRemovable = isRemovable;
        }

        public long Id { get; set; }
        public string Title { get; set; }

        public bool IsHidden { get; set; }

        public bool IsRemovable { get; set; }

        public ICollection<PlaylistItem> PlaylistItems { get; set; }
    }
}
