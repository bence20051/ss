using InformatikaiEszkozok_LIB;
using InformatikaiEszkozok_LIB.DTO;

// =====================================================
//  KONZOLOS APP - menü és funkciók
//  Most már DTO-kkal dolgozik a régi View* osztályok helyett.
// =====================================================

while (true)
{
    Console.Clear();
    Console.WriteLine("Válasszon egy menüpontot!\n");
    Console.WriteLine("1. Termékek listája és készlet mennyisége");
    Console.WriteLine("2. Készleten lévő termékek értéke terméknév alapján és összérték");
    Console.WriteLine("3. Rendelések listája, legnagyobb és legkisebb értékű rendelés tételei");
    Console.WriteLine("0. Kilépés");
    Console.Write("\nVálasztás: ");

    var valasztas = Console.ReadLine();

    switch (valasztas)
    {
        case "1":
            await TermekListaKiir();
            break;

        case "2":
            await KeszletListaKiir();
            break;

        case "3":
            await RendelesekLekerdezes();
            break;

        case "0":
            return;
    }
}

// =====================================================
//  4. feladat: Termékek listája
// =====================================================
static async Task TermekListaKiir()
{
    var termekek = await Worker.TermekLista();

    Console.Clear();
    Console.WriteLine("Termékek listája\n");

    Console.WriteLine($"{"Id",-3} {"Nev",-25} {"Keszlet",5}");
    foreach (var t in termekek)
    {
        Console.WriteLine($"{t.Id,-3} {t.Nev,-25} {t.Keszlet,5} db");
    }

    Console.WriteLine("\nTovábblépéshez üssön Entert!");
    Console.ReadLine();
}

// =====================================================
//  5. feladat: Készletérték
// =====================================================
static async Task KeszletListaKiir()
{
    var termekek = await Worker.KeszletLista();

    Console.Clear();
    Console.WriteLine("Készletérték\n");
    Console.WriteLine($"{"Id",-3} {"Nev",-28} {"Értéke",15}");

    foreach (var t in termekek)
    {
        Console.WriteLine($"{t.Id,-3} {t.Nev,-28} {t.Ertek,15:N0} Ft");
    }

    Console.WriteLine();
    Console.WriteLine($"{"",-4} {"Összérték:",-27} {termekek.Sum(t => t.Ertek),15:N0} Ft");

    Console.WriteLine("\nTovábblépéshez üssön Entert!");
    Console.ReadLine();
}

// =====================================================
//  6. feladat: Rendelések, legnagyobb és legkisebb értékű
// =====================================================
static async Task RendelesekLekerdezes()
{
    var rendelesek = await Worker.RendelesLista();

    if (rendelesek.Count == 0)
    {
        Console.WriteLine("Nincs rendelés az adatbázisban.");
        Console.ReadLine();
        return;
    }

    var legnagyobb = rendelesek.OrderByDescending(r => r.RendelesErtek).First();
    var legkisebb  = rendelesek.OrderBy(r => r.RendelesErtek).First();

    var legnagyobbTetelek = await Worker.RendelesTetelek(legnagyobb.Id);
    var legkisebbTetelek  = await Worker.RendelesTetelek(legkisebb.Id);

    Console.Clear();

    RendelesKiir("Legnagyobb összegű vásárlás:", legnagyobb, legnagyobbTetelek);
    Console.WriteLine();
    Console.WriteLine();
    RendelesKiir("Legkisebb összegű vásárlás:",  legkisebb,  legkisebbTetelek);

    Console.WriteLine();
    Console.WriteLine("Továbblépéshez üssön Entert!");
    Console.ReadLine();
}

// =====================================================
//  6.1 Egy rendelés részleteinek kiírása
// =====================================================
static void RendelesKiir(string cim, RendelesOsszesitesDto rendeles, List<RendelesTetelDto> tetelek)
{
    Console.WriteLine(cim);
    Console.WriteLine();

    Console.WriteLine($"{"rendelésszám",-25} {rendeles.Id}");
    Console.WriteLine($"{"kedvezmény azonosító",-25} {rendeles.KedvezmenyAzonosito}");
    Console.WriteLine($"{"Név",-25} {rendeles.Nev}");
    Console.WriteLine($"{"telefon",-25} {rendeles.Telefon}");
    Console.WriteLine($"{"email",-25} {rendeles.Email}");
    Console.WriteLine();

    Console.WriteLine($"{"terméknév",-20} {"mennyiség",10} {"egységár",14} {"kedvezmény",14} {"ár",14}");

    foreach (var tetel in tetelek)
    {
        string kedvezmeny = tetel.Kedvezmeny == 0
            ? "-"
            : $"{tetel.Kedvezmeny}%";

        Console.WriteLine(
            $"{tetel.Termeknev,-20} " +
            $"{tetel.Mennyiseg,8} db " +
            $"{tetel.Egysegar,12:N0} Ft " +
            $"{kedvezmeny,14} " +
            $"{tetel.Ar,12:N0} Ft"
        );
    }
    Console.WriteLine();

    if (rendeles.Kedvezmeny > 0)
    {
        Console.WriteLine($"{rendeles.Kedvezmeny}% kedvezmény érvényes a rendelésre.");
    }

    Console.WriteLine(
        $"{"Végösszeg:",-50}" +
        $"{rendeles.RendelesErtek,26:N0} Ft"
    );
}
