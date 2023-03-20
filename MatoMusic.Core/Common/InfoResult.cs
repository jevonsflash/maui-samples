using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatoMusic.Common
{
    public class InfoResult<T>
    {
        public InfoResult(bool isSucess)
        {
            IsSucess = isSucess;
            Result = default;
        }
        public InfoResult(bool isSucess, T result)
        {
            IsSucess = isSucess;
            Result = result;
        }
        public bool IsSucess { get; set; }
        public T Result { get; set; }
    }
}
