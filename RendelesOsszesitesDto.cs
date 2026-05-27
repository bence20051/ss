namespace InformatikaiEszkozok_LIB.DTO
{
    /// <summary>
    /// Rendelés összesítés DTO.
    /// Több táblából JOIN-nal összeállított adat:
    /// - Rendeles (Id, VasarloId)
    /// - Vasarlo (Nev, Email, Telefon, Azonosito)
    /// - RendelesTetel + Termek (összérték számításhoz)
    /// Helyettesíti a régi SQL View-t és ViewRendelesOsszesites entitást.
    /// NEM entitás - nem szerepel a DbContext-ben.
    /// </summary>
    public class RendelesOsszesitesDto
    {
        public int Id { get; set; }
        public string? KedvezmenyAzonosito { get; set; }
        public string? Nev { get; set; }
        public string? Telefon { get; set; }
        public string? Email { get; set; }
        public int? Kedvezmeny { get; set; }
        public int? RendelesErtek { get; set; }
    }
}
