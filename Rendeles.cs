using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformatikaiEszkozok_LIB.MODEL
{
    /// <summary>
    /// A rendelés fő adatait tartalmazza.
    /// </summary>
    public class Rendeles
    {
        [Key]
        public int Id { get; set; }
        public int VasarloId { get; set; }
        public DateOnly Datum { get; set; }
    }
}
