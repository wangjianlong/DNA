using DNA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DNA.Tools
{
    public class GainOne:GainBase,IGain
    {
        public GainOne(string SourcePath)
        {
            this.SourcePath = SourcePath;
        }

        public void Work()
        {

        }

    }
}
