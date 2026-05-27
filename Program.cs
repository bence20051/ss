using InformatikaiEszkozok_LIB.DATA;
using InformatikaiEszkozok_LIB.MODEL;


Console.WriteLine("Adatbázis létrehozása indul...");

using var context = new InfotermekDbContext();
context.Database.EnsureDeleted(); // Ez a sor törli az adatbázist, ha már létezik, így mindig tiszta állapotból indulunk
context.Database.EnsureCreated(); // Ez a sor létrehozza az adatbázist a modell alapján, ha még nem létezik

Console.WriteLine("Adatok beolvasása");
Console.WriteLine();

// ======================
// 1. VÁSÁRLÓ BETÖLTÉS
// ======================
Console.WriteLine("Vásárlók beolvasása...");
var vasarlok = File.ReadLines("vasarlo.csv") // Soronként olvassuk be a fájlt (memóriabarát)
.Skip(1) // fejléc kihagyása
.Select(line =>
{                                      // Megtartja a pontos helyet, még ha üres is (pl. azonosító hiánya esetén)
    var parts = line.Split(';', StringSplitOptions.None);

    return new Vasarlo
    {
        Azonosito = string.IsNullOrWhiteSpace(parts[1]) ? "" : parts[1], // Ha az azonosító hiányzik, akkor egy szóközt adunk helyette
        Nev = parts[2],
        Telefon = parts[3],
        Email = parts[4]
    };
})
.ToList();

context.Vasarlo.AddRange(vasarlok);
context.SaveChanges();
Console.WriteLine();

// ======================
// 2. TERMÉK BETÖLTÉS
// ======================
Console.WriteLine("Termékek beolvasása...");
var termekek = File.ReadLines("termek.csv") // Soronként olvassuk be a fájlt (memóriabarát)
.Skip(1) // fejléc kihagyása
.Select(line =>
{                                      // Megtartja a pontos helyet, még ha üres is (pl. azonosító hiánya esetén)
    var parts = line.Split(';', StringSplitOptions.None);

    return new Termek
    {
        Nev = parts[1],
        Leiras = parts[2],
        Egysegar = int.Parse(parts[3]),
        Keszlet = int.Parse(parts[4]),
        Elerheto = parts[5] == "1" ? true : false,
        KepFajlnev = parts[6]
    };
})
.ToList();

context.Termek.AddRange(termekek);
context.SaveChanges();

// ======================
// 3. RENDELÉS BETÖLTÉS
// ======================
Console.WriteLine("Rendelések beolvasása...");
var rendelesek = File.ReadLines("rendeles.csv") 
.Skip(1) 
.Select(line =>
{                                     
    var parts = line.Split(';', StringSplitOptions.None);

    return new Rendeles
    {
        VasarloId = int.Parse(parts[1]),
        Datum = DateOnly.Parse(parts[2])
    };
})
.ToList();

context.Rendeles.AddRange(rendelesek);
context.SaveChanges();
Console.WriteLine();

// ======================
// 4. RENDELÉS TÉTELEK BETÖLTÉS
// ======================
Console.WriteLine("Rendelések tételei beolvasása...");
var rendelestetelek = File.ReadLines("rendeles_tetel.csv") 
.Skip(1) 
.Select(line =>
{                                     
    var parts = line.Split(';', StringSplitOptions.None);

    return new RendelesTetel
    {
        RendelesId = int.Parse(parts[1]),
        TermekId = int.Parse(parts[2]),
        Mennyiseg = int.Parse(parts[3])
    };
})
.ToList();

context.RendelesTetel.AddRange(rendelestetelek);
context.SaveChanges();
Console.WriteLine();

Console.WriteLine("Adatbázis létrehozva.");