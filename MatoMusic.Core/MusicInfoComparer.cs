using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatoMusic.Core
{
    public class MusicInfoComparer : IEqualityComparer<MusicInfo>
    {
        public bool Equals(MusicInfo x, MusicInfo y)
        {
            return x.Id == y.Id && (bool)x.Url?.Equals(y.Url);
        }

        public int GetHashCode(MusicInfo obj)
        {
            return (int)obj.Id;
        }
    }
}
