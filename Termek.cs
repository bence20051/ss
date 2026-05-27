using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformatikaiEszkozok_LIB.MODEL
{
    /// <summary>
    /// A megvásárolható informatikai eszközök adatait tartalmazza.
    /// </summary>
    public class Termek
    {
        [Key] 
        public int Id { get; set; }
        public string? Nev { get; set; } 
        public string? Leiras { get; set; }
        public int Egysegar { get; set; }
        public int Keszlet { get; set; }
        public bool Elerheto { get; set; }
        public string? KepFajlnev { get; set; }
    }
}
