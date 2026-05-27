namespace InformatikaiEszkozok_LIB.DTO
{
    /// <summary>
    /// Rendelés tétel DTO.
    /// Egy rendelés egy tételét reprezentálja, kiegészítve a termék
    /// nevével és egységárával (JOIN a Termek táblával).
    /// Helyettesíti a régi SQL View-t és ViewRendelesTetel entitást.
    /// NEM entitás - nem szerepel a DbContext-ben.
    /// </summary>
    public class RendelesTetelDto  // ????????????????
    {
        public int Id { get; set; }
        public int RendelesId { get; set; }
        public string? Termeknev { get; set; }
        public int Mennyiseg { get; set; }
        public int Egysegar { get; set; }
        public int Kedvezmeny { get; set; }
        public int Ar { get; set; }
    }
}
