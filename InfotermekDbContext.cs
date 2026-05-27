using InformatikaiEszkozok_LIB.MODEL;
using Microsoft.EntityFrameworkCore;

namespace InformatikaiEszkozok_LIB.DATA
{
    /// <summary>
    /// Az Entity Framework Core DbContext osztálya.
    /// 
    /// Egy DbSet egy adatbázis táblát képvisel.
    /// FONTOS: Itt csak az igazi entitások (táblák) vannak,
    /// a régi View*-osztályokat töröltük, helyettük DTO-kat használunk
    /// a controllerekben LINQ JOIN-okkal.
    /// </summary>
    public class InfotermekDbContext : DbContext
    {
        // ---------------------------------------------
        // ENTITÁSOK = ADATBÁZIS TÁBLÁK
        // ---------------------------------------------

        public DbSet<Vasarlo> Vasarlo { get; set; }
        public DbSet<Termek> Termek { get; set; }
        public DbSet<Rendeles> Rendeles { get; set; }
        public DbSet<RendelesTetel> RendelesTetel { get; set; }

        // ---------------------------------------------
        // KAPCSOLAT BEÁLLÍTÁSA
        // ---------------------------------------------

        /// <summary>
        /// Beállítja az adatbázis kapcsolatot.
        /// Server   = az SQL szerver neve
        /// Database = az adatbázis neve
        /// Trusted_Connection = Windows authentikáció
        /// TrustServerCertificate = a fejlesztői gép tanúsítványát elfogadja
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=DESKTOP-H7QN99O; Database=InfotermekDb;Trusted_Connection=True;TrustServerCertificate=True");
        }


       


        
    }
}