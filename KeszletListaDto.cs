namespace InformatikaiEszkozok_LIB.DTO
{
    /// <summary>
    /// Készlet lista DTO.
    /// A termékek nevét és a készlet értékét (darab × egységár) tartalmazza.
    /// NEM entitás - LINQ Select-tel állítjuk össze a Termek táblából.
    /// </summary>
    public class KeszletListaDto
    {
        public int Id { get; set; }
        public string? Nev { get; set; }
        public int Ertek { get; set; }
    }
}
