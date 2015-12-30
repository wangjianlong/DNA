using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DNA.Models
{
    public interface ITool
    {
        void Working();
        void Write(ref ISheet Sheet);
    }
}
