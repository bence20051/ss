using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace InformatikaiEszkozok_LIB.DATA
{
    public static class ApiClient // sttikus osztály fontos
    {
        private static readonly HttpClient client = new HttpClient
        {                          // Az API alap URL-je, amelyre a kéréseket küldjük.
            BaseAddress = new Uri("https://localhost:44324/api/")
        };

        /// <summary>
        /// Generikus metódus egy adott típusú lista lekérésére egy megadott végpontról.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="vegpont"></param>
        /// <returns>A lista a megadott típusú elemekkel.</returns>
        /// <példa>var termekek = await ApiClient.GetListaAsync<TermekListaDto>("Termek/TermekLista");</példa>
        public static async Task<List<T>> GetListaAsync<T>(string vegpont)
        {
            var lista = await client.GetFromJsonAsync<List<T>>(vegpont);

            return lista ?? new List<T>();
        }

        /// <summary>
        /// Generikus metódus egy adott típusú objektum lekérésére egy megadott végpontról.
        /// NEM LISTA, hanem egyetlen objektum lekérése, például egy termék 
        /// vagy vásárló részletes adatai.
        /// Ezt használjuk pl. egy vásárló ellenőrzésére.
        /// </summary>
        /// <typeparam name="T">A lekért objektum típusa.</typeparam>
        /// <param name="vegpont">Az API végpont.</param>
        /// <returns>A lekért objektum, vagy null, ha nincs találat.</returns>
        public static async Task<T?> GetAsync<T>(string vegpont)
        {
            try
            {
                return await client.GetFromJsonAsync<T>(vegpont);
            }
            catch (HttpRequestException)
            {
                return default;
            }
        }

        /// <summary>
        /// Beszúrás kérés küldése az API felé JSON adattal.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="vegpont"></param>
        /// <param name="adat"></param>
        /// <returns>HttpResponseMessage</returns>
        public static async Task<HttpResponseMessage> PostAsync<T>(string vegpont, T adat)
        {
            return await client.PostAsJsonAsync(vegpont, adat);
        }

        /// <summary>
        /// Módosítás kérés küldése az API felé JSON adattal.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="vegpont"></param>
        /// <param name="adat"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> PutAsync<T>(string vegpont, T adat)
        {
            return await client.PutAsJsonAsync(vegpont, adat);
        }

        /// <summary>
        /// Törlés kérés küldése az API felé egy megadott végpontra.
        /// </summary>
        /// <param name="vegpont"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> DeleteAsync(string vegpont)
        {
            return await client.DeleteAsync(vegpont);
        }
    }
}