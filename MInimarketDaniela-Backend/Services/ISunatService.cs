using MInimarketDaniela_Backend.DTOs;

namespace MInimarketDaniela_Backend.Services
{
    public interface ISunatService
    {
        Task<SunatResponseDto?> ConsultaRucAsync(string ruc);
    }
}
