using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformatikaiEszkozok_LIB.MODEL
{
    /// <summary>
    /// A rendeléshez tartozó
    /// konkrét termékeket és azok mennyiségét tartalmazza.
    /// </summary>
    public class RendelesTetel
    {
        [Key]
        public int Id { get; set; }
        public int RendelesId { get; set; }
        public int TermekId { get; set; }
        public int Mennyiseg { get; set; }
    }
}
