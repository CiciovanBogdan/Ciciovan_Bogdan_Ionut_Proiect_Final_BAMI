using System.ComponentModel.DataAnnotations;

namespace Ciciovan_Bogdan_Ionut_HotelReservations.Models
{
    public class PredictionHistory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string PredictedLabel { get; set; }

        public float Probability { get; set; }

        [Required]
        public string RiskLevel { get; set; }

        public string Recommendation { get; set; }

        public string RequestDataJson { get; set; }

        public string CustomerEmail { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}