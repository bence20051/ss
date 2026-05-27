using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformatikaiEszkozok_LIB.MODEL
{
    public class RendelesRogzitesAdat
    {
        public Vasarlo? Vasarlo { get; set; }
        public List<RendelesTetel> Tetelek { get; set; } = new();
    }
}
