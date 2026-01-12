using System.Text.Json.Serialization;

namespace Ciciovan_Bogdan_Ionut_HotelReservations.Models
{
    public class PredictionRequest
    {
        [JsonPropertyName("no_of_adults")]
        public float No_of_adults { get; set; }

        [JsonPropertyName("no_of_children")]
        public float No_of_children { get; set; }

        [JsonPropertyName("no_of_weekend_nights")]
        public float No_of_weekend_nights { get; set; }

        [JsonPropertyName("no_of_week_nights")]
        public float No_of_week_nights { get; set; }

        [JsonPropertyName("type_of_meal_plan")]
        public string Type_of_meal_plan { get; set; }

        [JsonPropertyName("required_car_parking_space")]
        public float Required_car_parking_space { get; set; }

        [JsonPropertyName("room_type_reserved")]
        public string Room_type_reserved { get; set; }

        [JsonPropertyName("lead_time")]
        public float Lead_time { get; set; }

        [JsonPropertyName("arrival_year")]
        public float Arrival_year { get; set; } = 2026;

        [JsonPropertyName("arrival_month")]
        public float Arrival_month { get; set; } = 1;

        [JsonPropertyName("arrival_date")]
        public float Arrival_date { get; set; } = 15;

        [JsonPropertyName("market_segment_type")]
        public string Market_segment_type { get; set; }

        [JsonPropertyName("repeated_guest")]
        public float Repeated_guest { get; set; }

        [JsonPropertyName("no_of_previous_cancellations")]
        public float No_of_previous_cancellations { get; set; }

        [JsonPropertyName("no_of_previous_bookings_not_canceled")]
        public float No_of_previous_bookings_not_canceled { get; set; }

        [JsonPropertyName("avg_price_per_room")]
        public float Avg_price_per_room { get; set; }

        [JsonPropertyName("no_of_special_requests")]
        public float No_of_special_requests { get; set; }

        [JsonPropertyName("booking_status")]
        public string Booking_status { get; set; } = "Not_Canceled";

        [JsonIgnore]
        public int LeadTime
        {
            get => (int)Lead_time;
            set => Lead_time = value;
        }

        [JsonIgnore]
        public int NoOfWeekendNights
        {
            get => (int)No_of_weekend_nights;
            set => No_of_weekend_nights = value;
        }

        [JsonIgnore]
        public int NoOfWeekNights
        {
            get => (int)No_of_week_nights;
            set => No_of_week_nights = value;
        }

        [JsonIgnore]
        public int NoOfAdults
        {
            get => (int)No_of_adults;
            set => No_of_adults = value;
        }

        [JsonIgnore]
        public int NoOfChildren
        {
            get => (int)No_of_children;
            set => No_of_children = value;
        }

        [JsonIgnore]
        public bool RequiredCarParking
        {
            get => Required_car_parking_space == 1;
            set => Required_car_parking_space = value ? 1 : 0;
        }

        [JsonIgnore]
        public int NoOfSpecialRequests
        {
            get => (int)No_of_special_requests;
            set => No_of_special_requests = value;
        }

        [JsonIgnore]
        public decimal AvgPricePerRoom
        {
            get => (decimal)Avg_price_per_room;
            set => Avg_price_per_room = (float)value;
        }

        [JsonIgnore]
        public bool IsRepeatedGuest
        {
            get => Repeated_guest == 1;
            set => Repeated_guest = value ? 1 : 0;
        }

        [JsonIgnore]
        public int NoPreviousCancellations
        {
            get => (int)No_of_previous_cancellations;
            set => No_of_previous_cancellations = value;
        }

        [JsonIgnore]
        public int NoPreviousBookings
        {
            get => (int)No_of_previous_bookings_not_canceled;
            set => No_of_previous_bookings_not_canceled = value;
        }

        [JsonIgnore]
        public string RoomTypeReserved
        {
            get => Room_type_reserved;
            set => Room_type_reserved = value;
        }

        [JsonIgnore]
        public string TypeOfMealPlan
        {
            get => Type_of_meal_plan;
            set => Type_of_meal_plan = value;
        }

        [JsonIgnore]
        public string MarketSegmentType
        {
            get => Market_segment_type;
            set => Market_segment_type = value;
        }
    }
}