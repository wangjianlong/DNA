using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DNA.Models
{
    public interface ITool
    {
        string GetSheetName();
        void Doing();
        void Working();
        void Write(ref ISheet Sheet);
    }
}
