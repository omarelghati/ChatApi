using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApi.Uitilities
{
    public static class Extensions
    {
        public static bool NotNull(this object data)
        {
            return data != null;
        }

        public static bool IsNull(this object data)
        {
            return data == null;
        }
    }
}
