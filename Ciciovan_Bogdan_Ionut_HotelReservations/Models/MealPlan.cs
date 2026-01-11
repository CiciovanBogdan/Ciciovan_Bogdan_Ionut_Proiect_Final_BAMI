using System.ComponentModel.DataAnnotations;

namespace Ciciovan_Bogdan_Ionut_HotelReservations.Models
{
    public class MealPlan
    {
        [Key]
        public int MealPlanId { get; set; }

        [Required(ErrorMessage = "Numele planului este obligatoriu")]
        [StringLength(50)]
        [Display(Name = "Plan masa")]
        public string PlanName { get; set; }

        [StringLength(200)]
        [Display(Name = "Descriere")]
        public string? Description { get; set; }

        [Required]
        [Range(0, 500.00)]
        [Display(Name = "Pret per persoana")]
        [DataType(DataType.Currency)]
        public decimal PricePerPerson { get; set; }

        [Display(Name = "Activ?")]
        public bool IsActive { get; set; } = true;

        public ICollection<Booking>? Bookings { get; set; }
    }
}