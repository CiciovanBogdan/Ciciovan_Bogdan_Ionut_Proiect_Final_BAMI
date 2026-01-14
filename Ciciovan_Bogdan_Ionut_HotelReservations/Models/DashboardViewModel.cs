namespace Ciciovan_Bogdan_Ionut_HotelReservations.Models
{
    public class DashboardViewModel
    {
        public int TotalBookings { get; set; }
        public int ActiveBookings { get; set; }
        public int CanceledBookings { get; set; }
        public int CompletedBookings { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal AverageBookingValue { get; set; }

        public List<StatusStat> StatusStats { get; set; } = new();
        public List<RoomTypeStat> RoomTypeStats { get; set; } = new();
        public List<MonthStat> MonthlyStats { get; set; } = new();

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class StatusStat
    {
        public string Status { get; set; } = string.Empty;
        public int Count { get; set; }
        public string Color { get; set; } = string.Empty;
    }

    public class RoomTypeStat
    {
        public string RoomTypeName { get; set; } = string.Empty;
        public decimal TotalRevenue { get; set; }
        public int Count { get; set; }
    }

    public class MonthStat
    {
        public string Month { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}