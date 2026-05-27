using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformatikaiEszkozok_LIB.MODEL
{
    /// <summary>
    /// A vásárlók adatait tartalmazza.
    /// </summary>
    public class Vasarlo
    {
        [Key] // Az egyedi azonosító mező jelölése kulcsként
        public int Id { get; set; }
        public string? Azonosito { get; set; } // A kérdőjel azt jelzi, hogy ez a mező lehet null értékű
        public string? Nev { get; set; }
        public string? Email { get; set; }
        public string? Telefon { get; set; }
    }
}
