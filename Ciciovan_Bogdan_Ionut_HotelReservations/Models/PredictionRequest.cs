using System.Text.Json.Serialization;

namespace Ciciovan_Bogdan_Ionut_HotelReservations.Models
{
    public class PredictionRequest
    {
        [JsonPropertyName("no_of_adults")]
        public float No_of_adults
        {
            get => _no_of_adults;
            set => _no_of_adults = value;
        }
        private float _no_of_adults = 2;

        [JsonPropertyName("no_of_children")]
        public float No_of_children
        {
            get => _no_of_children;
            set => _no_of_children = value;
        }
        private float _no_of_children = 0;

        [JsonPropertyName("repeated_guest")]
        public float Repeated_guest
        {
            get => _repeated_guest;
            set => _repeated_guest = value;
        }
        private float _repeated_guest = 0;

        [JsonPropertyName("no_of_previous_cancellations")]
        public float No_of_previous_cancellations
        {
            get => _no_of_previous_cancellations;
            set => _no_of_previous_cancellations = value;
        }
        private float _no_of_previous_cancellations = 0;

        [JsonPropertyName("no_of_previous_bookings_not_canceled")]
        public float No_of_previous_bookings_not_canceled
        {
            get => _no_of_previous_bookings_not_canceled;
            set => _no_of_previous_bookings_not_canceled = value;
        }
        private float _no_of_previous_bookings_not_canceled = 0;

        [JsonPropertyName("no_of_weekend_nights")]
        public float No_of_weekend_nights
        {
            get => _no_of_weekend_nights;
            set => _no_of_weekend_nights = value;
        }
        private float _no_of_weekend_nights = 2;

        [JsonPropertyName("no_of_week_nights")]
        public float No_of_week_nights
        {
            get => _no_of_week_nights;
            set => _no_of_week_nights = value;
        }
        private float _no_of_week_nights = 3;

        [JsonPropertyName("room_type_reserved")]
        public string Room_type_reserved
        {
            get => _room_type_reserved;
            set => _room_type_reserved = value;
        }
        private string _room_type_reserved = "Room_Type 1";

        [JsonPropertyName("type_of_meal_plan")]
        public string Type_of_meal_plan
        {
            get => _type_of_meal_plan;
            set => _type_of_meal_plan = value;
        }
        private string _type_of_meal_plan = "Not Selected";

        [JsonPropertyName("lead_time")]
        public float Lead_time
        {
            get => _lead_time;
            set => _lead_time = value;
        }
        private float _lead_time = 30;

        [JsonPropertyName("avg_price_per_room")]
        public float Avg_price_per_room
        {
            get => _avg_price_per_room;
            set => _avg_price_per_room = value;
        }
        private float _avg_price_per_room = 100;

        [JsonPropertyName("no_of_special_requests")]
        public float No_of_special_requests
        {
            get => _no_of_special_requests;
            set => _no_of_special_requests = value;
        }
        private float _no_of_special_requests = 0;

        [JsonPropertyName("market_segment_type")]
        public string Market_segment_type
        {
            get => _market_segment_type;
            set => _market_segment_type = value;
        }
        private string _market_segment_type = "Online";

        [JsonPropertyName("required_car_parking_space")]
        public float Required_car_parking_space
        {
            get => _required_car_parking_space;
            set => _required_car_parking_space = value;
        }
        private float _required_car_parking_space = 0;

        [JsonPropertyName("arrival_year")]
        public int Arrival_year { get; set; } = DateTime.Now.Year;

        [JsonPropertyName("arrival_month")]
        public int Arrival_month
        {
            get => _arrival_month;
            set => _arrival_month = value;
        }
        private int _arrival_month = DateTime.Now.Month;

        [JsonPropertyName("arrival_date")]
        public int Arrival_date { get; set; } = 15;

        [JsonPropertyName("booking_status")]
        public string Booking_status { get; set; } = "Not_Canceled";

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
        public int LeadTime
        {
            get => (int)Lead_time;
            set => Lead_time = value;
        }

        [JsonIgnore]
        public decimal AvgPricePerRoom
        {
            get => (decimal)Avg_price_per_room;
            set => Avg_price_per_room = (float)value;
        }

        [JsonIgnore]
        public int NoOfSpecialRequests
        {
            get => (int)No_of_special_requests;
            set => No_of_special_requests = value;
        }

        [JsonIgnore]
        public string MarketSegmentType
        {
            get => Market_segment_type;
            set => Market_segment_type = value;
        }

        [JsonIgnore]
        public bool RequiredCarParking
        {
            get => Required_car_parking_space == 1;
            set => Required_car_parking_space = value ? 1 : 0;
        }

        [JsonIgnore]
        public int ArrivalMonth
        {
            get => Arrival_month;
            set => Arrival_month = value;
        }
    }
}