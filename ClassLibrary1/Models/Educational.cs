using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class Educational
    {
        public string CivilProfession { get; set; }

        public string Education { get; set; }

        public string Rank { get; set; }

        public DateTime DataReceiveRank { get; set; } = DateTime.MinValue;
    }
}
