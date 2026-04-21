namespace MInimarketDaniela_Backend.DTOs
{
    public class DashboardStatsDto
    {
        public StatPeriod Daily { get; set; } = new StatPeriod();
        public StatPeriod Weekly { get; set; } = new StatPeriod();
        public StatPeriod Monthly { get; set; } = new StatPeriod();

        public List<TopProductDto> TopProducts { get; set; } = new List<TopProductDto>();
        public List<ChartDataDto> ChartData { get; set; } = new List<ChartDataDto>();
    }

    public class StatPeriod
    {
        public decimal Ingresos { get; set; }
        public decimal IngresosCambio { get; set; }
        public int Ventas { get; set; }
        public int VentasCambio { get; set; }
        public decimal TicketPromedio { get; set; }
        public int ProductosBajos { get; set; }
    }
}
