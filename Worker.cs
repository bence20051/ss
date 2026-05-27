using InformatikaiEszkozok_LIB.DATA;
using InformatikaiEszkozok_LIB.DTO;
using InformatikaiEszkozok_LIB.MODEL;

namespace InformatikaiEszkozok_LIB
{
   
    public static class Worker
    {

        public static async Task<List<TermekListaDto>> TermekLista()
        {
            return await ApiClient.GetListaAsync<TermekListaDto>("Termek/TermekLista");
        }

        public static async Task<List<KeszletListaDto>> KeszletLista()
        {
            return await ApiClient.GetListaAsync<KeszletListaDto>("Termek/KeszletLista");
        }

        public static async Task<List<Termek>> Termekek()
        {
            return await ApiClient.GetListaAsync<Termek>("Termek/Termekek");
        }
        
        public static async Task<bool> TermekFelvitel(Termek termek)
        {
            var response = await ApiClient.PostAsync("Termek/TermekFelvitel", termek);
            return response.IsSuccessStatusCode;
        }

        public static async Task<bool> TermekModositas(Termek termek)
        {
            var response = await ApiClient.PutAsync("Termek/TermekModositas", termek);
            return response.IsSuccessStatusCode;
        }

        public static async Task<bool> TermekTorles(int id)
        {
            var response = await ApiClient.DeleteAsync($"Termek/TermekTorles/{id}");
            return response.IsSuccessStatusCode;
        }

        public static async Task<List<RendelesOsszesitesDto>> RendelesLista()
        {
            return await ApiClient.GetListaAsync<RendelesOsszesitesDto>("Rendeles/RendelesLista");
        }
 
        public static async Task<List<RendelesTetelDto>> RendelesTetelek(int rendelesId)
        {
            return await ApiClient.GetListaAsync<RendelesTetelDto>(
                $"Rendeles/RendelesTetelek/{rendelesId}");
        }

        public static async Task<bool> RendelesRogzites(RendelesRogzitesAdat adat)
        {
            var response = await ApiClient.PostAsync("Rendeles/RendelesRogzites", adat);
            return response.IsSuccessStatusCode;
        }

        public static async Task<Vasarlo?> VasarloKereses(string email, string telefon)
        {
            return await ApiClient.GetAsync<Vasarlo>(
                $"Vasarlo/Kereses?email={email}&telefon={telefon}");
        }
    }
}
