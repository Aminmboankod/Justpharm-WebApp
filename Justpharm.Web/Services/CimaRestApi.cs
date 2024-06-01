using Justpharm.Library.Models.Medical;
using System.Text.Json;

namespace Justpharm.Web.Services
{
    public class CimaRestApi
    {
        private static readonly string APIURICIMA = "https://cima.aemps.es/cima/rest";
        private static HttpClient _httpClient;

        private static void SetUri()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(APIURICIMA);
        }

        /// <summary>
        /// Obtiene los medicamentos de CIMA
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        public static async Task<List<CimaResponse>?> GetMedicamentos()
        {
            try
            {
                SetUri();
                HttpResponseMessage? response = await _httpClient.GetAsync($"/cima/rest/medicamentos?comerc=1");
                _httpClient.Dispose();
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    string? jsonresponse = await response.Content.ReadAsStringAsync();

                    JsonDocument doc = JsonDocument.Parse(jsonresponse);
                    JsonElement root = doc.RootElement;
                    JsonElement medicamentos = root.GetProperty("resultados");
                    string? resultados = medicamentos.GetRawText();
                    Stream? responseStream = await response.Content.ReadAsStreamAsync();
                    List<CimaResponse>? cimaResponse = JsonSerializer.Deserialize<List<CimaResponse>>(resultados);
                    return cimaResponse;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw new HttpRequestException("Error al obtener los medicamentos de CIMA", ex);
            }
        }
        public static async Task<List<CimaResponse>> GetAllFarmacos(int pagina)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<CimaResponse>>($"{APIURICIMA}/medicamentos?comerc=1");
                return response!;
            }
            catch (Exception ex)
            {
                throw new HttpRequestException("Error al obtener los medicamentos de CIMA", ex);
            }
        }
        public static async Task<List<CimaResponse>?> GetFarmacosFilter(string filter)
        {
            try
            {
                SetUri();
                HttpResponseMessage? response = await _httpClient.GetAsync(string.Format("/cima/rest/medicamentos?nombre={0}", filter));
                _httpClient.Dispose();

                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    string? jsonresponse = await response.Content.ReadAsStringAsync();

                    JsonDocument doc = JsonDocument.Parse(jsonresponse);
                    JsonElement root = doc.RootElement;
                    JsonElement medicamentos = root.GetProperty("resultados");
                    string? resultados = medicamentos.GetRawText();
                    List<CimaResponse>? cimaResponse = JsonSerializer.Deserialize<List<CimaResponse>>(resultados);
                    return cimaResponse;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw new HttpRequestException("Error al obtener los medicamentos de CIMA", ex);
            }
        }
        public static async Task<string?> GetFichaTecnicaHTML(string nRegistro)
        {
            try
            {
                SetUri();
                HttpResponseMessage? response = await _httpClient.GetAsync($"/cima/dochtml/p/{nRegistro}/FichaTecnica.html");
                _httpClient.Dispose();
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    string? htmlResponse = await response.Content.ReadAsStringAsync();
                    return htmlResponse;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw new HttpRequestException("Error al obtener los medicamentos de CIMA", ex);
            }
        }
        public static async Task<CimaResponse?> GetFotoFarmaco(string cn)
        {
            try
            {

                var response = await _httpClient.GetAsync($"{APIURICIMA}/medicamentos?comerc=1");
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var jsonresponse = await response.Content.ReadAsStringAsync();

                    JsonDocument doc = JsonDocument.Parse(jsonresponse);
                    JsonElement root = doc.RootElement;
                    var medicamentos = root.GetProperty("resultados");
                    var resultados = medicamentos.GetRawText();
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var cimaResponse = JsonSerializer.Deserialize<List<CimaResponse>>(resultados);
                    return cimaResponse!.FirstOrDefault();
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw new HttpRequestException("Error al obtener los medicamentos de CIMA", ex);
            }
        }
        public static Task<CimaResponse?> GetFarmaco(string cn)
        {
            throw new NotImplementedException();
        }
    }
}
