namespace InformatikaiEszkozok_LIB.DTO
{
    /// <summary>
    /// Termék lista DTO (Data Transfer Object).
    /// A termékek azonosítóját, nevét és készletét tartalmazza.
    /// NEM entitás, nem szerepel a DbContext-ben - csak adattovábbításra való.
    /// A controllerben LINQ Select-tel állítjuk össze a Termek táblából.
    /// </summary>
    public class TermekListaDto
    {
        public int Id { get; set; }
        public string? Nev { get; set; }
        public int Keszlet { get; set; }
    }
}
