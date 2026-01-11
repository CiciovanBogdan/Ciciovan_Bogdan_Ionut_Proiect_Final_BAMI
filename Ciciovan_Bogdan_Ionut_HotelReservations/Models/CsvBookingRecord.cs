namespace Ciciovan_Bogdan_Ionut_HotelReservations.Models
{
    public class CsvBookingRecord
    {
        public string Booking_ID { get; set; }
        public int no_of_adults { get; set; }
        public int no_of_children { get; set; }
        public int no_of_weekend_nights { get; set; }
        public int no_of_week_nights { get; set; }
        public string type_of_meal_plan { get; set; }
        public int required_car_parking_space { get; set; }
        public string room_type_reserved { get; set; }
        public int lead_time { get; set; }
        public int arrival_year { get; set; }
        public int arrival_month { get; set; }
        public int arrival_date { get; set; }
        public string market_segment_type { get; set; }
        public int repeated_guest { get; set; }
        public int no_of_previous_cancellations { get; set; }
        public int no_of_previous_bookings_not_canceled { get; set; }
        public double avg_price_per_room { get; set; }
        public int no_of_special_requests { get; set; }
        public string booking_status { get; set; }
    }
}