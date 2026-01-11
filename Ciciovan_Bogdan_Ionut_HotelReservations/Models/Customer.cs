using System.ComponentModel.DataAnnotations;

namespace Ciciovan_Bogdan_Ionut_HotelReservations.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Prenumele este obligatoriu")]
        [StringLength(50)]
        [Display(Name = "Prenume")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Numele este obligatoriu")]
        [StringLength(50)]
        [Display(Name = "Nume")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email-ul este obligatoriu")]
        [EmailAddress(ErrorMessage = "Adresa de email nu este valida")]
        [StringLength(100)]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Numarul de telefon nu este valid")]
        [StringLength(20)]
        [Display(Name = "Telefon")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Client repetat?")]
        public bool IsRepeatedGuest { get; set; } = false;

        [Display(Name = "Numar anulari anterioare")]
        public int TotalPreviousCancellations { get; set; } = 0;

        [Display(Name = "Numar rezervari finalizate")]
        public int TotalPreviousBookings { get; set; } = 0;

        [Display(Name = "Data crearii")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public ICollection<Booking>? Bookings { get; set; }
    }
}