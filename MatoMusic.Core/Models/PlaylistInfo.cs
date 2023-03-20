using Microsoft.Maui.Controls;

namespace MatoMusic.Core.Models
{


    public class PlaylistInfo : MusicCollectionInfo
    {
        public bool IsHidden { get; set; }

        public bool IsRemovable { get; set; }

        public ImageSource PlaylistArt { get; set; }

    }
}