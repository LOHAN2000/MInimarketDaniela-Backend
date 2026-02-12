using MInimarketDaniela_Backend.DTOs;
using Refit;

namespace MInimarketDaniela_Backend.Services
{
    public class SunatService : ISunatService
    {
        private readonly IDecolectaApi _sunatApi;

        public SunatService(IDecolectaApi sunatApi)
        {
            _sunatApi = sunatApi;
        }

        public async Task<SunatResponseDto?> ConsultaRucAsync(string ruc)
        {
            try
            {
                var datosSunat = await _sunatApi.ObtenerRucAsync(ruc);

                return datosSunat;

                
            }
            catch (ApiException ex)
            {
                // Si la API responde 404 (No encontrado)
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                // Si es otro error (ej. 401 Token Inválido o 500 Error del servidor externo)
                // Lo logueamos o lo relanzamos. Por ahora retornamos null para no romper el flujo.
                Console.WriteLine($"Error en API SUNAT: {ex.Content}");
                return null;
            }
            catch (Exception ex)
            {
                // Error de conexión (Internet, DNS, etc.)
                Console.WriteLine($"Error crítico: {ex.Message}");
                return null;
            }
        }

    }
}
