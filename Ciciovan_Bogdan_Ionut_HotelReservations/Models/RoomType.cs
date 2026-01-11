using System.ComponentModel.DataAnnotations;

namespace Ciciovan_Bogdan_Ionut_HotelReservations.Models
{
    public class RoomType
    {
        [Key]
        public int RoomTypeId { get; set; }

        [Required(ErrorMessage = "Numele tipului de camera este obligatoriu")]
        [StringLength(50)]
        [Display(Name = "Tip camera")]
        public string RoomTypeName { get; set; }

        [StringLength(200)]
        [Display(Name = "Descriere")]
        public string? Description { get; set; }

        [Required]
        [Range(1, 10)]
        [Display(Name = "Capacitate maxima")]
        public int MaxOccupancy { get; set; }

        [Required]
        [Range(0.01, 10000.00)]
        [Display(Name = "Pret de baza (per noapte)")]
        [DataType(DataType.Currency)]
        public decimal BasePrice { get; set; }

        [Display(Name = "Activ?")]
        public bool IsActive { get; set; } = true;

        public ICollection<Booking>? Bookings { get; set; }
    }
}