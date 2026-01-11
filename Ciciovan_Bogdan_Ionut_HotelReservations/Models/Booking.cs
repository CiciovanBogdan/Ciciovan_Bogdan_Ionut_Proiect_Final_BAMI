using Ciciovan_Bogdan_Ionut_HotelReservations.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ciciovan_Bogdan_Ionut_HotelReservations.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Cod rezervare")]
        public string BookingCode { get; set; }

        [Required]
        [Display(Name = "Client")]
        public int CustomerId { get; set; }

        [Required]
        [Display(Name = "Tip camera")]
        public int RoomTypeId { get; set; }

        [Display(Name = "Plan masa")]
        public int? MealPlanId { get; set; }

        [Required]
        [Display(Name = "Data sosirii")]
        [DataType(DataType.Date)]
        public DateTime ArrivalDate { get; set; }

        [Required]
        [Range(0, 30)]
        [Display(Name = "Nopti weekend")]
        public int NoOfWeekendNights { get; set; }

        [Required]
        [Range(0, 30)]
        [Display(Name = "Nopti saptamana")]
        public int NoOfWeekNights { get; set; }

        [Display(Name = "Data plecarii")]
        [DataType(DataType.Date)]
        [NotMapped]
        public DateTime CheckOutDate => ArrivalDate.AddDays(NoOfWeekendNights + NoOfWeekNights);

        [Required]
        [Range(1, 10)]
        [Display(Name = "Numar adulti")]
        public int NoOfAdults { get; set; }

        [Range(0, 10)]
        [Display(Name = "Numar copii")]
        public int NoOfChildren { get; set; }

        [Display(Name = "Parcare necesara?")]
        public bool RequiredCarParking { get; set; } = false;

        [Range(0, 10)]
        [Display(Name = "Cerinte speciale")]
        public int NoOfSpecialRequests { get; set; } = 0;

        [Required]
        [Range(0.01, 10000.00)]
        [Display(Name = "Pret mediu per camera")]
        [DataType(DataType.Currency)]
        public decimal AvgPricePerRoom { get; set; }

        [Display(Name = "Pret total")]
        [DataType(DataType.Currency)]
        [NotMapped]
        public decimal TotalPrice => AvgPricePerRoom * (NoOfWeekendNights + NoOfWeekNights);

        [Range(0, 365)]
        [Display(Name = "Zile pana la sosire")]
        public int LeadTime { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Segment piata")]
        public string MarketSegmentType { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Status rezervare")]
        public string BookingStatus { get; set; } = "Confirmed";

        [Display(Name = "Data crearii")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Ultima modificare")]
        public DateTime LastModifiedDate { get; set; } = DateTime.Now;

        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }

        [ForeignKey("RoomTypeId")]
        public RoomType? RoomType { get; set; }

        [ForeignKey("MealPlanId")]
        public MealPlan? MealPlan { get; set; }

        public ICollection<BookingHistory>? BookingHistories { get; set; }
    }
}