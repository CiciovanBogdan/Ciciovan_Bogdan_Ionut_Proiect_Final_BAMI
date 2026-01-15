using Microsoft.ML.Data;

namespace Ciciovan_Bogdan_Ionut_HotelReservations.Models
{
    public class BookingInput
    {
        [LoadColumn(0)] public int no_of_adults { get; set; }
        [LoadColumn(1)] public int no_of_children { get; set; }
        [LoadColumn(2)] public int no_of_weekend_nights { get; set; }
        [LoadColumn(3)] public int no_of_week_nights { get; set; }
        [LoadColumn(4)] public int type_of_meal_plan { get; set; }
        [LoadColumn(5)] public int required_car_parking_space { get; set; }
        [LoadColumn(6)] public int room_type_reserved { get; set; }
        [LoadColumn(7)] public int lead_time { get; set; }
        [LoadColumn(8)] public int arrival_month { get; set; }
        [LoadColumn(9)] public float avg_price_per_room { get; set; }
        [LoadColumn(10)] public int no_of_special_requests { get; set; }
        [LoadColumn(11)] public string market_segment_type { get; set; }
        [LoadColumn(12)] public int is_repeated_guest { get; set; }
        [LoadColumn(13)] public int no_previous_cancellations { get; set; }
        [LoadColumn(14)] public int no_previous_bookings_not_canceled { get; set; }
    }

    public class BookingPrediction
    {
        [ColumnName("PredictedLabel")]
        public string PredictedLabel { get; set; }

        [ColumnName("Score")]
        public float[] Probability { get; set; }
    }
}