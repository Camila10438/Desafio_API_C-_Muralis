using ClienteApiFinal.Services.Model;
using System.Text.Json;

namespace ClienteApiFinal.Services
{
    public class ViaCEPService
    {
        private const string JsonUrlCep = "/ws/{0}/json";
        private HttpClient _httpClient;

        public ViaCEPService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://viacep.com.br/ws");
        }

        public async Task<EnderecoCep> GetCep(string cep)
        {
            var responseMessage = await _httpClient.GetAsync(new Uri(String.Format(JsonUrlCep, cep.Replace("-", "")), UriKind.Relative));
            responseMessage.EnsureSuccessStatusCode();
            var content = await responseMessage.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<EnderecoCep>(content);
            if (result?.Erro != false)
            {
                throw new Exception("Falha no viacep", new Exception($"Resposta: {responseMessage.StatusCode} - {content}"));
            }
            return result;
        }
    }
}
