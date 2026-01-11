namespace Ciciovan_Bogdan_Ionut_HotelReservations.Models
{
    public class PredictionRequest
    {
        public int LeadTime { get; set; }
        public int NoOfWeekendNights { get; set; }
        public int NoOfWeekNights { get; set; }
        public int NoOfAdults { get; set; }
        public int NoOfChildren { get; set; }
        public bool RequiredCarParking { get; set; }
        public int NoOfSpecialRequests { get; set; }
        public float AvgPricePerRoom { get; set; }
        public bool IsRepeatedGuest { get; set; }
        public int NoPreviousCancellations { get; set; }
        public int NoPreviousBookings { get; set; }
        public string RoomTypeReserved { get; set; }
        public string TypeOfMealPlan { get; set; }
        public string MarketSegmentType { get; set; }
    }
}