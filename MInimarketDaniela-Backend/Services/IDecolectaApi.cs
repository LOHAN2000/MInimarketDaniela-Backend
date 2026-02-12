using MInimarketDaniela_Backend.DTOs;
using Refit;

namespace MInimarketDaniela_Backend.Services
{
    public interface IDecolectaApi
    {
        [Get("/sunat/ruc")]
        Task<SunatResponseDto> ObtenerRucAsync([AliasAs("numero")] string ruc);
    }
}
