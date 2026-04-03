using Microsoft.EntityFrameworkCore;
using MInimarketDaniela_Backend.DataAccess;
using MInimarketDaniela_Backend.DTOs;

namespace MInimarketDaniela_Backend.Services
{
    public class DashBoardService : IDashboardService
    {
        private readonly MinimarketContext _context;

        public DashBoardService(MinimarketContext context)
        {
            _context = context;
        }

        public async Task<DashboardStatsDto> GetDashboardStatsAsync()
        {
            var todayUtc = DateTime.SpecifyKind(DateTime.UtcNow.Date, DateTimeKind.Utc);
            int diff = (7 + (todayUtc.DayOfWeek - DayOfWeek.Monday)) % 7;
            var startOfWeekUtc = DateTime.SpecifyKind(todayUtc.AddDays(-1 * diff), DateTimeKind.Utc);
            var startOfMonthUtc = new DateTime(todayUtc.Year, todayUtc.Month, 1, 0, 0, 0, DateTimeKind.Utc);

            var yesterdayUtc = todayUtc.AddDays(-1);
            var startOfLastWeekUtc = startOfWeekUtc.AddDays(-7);
            var startOfLastMonthUtc = startOfMonthUtc.AddMonths(-1);

            var allSales = await _context.Sales
                .Where(s => !s.IsDeleted && s.CreatedAt >= startOfLastMonthUtc)
                .ToListAsync();

            var lowStockCount = await _context.Products.CountAsync(p => !p.IsDeleted && p.Stock <= 10);

            var stats = new DashboardStatsDto();

            var todaySales = allSales.Where(s => s.CreatedAt.Date == todayUtc).ToList();
            var yesterdaySales = allSales.Where(s => s.CreatedAt.Date == yesterdayUtc).ToList();

            stats.Daily.Ventas = todaySales.Count;
            stats.Daily.Ingresos = todaySales.Sum(s => s.Total);
            stats.Daily.TicketPromedio = stats.Daily.Ventas > 0 ? stats.Daily.Ingresos / stats.Daily.Ventas : 0;
            stats.Daily.ProductosBajos = lowStockCount;
            stats.Daily.IngresosCambio = CalcularPorcentaje(stats.Daily.Ingresos, yesterdaySales.Sum(s => s.Total));
            stats.Daily.VentasCambio = (int)CalcularPorcentaje(stats.Daily.Ventas, yesterdaySales.Count);

            var thisWeekSales = allSales.Where(s => s.CreatedAt >= startOfWeekUtc).ToList();
            var lastWeekSales = allSales.Where(s => s.CreatedAt >= startOfLastWeekUtc && s.CreatedAt < startOfWeekUtc).ToList();

            stats.Weekly.Ventas = thisWeekSales.Count;
            stats.Weekly.Ingresos = thisWeekSales.Sum(s => s.Total);
            stats.Weekly.TicketPromedio = stats.Weekly.Ventas > 0 ? stats.Weekly.Ingresos / stats.Weekly.Ventas : 0;
            stats.Weekly.ProductosBajos = lowStockCount;
            stats.Weekly.IngresosCambio = CalcularPorcentaje(stats.Weekly.Ingresos, lastWeekSales.Sum(s => s.Total));
            stats.Weekly.VentasCambio = (int)CalcularPorcentaje(stats.Weekly.Ventas, lastWeekSales.Count);

            var thisMonthSales = allSales.Where(s => s.CreatedAt >= startOfMonthUtc).ToList();
            var lastMonthSales = allSales.Where(s => s.CreatedAt >= startOfLastMonthUtc && s.CreatedAt < startOfMonthUtc).ToList();

            stats.Monthly.Ventas = thisMonthSales.Count;
            stats.Monthly.Ingresos = thisMonthSales.Sum(s => s.Total);
            stats.Monthly.TicketPromedio = stats.Monthly.Ventas > 0 ? stats.Monthly.Ingresos / stats.Monthly.Ventas : 0;
            stats.Monthly.ProductosBajos = lowStockCount;
            stats.Monthly.IngresosCambio = CalcularPorcentaje(stats.Monthly.Ingresos, lastMonthSales.Sum(s => s.Total));
            stats.Monthly.VentasCambio = (int)CalcularPorcentaje(stats.Monthly.Ventas, lastMonthSales.Count);

            return stats;
        }

        private decimal CalcularPorcentaje(decimal actual, decimal anterior)
        {
            if (anterior == 0) return actual > 0 ? 100 : 0;
            return ((actual - anterior) / anterior) * 100;
        }
    }
}
